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
    [Route("master/[controller]", Name = "Master-Group-Feature")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public class m_group_featureController : ControllerBase
    {
        private readonly ILogger<m_group_featureController> _logger;
        private string INVALID = "INVALID";
        private string VALID = "VALID";
        private model_user_managemen _context;
        private readonly IUriService uriService;

        public m_group_featureController(ILogger<m_group_featureController> logger, IConfiguration configuration, IUriService uriService)
        {
            //jwt_token._configuration = configuration;
            _logger = logger;
            _context = new model_user_managemen();
            this.uriService = uriService;
        }

        /// <summary>
        /// List data group_feature dengan status aktif
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet(nameof(list_m_group_feature))]
        [SwaggerOperation(Tags = new[] { "Master-Group-Feature" })]
        public async Task<IActionResult> list_m_group_feature([FromQuery] m_group_feature_application_body group_feature)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var query = await _context.m_group_feature.OrderBy(e => e.create_at).OrderByDescending(e => e.create_at).ToListAsync();
                    if (group_feature.m_group_feature_application_id != 0)
                    {
                        query =  query.Where(e => e.m_group_feature_id == group_feature.m_group_feature_application_id).ToList();
                    }
                    if (group_feature.m_group_feature_application_name != null)
                    {
                        query =  query.Where(e => e.group_feature_name == group_feature.m_group_feature_application_name).ToList();
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
        /// List data group_feature dengan status aktif
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet(nameof(list_m_group_feature_pagination))]
        [SwaggerOperation(Tags = new[] { "Master-Group-Feature" })]
        public async Task<IActionResult> list_m_group_feature_pagination([FromQuery] m_group_feature group_feature, [FromQuery] PaginationFilter filter)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            List<m_group_feature> query = new List<m_group_feature>();
            var route = Request.Path.Value;
            int totalRecords = 0;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    query = _context.m_group_feature.ToList();

                    if (filter.Order == "desc")
                    {
                        if (filter.OrderBy == "m_group_feature_id") { query = query.OrderByDescending(e => e.m_group_feature_id).ToList(); }
                        else if (filter.OrderBy == "index") { query = query.OrderByDescending(e => e.index).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderByDescending(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "group_feature_name") { query = query.OrderByDescending(e => e.group_feature_name).ToList(); }
                        else if (filter.OrderBy == "status") { query = query.OrderByDescending(e => e.status).ToList(); }
                        else { query = query.OrderByDescending(e => e.create_at).ToList(); }
                    }
                    else
                    {
                        if (filter.OrderBy == "m_group_feature_id") { query = query.OrderBy(e => e.m_group_feature_id).ToList(); }
                        else if (filter.OrderBy == "index") { query = query.OrderBy(e => e.index).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderBy(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "group_feature_name") { query = query.OrderBy(e => e.group_feature_name).ToList(); }
                        else if (filter.OrderBy == "status") { query = query.OrderBy(e => e.status).ToList(); }
                        else { query = query.OrderBy(e => e.create_at).ToList(); }
                    }

                    if (filter.StartDate != null) { query = query.Where(e => e.create_at >= filter.StartDate).ToList(); }
                    if (filter.EndDate != null) { query = query.Where(e => e.create_at <= filter.EndDate).ToList(); }

                    if (group_feature.m_group_feature_id > 0) { query = query.Where(e => e.m_group_feature_id == group_feature.m_group_feature_id).ToList(); }
                    if (group_feature.index > 0) { query = query.Where(e => e.index == group_feature.index).ToList(); }
                    if (group_feature.create_by != null) { query = query.Where(e => e.create_by == group_feature.create_by).ToList(); }
                    if (group_feature.group_feature_name != null) { query = query.Where(e => e.group_feature_name == group_feature.group_feature_name).ToList(); }
                    if (group_feature.status == true) query = query.Where(e => e.status == true).ToList();
                    if (group_feature.status == false) query = query.Where(e => e.status == false).ToList();

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

            return new JsonResult(PaginationHelper.CreatePagedReponse<m_group_feature>(query, validFilter, totalRecords, uriService, route));
        }


        /// <summary>
        /// add data master group_feature
        /// </summary>
        /// <param name="group_feature"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost(nameof(add_m_group_feature))]
        [SwaggerOperation(Tags = new[] { "Master-Group-Feature" })]
        public async Task<IActionResult> add_m_group_feature([FromForm] m_group_feature group_feature)
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

                    var valided= _context.m_group_feature.AsNoTracking().FirstOrDefaultAsync(e => e.group_feature_name == group_feature.group_feature_name);

                    if (valided.Result != null)
                    {
                        result.message = "Nama/Kode group_feature Application Sudah Terdaftar";
                        return new JsonResult(result);
                    }

                    // data input master satuan
                    m_group_feature data = new m_group_feature
                    {
                        group_feature_name = group_feature.group_feature_name,
                        index = group_feature.index,
                        status = true,
                        create_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        create_by = ""
                    };
                    _context.m_group_feature.Add(data);

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    result.data = data.m_group_feature_id;
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
        /// update master group_feature
        /// </summary>
        /// <param name="group_feature"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut(nameof(update_group_feature))]
        [SwaggerOperation(Tags = new[] { "Master-Group-Feature" })]
        public async Task<IActionResult> update_group_feature([FromForm] m_group_feature group_feature)
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

                    var query = _context.m_group_feature.AsNoTracking().SingleOrDefault(e => e.m_group_feature_id == group_feature.m_group_feature_id);
                    if (query != null)
                    {

                        // data input m_organiasi
                        query.group_feature_name = group_feature.group_feature_name;
                        query.status = group_feature.status;
                        query.index = group_feature.index;
                        query.update_by = "";
                        query.update_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        _context.m_group_feature.Update(query);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        result.message = "Berhasil Menggubah Data";
                        result.status = true;
                        result.data = group_feature.m_group_feature_id;
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
                    result.message = MessageValue.MessageValueDelete(ex.Message);
                    result.status = false;
                    return BadRequest(result);
                }
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// delete data master group_feature
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete(nameof(delete_m_group_feature))]
        [SwaggerOperation(Tags = new[] { "Master-Group-Feature" })]
        public async Task<IActionResult> delete_m_group_feature(int id)
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
                    var query = _context.m_group_feature.AsNoTracking().SingleOrDefault(e => e.m_group_feature_id == id);
                    if (query != null)
                    {
                        // data input madrasah
                        _context.m_group_feature.Remove(query);
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

        public  class m_group_feature_application_body
        {

            public virtual int m_group_feature_application_id
            {
                get;
                set;
            }

            public virtual string m_group_feature_application_name
            {
                get;
                set;
            }

        }
    }
}
