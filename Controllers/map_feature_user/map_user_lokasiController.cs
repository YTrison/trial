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
    [Route("map_lokasi_user/[controller]", Name = "Map-User-Lokasi")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public class map_user_lokasiController : ControllerBase
    {
        private readonly ILogger<map_user_lokasiController> _logger;
        private string INVALID = "INVALID";
        private string VALID = "VALID";
        private model_user_managemen _context;
        private readonly IUriService uriService;

        public map_user_lokasiController(ILogger<map_user_lokasiController> logger, IConfiguration configuration, IUriService uriService)
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
        [HttpGet(nameof(list_map_user_lokasi))]
        [SwaggerOperation(Tags = new[] { "Map-User-Lokasi" })]
        public async Task<IActionResult> list_map_user_lokasi([FromQuery] map_user_lokasi_body map_lokasi)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                  
                    var query = await _context.map_user_lokasi.AsNoTracking().AsQueryable()
                        .Include(e=> e.m_lokasi).ThenInclude(e=>e.m_organisasi)
                        .Include(e => e.m_project_application)
                        .Include(e => e.m_user)
                        .ToListAsync();

                  
                    if (map_lokasi.map_user_lokasi_id != Guid.Empty)
                    {
                        query = query.Where(e => e.map_user_lokasi_id == map_lokasi.map_user_lokasi_id).ToList();
                    }
                    if(map_lokasi.m_organisasi_id != 0)
                    {
                        query = query.Where(e => _context.m_lokasi.Where(a=> a.m_organisasi_id == map_lokasi.m_organisasi_id).Any(a=> a.m_lokasi_id == e.m_lokasi_id)).ToList();
                    }
                    if (map_lokasi.m_lokasi_id != 0)
                    {
                        query = query.Where(e => e.m_lokasi_id == map_lokasi.m_lokasi_id).ToList();
                    }
                    if (map_lokasi.m_user_id != Guid.Empty)
                    {
                        query = query.Where(e => e.m_user_id == map_lokasi.m_user_id).ToList();
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
        [HttpGet(nameof(list_map_user_lokasi_pagination))]
        [SwaggerOperation(Tags = new[] { "Map-User-Lokasi" })]
        public async Task<IActionResult> list_map_user_lokasi_pagination([FromQuery] map_user_lokasi_body map_lokasi, [FromQuery] PaginationFilter filter)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            List<map_user_lokasi> query = new List<map_user_lokasi>();
            var route = Request.Path.Value;
            int totalRecords = 0;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    query = _context.map_user_lokasi.ToList();

                    if (filter.Order == "desc")
                    {
                        if (filter.OrderBy == "map_user_lokasi_id") { query = query.OrderByDescending(e => e.map_user_lokasi_id).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderByDescending(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "m_user_id") { query = query.OrderByDescending(e => e.m_user_id).ToList(); }
                        else if (filter.OrderBy == "m_project_application_id") { query = query.OrderByDescending(e => e.m_project_application_id).ToList(); }
                        else if (filter.OrderBy == "m_lokasi_id") { query = query.OrderByDescending(e => e.m_lokasi_id).ToList(); }
                        else { query = query.OrderByDescending(e => e.create_at).ToList(); }
                    }
                    else
                    {
                        if (filter.OrderBy == "map_user_lokasi_id") { query = query.OrderBy(e => e.map_user_lokasi_id).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderBy(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "m_user_id") { query = query.OrderBy(e => e.m_user_id).ToList(); }
                        else if (filter.OrderBy == "m_project_application_id") { query = query.OrderBy(e => e.m_project_application_id).ToList(); }
                        else if (filter.OrderBy == "m_lokasi_id") { query = query.OrderBy(e => e.m_lokasi_id).ToList(); }
                        else { query = query.OrderBy(e => e.create_at).ToList(); }
                    }

                    if (filter.StartDate != null) { query = query.Where(e => e.create_at >= filter.StartDate).ToList(); }
                    if (filter.EndDate != null) { query = query.Where(e => e.create_at <= filter.EndDate).ToList(); }

                    if (map_lokasi.map_user_lokasi_id != Guid.Empty) { query = query.Where(e => e.map_user_lokasi_id == map_lokasi.map_user_lokasi_id).ToList(); }
                    if (map_lokasi.create_by != null) { query = query.Where(e => e.m_user_id == map_lokasi.m_user_id).ToList(); }
                    if (map_lokasi.m_user_id != Guid.Empty) { query = query.Where(e => e.m_user_id == map_lokasi.m_user_id).ToList(); }
                    if (map_lokasi.m_project_application_id > 0) { query = query.Where(e => e.m_project_application_id == map_lokasi.m_project_application_id).ToList(); }
                    if (map_lokasi.m_lokasi_id > 0) { query = query.Where(e => e.m_lokasi_id == map_lokasi.m_lokasi_id).ToList(); }

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
            return new JsonResult(PaginationHelper.CreatePagedReponse<map_user_lokasi>(query, validFilter, totalRecords, uriService, route));
        }

        /// <summary>
        /// add data map feature group project
        /// </summary>
        /// <param name="map_lokasi"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost(nameof(add_map_user_lokasi))]
        [SwaggerOperation(Tags = new[] { "Map-User-Lokasi" })]
        public async Task<IActionResult> add_map_user_lokasi([FromBody] List<map_user_lokasi_body> map_lokasi)
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
                    ////var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                    ////var name =  claims["username"];
                    ////string role = User.Claims.SingleOrDefault(e=> e.Type.Contains("role")).Value;

                    if (map_lokasi != null)
                    {
                        foreach (var item in map_lokasi)
                        {
                            map_user_lokasi item_data = new map_user_lokasi()
                            {
                                map_user_lokasi_id = Guid.NewGuid(),
                                m_lokasi_id = item.m_lokasi_id,
                                m_project_application_id = item.m_project_application_id,
                                m_user_id = item.m_user_id,
                                create_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                create_by = ""
                            };
                            _context.map_user_lokasi.Add(item_data);
                        }
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    result.data = null;
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
        /// add data map feature group project
        /// </summary>
        /// <param name="map_lokasi"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut(nameof(update_map_user_lokasi))]
        [SwaggerOperation(Tags = new[] { "Map-User-Lokasi" })]
        public async Task<IActionResult> update_map_user_lokasi([FromBody] List<map_user_lokasi_body> map_lokasi)
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
                    ////var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                    ////var name =  claims["username"];
                    ////string role = User.Claims.SingleOrDefault(e=> e.Type.Contains("role")).Value;

                    if (map_lokasi != null)
                    {
                        foreach (var item in map_lokasi)
                        {
                            var data = _context.map_user_lokasi.AsNoTracking().FirstOrDefault(e => e.map_user_lokasi_id == item.map_user_lokasi_id);

                            if(data != null)
                            {
                                data.m_lokasi_id = item.m_lokasi_id;
                                data.update_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                data.update_by = "";
                               _context.map_user_lokasi.Update(data);
                            }
                        
                        }
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    result.data = null;
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
        /// delete data map feature group project
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete(nameof(delete_map_user_lokasi))]
        [SwaggerOperation(Tags = new[] { "Map-User-Lokasi" })]
        public async Task<IActionResult> delete_map_user_lokasi([FromBody] List<string> id)
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
                    ////var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                    ////var name =  claims["username"];
                    ////string role = User.Claims.SingleOrDefault(e=> e.Type.Contains("role")).Value;


                    //List<map_lokasi_group_project> data_remove = new List<map_lokasi_group_project>();
                    if (id != null)
                    {
                        foreach (var item in id)
                        {
                            var data_remove = _context.map_user_lokasi.AsNoTracking().SingleOrDefault(e => e.map_user_lokasi_id.ToString() == item);

                            _context.map_user_lokasi.Remove(data_remove);
                        }
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    result.data = null;
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
    }



    public partial class map_user_lokasi_body
    {

        public virtual System.Guid map_user_lokasi_id
        {
            get;
            set;
        }

        public virtual int m_lokasi_id
        {
            get;
            set;
        }

        public virtual System.DateTime? create_at
        {
            get;
            set;
        }

        public virtual string create_by
        {
            get;
            set;
        }

        public virtual System.DateTime? update_at
        {
            get;
            set;
        }

        public virtual string update_by
        {
            get;
            set;
        }

        public virtual System.Guid m_user_id
        {
            get;
            set;
        }

        public virtual int m_project_application_id
        {
            get;
            set;
        }

        public virtual int m_organisasi_id
        {
            get;
            set;
        }
        
    }
}
