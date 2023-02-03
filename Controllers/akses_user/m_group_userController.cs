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
    [Route("akses_user/[controller]", Name = "Akses-Group-User")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public class m_group_userController : ControllerBase
    {
        private readonly ILogger<m_group_userController> _logger;
        private string INVALID = "INVALID";
        private string VALID = "VALID";
        private model_user_managemen _context;
        private readonly IUriService uriService;

        public m_group_userController(ILogger<m_group_userController> logger, IConfiguration configuration, IUriService uriService)
        {
            //jwt_token._configuration = configuration;
            _logger = logger;
            _context = new model_user_managemen();
            this.uriService = uriService;
        }

        /// <summary>
        /// List data group_user dengan status aktif
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(nameof(list_m_group_user))]
        [SwaggerOperation(Tags = new[] { "Akses-Group-User" })]
        public async Task<IActionResult> list_m_group_user([FromQuery] m_group_user_body group_user)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var query = await _context.m_group_user.OrderBy(e => e.create_at).OrderByDescending(e => e.create_at).ToListAsync();
                    if (group_user.m_group_user_id != 0)
                    {
                        query = query.Where(e => e.m_group_user_id == group_user.m_group_user_id).ToList();
                    }
                    if (group_user.m_group_user_name != null)
                    {
                        query = query.Where(e => e.m_group_user_name == group_user.m_group_user_name).ToList();
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
        /// List data group_user dengan status aktif
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(nameof(list_m_group_user_pagination))]
        [SwaggerOperation(Tags = new[] { "Akses-Group-User" })]
        public async Task<IActionResult> list_m_group_user_pagination([FromQuery] m_group_user group_user, [FromQuery] PaginationFilter filter)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            List<m_group_user> query = new List<m_group_user>();
            var route = Request.Path.Value;
            int totalRecords = 0;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    query = _context.m_group_user.ToList();

                    if (filter.Order == "desc")
                    {
                        if (filter.OrderBy == "m_group_user_id") {          query = query.OrderByDescending(e => e.m_group_user_id).ToList(); }
                        else if (filter.OrderBy == "m_group_user_name") {   query = query.OrderByDescending(e => e.m_group_user_name).ToList(); }
                        else if (filter.OrderBy == "create_by") {           query = query.OrderByDescending(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "status") {              query = query.OrderByDescending(e => e.status).ToList(); }
                        else {                                              query = query.OrderByDescending(e => e.create_at).ToList(); }
                    }
                    else
                    {
                        if (filter.OrderBy == "m_group_user_id") {          query = query.OrderBy(e => e.m_group_user_id).ToList(); }
                        else if (filter.OrderBy == "m_group_user_name") {   query = query.OrderBy(e => e.m_group_user_name).ToList(); }
                        else if (filter.OrderBy == "create_by") {           query = query.OrderBy(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "status") {              query = query.OrderBy(e => e.status).ToList(); }
                        else {                                              query = query.OrderBy(e => e.create_at).ToList(); }
                    }

                    if (filter.StartDate != null) { query = query.Where(e => e.create_at >= filter.StartDate).ToList(); }
                    if (filter.EndDate != null) { query = query.Where(e => e.create_at <= filter.EndDate).ToList(); }

                    if (group_user.m_group_user_id > 0) { query = query.Where(e => e.m_group_user_id == group_user.m_group_user_id).ToList(); }
                    if (group_user.m_group_user_name != null) { query = query.Where(e => e.m_group_user_name == group_user.m_group_user_name).ToList(); }
                    if (group_user.status == true) query = query.Where(e => e.status == true).ToList();
                    if (group_user.status == false) query = query.Where(e => e.status == false).ToList();
                    if (group_user.create_by != null) { query = query.Where(e => e.create_by == group_user.create_by).ToList(); }

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
            return new JsonResult(PaginationHelper.CreatePagedReponse<m_group_user>(query, validFilter, totalRecords, uriService, route));
        }


        /// <summary>
        /// add data master group_user
        /// </summary>
        /// <param name="group_user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost(nameof(add_m_group_user))]
        [SwaggerOperation(Tags = new[] { "Akses-Group-User" })]
        public async Task<IActionResult> add_m_group_user([FromForm] m_group_user group_user)
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

                    var valided= _context.m_group_user.AsNoTracking().FirstOrDefaultAsync(e => e.m_group_user_name == group_user.m_group_user_name);

                    if (valided.Result != null)
                    {
                        result.message = "Nama/Kode group_user Application Sudah Terdaftar";
                        return new JsonResult(result);
                    }

                    // data input master satuan
                    m_group_user data = new m_group_user
                    {
                        m_group_user_name = group_user.m_group_user_name,
                        status = true,
                        create_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        create_by = ""
                    };
                    _context.m_group_user.Add(data);

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    result.data = data.m_group_user_id;
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
        /// update master group_user
        /// </summary>
        /// <param name="group_user"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPut(nameof(update_group_user))]
        [SwaggerOperation(Tags = new[] { "Akses-Group-User" })]
        public async Task<IActionResult> update_group_user([FromForm] m_group_user group_user)
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

                    var query = _context.m_group_user.AsNoTracking().SingleOrDefault(e => e.m_group_user_id == group_user.m_group_user_id);
                    if (query != null)
                    {

                        // data input m_organiasi
                        query.m_group_user_name = group_user.m_group_user_name;
                        query.status = group_user.status;
                        query.update_by = "";
                        query.update_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        _context.m_group_user.Update(query);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        result.message = "Berhasil Menggubah Data";
                        result.status = true;
                        result.data = group_user.m_group_user_id;
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
        /// delete data master group_user
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpDelete(nameof(delete_m_group_user))]
        [SwaggerOperation(Tags = new[] { "Akses-Group-User" })]
        public async Task<IActionResult> delete_m_group_user(int id)
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
                    var query = _context.m_group_user.AsNoTracking().SingleOrDefault(e => e.m_group_user_id == id);
                    if (query != null)
                    {
                        // data input madrasah
                        _context.m_group_user.Remove(query);
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

        public  class m_group_user_body
        {

            public virtual int m_group_user_id
            {
                get;
                set;
            }

            public virtual string m_group_user_name
            {
                get;
                set;
            }

        }
    }
}
