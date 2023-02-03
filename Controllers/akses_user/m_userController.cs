using appglobal;
using EmailClass;
using HashKey;
using jasuindo_models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using web_api_managemen_user;
using web_api_managemen_user.Class;
using web_api_managemen_user.Class.Helper;
using web_api_managemen_user.Class.Service;
using web_api_management_user.Controllers;

namespace web_api_management_user.Controllers
{
    [ApiController]
    [Route("akses_user/[controller]", Name = "Akses-User")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public class m_userController : ControllerBase
    {
        private readonly ILogger<m_userController> _logger;
        private string INVALID = "INVALID";
        private string VALID = "VALID";
        private model_user_managemen _context;
        private readonly IUriService uriService;
        public m_userController(ILogger<m_userController> logger, IConfiguration configuration, IUriService uriService)
        {
            //jwt_token._configuration = configuration;
            _logger = logger;
            _context = new model_user_managemen();
            this.uriService = uriService;
        }

        /// <summary>
        /// List data user dengan status aktif
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(nameof(list_m_user))]
        [SwaggerOperation(Tags = new[] { "Akses-User" })]
        public async Task<IActionResult> list_m_user([FromQuery] m_user user)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var query = await _context.m_user.OrderBy(e => e.create_at).OrderByDescending(e => e.create_at).ToListAsync();
                    if (user.m_user_id != Guid.Empty)
                    {
                        query =  query.Where(e => e.m_user_id == user.m_user_id).ToList();
                    }
                    if (user.m_username != null)
                    {
                        query =  query.Where(e => e.m_username == user.m_username).ToList();
                    }
                   
                    result.data = query;
                    result.message = "valid";
                    result.status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    result.data = null;
                    result.message = ex.Message;
                    result.status = false;
                    return BadRequest(result);
                }
            }
            return new JsonResult(result);
        }


        /// <summary>
        /// List data user dengan status aktif
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet(nameof(list_m_user_pagination))]
        [SwaggerOperation(Tags = new[] { "Akses-User" })]
        public async Task<IActionResult> list_m_user_pagination([FromQuery] m_user user, [FromQuery] PaginationFilter filter)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            List<m_user> query = new List<m_user>();
            var route = Request.Path.Value;
            int totalRecords = 0;

            using (var transaction = _context.Database.BeginTransaction())
            { 
                try
                {
                    query = _context.m_user.ToList();

                    if (filter.Order == "desc")
                    {
                        if (filter.OrderBy == "m_user_id") {            query = query.OrderByDescending(e => e.m_user_id).ToList(); }
                        else if (filter.OrderBy == "m_username") {      query = query.OrderByDescending(e => e.m_username).ToList(); }
                        else if (filter.OrderBy == "m_user_email") {    query = query.OrderByDescending(e => e.m_user_email).ToList(); }
                        else if (filter.OrderBy == "create_by") {       query = query.OrderByDescending(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "status") {          query = query.OrderByDescending(e => e.status).ToList(); }
                        else {                                          query = query.OrderByDescending(e => e.create_at).ToList(); }
                    }
                    else
                    {
                        if (filter.OrderBy == "m_user_id") {            query = query.OrderBy(e => e.m_user_id).ToList(); }
                        else if (filter.OrderBy == "m_username") {      query = query.OrderBy(e => e.m_username).ToList(); }
                        else if (filter.OrderBy == "m_user_email") {    query = query.OrderBy(e => e.m_user_email).ToList(); }
                        else if (filter.OrderBy == "create_by") {       query = query.OrderBy(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "status") {          query = query.OrderBy(e => e.status).ToList(); }
                        else {                                          query = query.OrderBy(e => e.create_at).ToList(); }
                    }

                    if (filter.StartDate != null) { query = query.Where(e => e.create_at >= filter.StartDate).ToList(); }
                    if (filter.EndDate != null) { query = query.Where(e => e.create_at <= filter.EndDate).ToList(); }

                    if (user.m_user_id  != Guid.Empty) { query= query.Where(e => e.m_user_id == user.m_user_id).ToList(); }
                    if (user.m_username != null) { query= query.Where(e => e.m_username == user.m_username).ToList(); }
                    if (user.m_user_email != null) { query = query.Where(e => e.m_user_email.Contains(user.m_username)).ToList(); }
                    if (user.create_by != null) { query = query.Where(e => e.create_by == user.create_by).ToList(); }
                    if (user.status == true) query = query.Where(e => e.status == true).ToList();
                    if (user.status == false) query = query.Where(e => e.status == false).ToList();
                    
                    totalRecords = query.Count();
                    query = query.Skip((validFilter.PageNumber - 1) * validFilter.PageSize).Take(validFilter.PageSize).ToList();

                    result.data = query;
                    result.message = "valid";
                    result.status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    result.data = null;
                    result.message = ex.Message;
                    result.status = false;
                    return BadRequest(result);
                }
            }

            return new JsonResult(PaginationHelper.CreatePagedReponse<m_user>(query, validFilter, totalRecords, uriService, route));
        }

        /// <summary>
        /// add data master user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost(nameof(add_m_user))]
        [SwaggerOperation(Tags = new[] { "Akses-User" })]
        public async Task<IActionResult> add_m_user([FromForm] m_user user)
        {

            if (ModelState.IsValid) ;

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                    //var name =  claims["username"];
                    //string role = User.Claims.SingleOrDefault(e=> e.Type.Contains("role")).Value;

                   
                    var valided= _context.m_user.AsNoTracking().FirstOrDefaultAsync(e => e.m_username == user.m_username);

                    if (valided.Result != null)
                    {
                        result.message = "Nama/Kode user Application Sudah Terdaftar";
                        return new JsonResult(result);
                    }
                    string password = KeyGenerator.GetPassword(user.m_user_password);
                    // data input master satuan
                    m_user data = new m_user
                    {
                        m_user_id = generate_guid(),
                        m_username = user.m_username,
                        status_aktif = user.status_aktif,
                        m_user_email = user.m_user_email,
                        ref1 = user.ref1,
                        ref2 = user.ref2,
                        ref3 = user.ref3,
                        m_user_password = password,
                        status = true,
                        create_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        create_by = ""
                    };
                    _context.m_user.Add(data);

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    result.data = data.m_user_id;
                    result.message = "Berhasil Menambah Data";
                    result.status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    transaction.Rollback();
                    result.data = null;
                    result.message = ex.Message;
                    result.status = false;
                    return BadRequest(result);
                }
            }


            return new JsonResult(result);
        }

        /// <summary>
        /// add data master user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost(nameof(register_user))]
        [SwaggerOperation(Tags = new[] { "Akses-User" })]
        public async Task<IActionResult> register_user([FromForm] m_user user)
        {

            if (ModelState.IsValid) ;

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    if (!AppGlobal.IsValidEmail(user.m_user_email))
                    {
                        result.data = null;
                        result.message = "Email tidak sesuai/terdaftar";
                        result.status = false;
                        return new JsonResult(result);
                    }

                    var validedEmail = _context.m_user.AsNoTracking().FirstOrDefaultAsync(e => e.m_user_email == user.m_user_email);

                    if (validedEmail.Result != null)
                    {
                        result.message = "Email Sudah Terdaftar";
                        return new JsonResult(result);
                    }

                    var valided = _context.m_user.AsNoTracking().FirstOrDefaultAsync(e => e.m_username == user.m_username);

                    if (valided.Result != null)
                    {
                        result.message = "Username Sudah Terdaftar";
                        return new JsonResult(result);
                    }

                    string passwordRandom = AppGlobal.CreateRandomPassword(8);
                    string password = KeyGenerator.GetPassword(passwordRandom);
                    // data input master satuan
                    m_user data = new m_user
                    {
                        m_user_id = generate_guid(),
                        m_username = user.m_username,
                        status_aktif = "aktif",
                        m_user_email = user.m_user_email,
                        m_user_password = password,
                        ref1 = user.ref1,
                        ref2 = user.ref2,
                        ref3 = user.ref3,
                        status = true,
                        create_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        create_by = ""
                    };
                    _context.m_user.Add(data);

                    

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    result.data = data.m_user_id;
                    result.message = "Registrasi Berhasil Cek Email";
                    result.status = true;
                    send_email_jtp.send_email(data, passwordRandom);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    transaction.Rollback();
                    result.data = null;
                    result.message = ex.Message;
                    result.status = false;
                    return BadRequest(result);
                }
            }
            return new JsonResult(result);
        }


        /// <summary>
        /// post change password 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost(nameof(change_password))]
        [SwaggerOperation(Tags = new[] { "Kemenag-Auth" })]
        public async Task<IActionResult> change_password([FromForm] reset_password_data data)
        {
            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;
            try
            {

                if (
                    data.Password_baru.Length >= 8 &&  //if length is >= 6
                    data.Password_baru.Any(char.IsUpper) &&  //if any character is upper case
                    data.Password_baru.Any(char.IsNumber) //better to have a digit then Number
                  )
                {
                    //valid
                }
                else
                {
                    result.message = "Password Minimal 8 karakter,huruf besar dan amgka";
                    result.status = false;
                    return new JsonResult(result);
                }

                var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                //int appID = int.Parse(claims[Constants.APPID]);
                var name = claims["username"];
                var email = User.Claims.SingleOrDefault(e => e.Type.Contains("email")).Value;
                string role = User.Claims.SingleOrDefault(e => e.Type.Contains("role")).Value;

                string passwordLama = KeyGenerator.GetPassword(data.password_lama);
                var data_user = _context.m_user.SingleOrDefault(e => e.m_username == name && e.m_user_email == email && e.m_user_password == passwordLama);
                if (data_user != null)
                {
                    result.message = "Cek username,email dan password lama";
                    result.status = false;
                    return new JsonResult(result);
                }
                data_user.m_user_password = data.Password_baru;
                data_user.status_aktif = "valid";
                _context.m_user.Update(data_user);
                await _context.SaveChangesAsync();
                result.status = true;
                result.message = VALID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result.data = null;
                result.message = ex.Message;
                result.status = false;
                return BadRequest(result);
            }

            return new JsonResult(result);
        }

        private Guid generate_guid()
        {
            bool status = false;
            Guid id = Guid.NewGuid();
            if(status == false)
            {
                var query = _context.m_user.Any(e => e.m_user_id == id);
                if(query)
                {
                    generate_guid();
                }
            }
            return id;
        }
        /// <summary>
        /// update master user
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut(nameof(update_user))]
        [SwaggerOperation(Tags = new[] { "Akses-User" })]
        public async Task<IActionResult> update_user([FromForm] m_user user)
        {
            if (ModelState.IsValid) ;

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                    //var name = claims["username"];

                    var query = _context.m_user.AsNoTracking().SingleOrDefault(e => e.m_user_id == user.m_user_id);
                    
                    if (query != null)
                    {

                        // data input m_organiasi
                        query.m_username = user.m_username;
                        query.m_user_email = user.m_user_email;
                        query.status = user.status;
                        query.ref1 = user.ref1;
                        query.ref2 = user.ref2;
                        query.ref3 = user.ref3;
                        query.update_by = "";
                        query.update_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        _context.m_user.Update(query);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        result.message = "Berhasil Menggubah Data";
                        result.status = true;
                        result.data = user.m_user_id;
                    }
                    else
                    {
                        transaction.Rollback();
                        result.message = "Data tidak di temukan";
                        result.status = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    result.data = null;
                    result.message = ex.Message;
                    result.status = false;
                    return BadRequest(result);
                }
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// delete data master user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete(nameof(delete_m_user))]
        [SwaggerOperation(Tags = new[] { "Akses-User" })]
        public async Task<IActionResult> delete_m_user(string id)
        {
            if (ModelState.IsValid) ;

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                    //var name = claims["username"];
                    var query = _context.m_user.AsNoTracking().SingleOrDefault(e => e.m_user_id.ToString() == id);
                    if (query != null)
                    {
                        // data input madrasah
                        _context.m_user.Remove(query);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        result.message = "Berhasil Menghapus Data";
                        result.status = true;
                    }
                    else
                    {
                        transaction.Rollback();
                        result.message = "Data tidak di temukan";
                        result.status = false;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    transaction.Rollback();
                    result.message = "Gagal Simpan";
                    result.status = false;
                    return BadRequest(result);
                }
            }
            return new JsonResult(result);
        }

        public  class m_user_body
        {

            public virtual int m_user_id
            {
                get;
                set;
            }

            public virtual string m_user_name
            {
                get;
                set;
            }

        }

        public class reset_password_data
        {
            [Required]
            public string password_lama { set; get; }

            [Required]
            public string Password_baru { set; get; }
        }
    }
}
