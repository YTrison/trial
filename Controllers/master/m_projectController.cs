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
    [Route("master/[controller]", Name = "Master-Project-Application")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public class m_project_applicationController : ControllerBase
    {
        private readonly ILogger<m_project_applicationController> _logger;
        private string INVALID = "INVALID";
        private string VALID = "VALID";
        private model_user_managemen _context;
        private readonly IUriService uriService;

        public m_project_applicationController(ILogger<m_project_applicationController> logger, IConfiguration configuration, IUriService uriService)
        {
            //jwt_token._configuration = configuration;
            _logger = logger;
            _context = new model_user_managemen();
            this.uriService = uriService;
        }

        /// <summary>
        /// List data project dengan status aktif
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet(nameof(list_m_project))]
        [SwaggerOperation(Tags = new[] { "Master-Project-Application" })]
        public async Task<IActionResult> list_m_project([FromQuery] m_project_application_body project)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    var query = await _context.m_project_application.OrderBy(e => e.create_at).OrderByDescending(e => e.create_at).ToListAsync();
                    if (project.m_project_application_id != 0)
                    {
                        query = query.Where(e => e.m_project_application_id == project.m_project_application_id).ToList();
                    }
                    if (project.m_project_application_name != null)
                    {
                        query = query.Where(e => e.m_project_application_name == project.m_project_application_name).ToList();
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
        /// List data project dengan status aktif
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet(nameof(list_m_project_pagination))]
        [SwaggerOperation(Tags = new[] { "Master-Project-Application" })]
        public async Task<IActionResult> list_m_project_pagination([FromQuery] m_project_application project, [FromQuery] PaginationFilter filter)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            List<m_project_application> query = new List<m_project_application>();
            var route = Request.Path.Value;
            int totalRecords = 0;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    query = _context.m_project_application.ToList();

                    if (filter.Order == "desc")
                    {
                        if (filter.OrderBy == "m_project_application_id") { query = query.OrderByDescending(e => e.m_project_application_id).ToList(); }
                        else if (filter.OrderBy == "m_project_application_name") { query = query.OrderByDescending(e => e.m_project_application_name).ToList(); }
                        else if (filter.OrderBy == "status_aktif") { query = query.OrderByDescending(e => e.status_aktif).ToList(); }
                        else if (filter.OrderBy == "key_project") { query = query.OrderByDescending(e => e.key_project).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderByDescending(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "type_project") { query = query.OrderByDescending(e => e.type_project).ToList(); }
                        else if (filter.OrderBy == "scope_fitur_project") { query = query.OrderByDescending(e => e.scope_fitur_project).ToList(); }
                        else { query = query.OrderByDescending(e => e.create_at).ToList(); }
                    }
                    else
                    {
                        if (filter.OrderBy == "m_project_application_id") { query = query.OrderBy(e => e.m_project_application_id).ToList(); }
                        else if (filter.OrderBy == "m_project_application_name") { query = query.OrderBy(e => e.m_project_application_name).ToList(); }
                        else if (filter.OrderBy == "status_aktif") { query = query.OrderBy(e => e.status_aktif).ToList(); }
                        else if (filter.OrderBy == "key_project") { query = query.OrderBy(e => e.key_project).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderBy(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "type_project") { query = query.OrderBy(e => e.type_project).ToList(); }
                        else if (filter.OrderBy == "status") { query = query.OrderBy(e => e.scope_fitur_project).ToList(); }
                        else { query = query.OrderBy(e => e.create_at).ToList(); }
                    }

                    if (filter.StartDate != null) { query = query.Where(e => e.create_at >= filter.StartDate).ToList(); }
                    if (filter.EndDate != null) { query = query.Where(e => e.create_at <= filter.EndDate).ToList(); }

                    if (project.m_project_application_id > 0) { query = query.Where(e => e.m_project_application_id == project.m_project_application_id).ToList(); }
                    if (project.m_project_application_name != null) { query = query.Where(e => e.m_project_application_name == project.m_project_application_name).ToList(); }
                    if (project.status_aktif == true) query = query.Where(e => e.status_aktif == true).ToList();
                    if (project.status_aktif == false) query = query.Where(e => e.status_aktif == false).ToList();
                    if (project.key_project != null) { query = query.Where(e => e.key_project== project.key_project).ToList(); }
                    if (project.create_by != null) { query = query.Where(e => e.create_by == project.create_by).ToList(); }
                    if (project.type_project != null) { query = query.Where(e => e.type_project == project.type_project).ToList(); }
                    if (project.scope_fitur_project != null) { query = query.Where(e => e.scope_fitur_project == project.scope_fitur_project).ToList(); }

                    totalRecords = await _context.m_user.CountAsync();
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
            return new JsonResult(PaginationHelper.CreatePagedReponse<m_project_application>(query, validFilter, totalRecords, uriService, route));
        }


        /// <summary>
        /// add data master project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost(nameof(add_m_project))]
        [SwaggerOperation(Tags = new[] { "Master-Project-Application" })]
        public async Task<IActionResult> add_m_project([FromForm] m_project_application project)
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

                    var valided= _context.m_project_application.AsNoTracking().FirstOrDefaultAsync(e => e.m_project_application_name == project.m_project_application_name);

                    if (valided.Result != null)
                    {
                        result.message = "Nama/Kode Project Application Sudah Terdaftar";
                        return new JsonResult(result);
                    }

                    // data input master satuan
                    m_project_application data = new m_project_application
                    {
                        m_project_application_name = project.m_project_application_name,
                        key_project = project.key_project,
                        type_project = project.type_project,
                        scope_fitur_project = project.scope_fitur_project,
                        status_aktif = true,
                        create_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        create_by = ""
                    };
                    _context.m_project_application.Add(data);

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    result.data = data.m_project_application_id;
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
        /// update master project
        /// </summary>
        /// <param name="project"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut(nameof(update_project))]
        [SwaggerOperation(Tags = new[] { "Master-Project-Application" })]
        public async Task<IActionResult> update_project([FromForm] m_project_application project)
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

                    var query = _context.m_project_application.AsNoTracking().SingleOrDefault(e => e.m_project_application_id == project.m_project_application_id);
                    if (query != null)
                    {

                        // data input m_organiasi
                        query.m_project_application_name = project.m_project_application_name;
                        query.key_project = project.key_project;
                        query.status_aktif = project.status_aktif;
                        query.scope_fitur_project = project.scope_fitur_project;
                        query.type_project = project.type_project;
                        query.update_by = "";
                        query.update_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        _context.m_project_application.Update(query);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        result.message = "Berhasil Menggubah Data";
                        result.status = true;
                        result.data = project.m_project_application_id;
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
        /// delete data master project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete(nameof(delete_m_project))]
        [SwaggerOperation(Tags = new[] { "Master-Project-Application" })]
        public async Task<IActionResult> delete_m_project(int id)
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
                    var query = _context.m_project_application.AsNoTracking().SingleOrDefault(e => e.m_project_application_id == id);
                    if (query != null)
                    {
                        // data input madrasah
                        _context.m_project_application.Remove(query);
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

        public  class m_project_application_body
        {

            public virtual int m_project_application_id
            {
                get;
                set;
            }

            public virtual string m_project_application_name
            {
                get;
                set;
            }

        }
    }
}
