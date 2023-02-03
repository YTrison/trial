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
    [Route("master/[controller]", Name = "Master-Feature")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public class m_featureController : ControllerBase
    {
        private readonly ILogger<m_featureController> _logger;
        private string INVALID = "INVALID";
        private string VALID = "VALID";
        private model_user_managemen _context;
        private readonly IUriService uriService;

        public m_featureController(ILogger<m_featureController> logger, IConfiguration configuration, IUriService uriService)
        {
            //jwt_token._configuration = configuration;
            _logger = logger;
            _context = new model_user_managemen();
            this.uriService = uriService;
        }

        /// <summary>
        /// List data feature dengan status aktif
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet(nameof(list_m_feature))]
        [SwaggerOperation(Tags = new[] { "Master-Feature" })]
        public async Task<IActionResult> list_m_feature([FromQuery] m_feature feature)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var query = await _context.m_feature.OrderBy(e => e.create_at).OrderByDescending(e => e.create_at).ToListAsync();
                    if (feature.m_feature_id != 0)
                    {
                        query =  query.Where(e => e.m_feature_id == feature.m_feature_id).ToList();
                    }
                    if (feature.m_feature_name != null)
                    {
                        query = query.Where(e => e.m_feature_name == feature.m_feature_name).ToList();
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
        /// List data feature dengan status aktif
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet(nameof(list_m_feature_pagination))]
        [SwaggerOperation(Tags = new[] { "Master-Feature" })]
        public async Task<IActionResult> list_m_feature_pagination([FromQuery] m_feature feature, [FromQuery] PaginationFilter filter)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            List<m_feature> query = new List<m_feature>();
            var route = Request.Path.Value;
            int totalRecords = 0;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    query = _context.m_feature.ToList();

                    if (filter.Order == "desc")
                    {
                        if (filter.OrderBy == "m_feature_id") { query = query.OrderByDescending(e => e.m_feature_id).ToList(); }
                        else if (filter.OrderBy == "m_feature_name") { query = query.OrderByDescending(e => e.m_feature_name).ToList(); }
                        else if (filter.OrderBy == "index") { query = query.OrderByDescending(e => e.index).ToList(); }
                        else if (filter.OrderBy == "name_link_feature") { query = query.OrderByDescending(e => e.name_link_feature).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderByDescending(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "feature_icon") { query = query.OrderByDescending(e => e.feature_icon).ToList(); }
                        else if (filter.OrderBy == "status") { query = query.OrderByDescending(e => e.status).ToList(); }
                        else { query = query.OrderByDescending(e => e.create_at).ToList(); }
                    }
                    else
                    {
                        if (filter.OrderBy == "m_feature_id") { query = query.OrderBy(e => e.m_feature_id).ToList(); }
                        else if (filter.OrderBy == "m_feature_name") { query = query.OrderBy(e => e.m_feature_name).ToList(); }
                        else if (filter.OrderBy == "index") { query = query.OrderBy(e => e.index).ToList(); }
                        else if (filter.OrderBy == "name_link_feature") { query = query.OrderBy(e => e.name_link_feature).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderBy(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "feature_icon") { query = query.OrderBy(e => e.feature_icon).ToList(); }
                        else if (filter.OrderBy == "status") { query = query.OrderBy(e => e.status).ToList(); }
                        else { query = query.OrderBy(e => e.create_at).ToList(); }
                    }

                    if (filter.StartDate != null) { query = query.Where(e => e.create_at >= filter.StartDate).ToList(); }
                    if (filter.EndDate != null) { query = query.Where(e => e.create_at <= filter.EndDate).ToList(); }

                    if (feature.m_feature_id > 0) { query = query.Where(e => e.m_feature_id == feature.m_feature_id).ToList(); }
                    if (feature.m_feature_name != null) { query = query.Where(e => e.m_feature_name == feature.m_feature_name).ToList(); }
                    if (feature.index > 0) { query = query.Where(e => e.index == feature.index).ToList(); }
                    if (feature.name_link_feature != null) { query = query.Where(e => e.name_link_feature == feature.name_link_feature).ToList(); }
                    if (feature.create_by != null) { query = query.Where(e => e.create_by == feature.create_by).ToList(); }
                    if (feature.feature_icon != null) { query = query.Where(e => e.feature_icon == feature.feature_icon).ToList(); }
                    if (feature.status == true) query = query.Where(e => e.status == true).ToList();
                    if (feature.status == false) query = query.Where(e => e.status == false).ToList();

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

            return new JsonResult(PaginationHelper.CreatePagedReponse<m_feature>(query, validFilter, totalRecords, uriService, route));
        }


        /// <summary>
        /// add data master feature
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost(nameof(add_m_feature))]
        [SwaggerOperation(Tags = new[] { "Master-Feature" })]
        public async Task<IActionResult> add_m_feature([FromForm] m_feature feature)
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

                    var valided= _context.m_feature.AsNoTracking().FirstOrDefaultAsync(e => e.m_feature_name == feature.m_feature_name);

                    if (valided.Result != null)
                    {
                        result.message = "Nama/Kode feature Application Sudah Terdaftar";
                        return new JsonResult(result);
                    }

                    // data input master satuan
                    m_feature data = new m_feature
                    {
                        m_feature_name = feature.m_feature_name,
                        name_link_feature = feature.name_link_feature,
                        feature_icon = feature.feature_icon,
                        index = feature.index,
                        status = true,
                        create_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        create_by = ""
                    };
                    _context.m_feature.Add(data);

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    result.data = data.m_feature_id;
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
        /// update master feature
        /// </summary>
        /// <param name="feature"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut(nameof(update_feature))]
        [SwaggerOperation(Tags = new[] { "Master-Feature" })]
        public async Task<IActionResult> update_feature([FromForm] m_feature feature)
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

                    var query = _context.m_feature.AsNoTracking().SingleOrDefault(e => e.m_feature_id == feature.m_feature_id);
                    if (query != null)
                    {

                        // data input m_organiasi
                        query.m_feature_name = feature.m_feature_name;
                        query.name_link_feature = feature.name_link_feature;
                        query.index = feature.index;
                        query.status = feature.status;
                        query.feature_icon = feature.feature_icon;
                        query.update_by = "";
                        query.update_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        _context.m_feature.Update(query);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        result.message = "Berhasil Menggubah Data";
                        result.status = true;
                        result.data = feature.m_feature_id;
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
        /// delete data master feature
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete(nameof(delete_m_feature))]
        [SwaggerOperation(Tags = new[] { "Master-Feature" })]
        public async Task<IActionResult> delete_m_feature(int id)
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
                    var query = _context.m_feature.AsNoTracking().SingleOrDefault(e => e.m_feature_id == id);
                    if (query != null)
                    {
                        // data input madrasah
                        _context.m_feature.Remove(query);
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
                    result.message = MessageValue.MessageValueDelete(ex.Message);
                    result.status = false;
                    return BadRequest(result);
                }
            }
            return new JsonResult(result);
        }

        public  class m_feature_application_body
        {

            public virtual int m_feature_application_id
            {
                get;
                set;
            }

            public virtual string m_feature_application_name
            {
                get;
                set;
            }

        }
    }
}
