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
    [Route("map_feature_user/[controller]", Name = "Map-Feature-Group-Icon")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public class map_feature_group_iconController : ControllerBase
    {
        private readonly ILogger<map_feature_group_iconController> _logger;
        private string INVALID = "INVALID";
        private string VALID = "VALID";
        private model_user_managemen _context;
        private readonly IUriService uriService;

        public map_feature_group_iconController(ILogger<map_feature_group_iconController> logger, IConfiguration configuration, IUriService uriService)
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
        [HttpGet(nameof(list_map_feature_group_icon))]
        [SwaggerOperation(Tags = new[] { "Map-Feature-Group-Icon" })]
        public async Task<IActionResult> list_map_feature_group_icon([FromQuery] map_feature_group_body map_feature)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    List<map_feature_group> query = new List<map_feature_group>();
                    var  result_data = await _context.map_feature_group.AsNoTracking().AsQueryable()
                        .Include(e=> e.map_feature_group_project).ThenInclude(e=>e.m_feature)
                        .Include(e => e.map_feature_group_project).ThenInclude(e => e.m_project_application)
                        .Include(e => e.map_feature_group_project).ThenInclude(e => e.m_group_feature)
                        .Include(e=> e.m_group_user)
                        .ToListAsync()
                        ;
                    query = result_data;
                    if (map_feature.map_feature_group_id != null)
                    {
                        query = result_data.Where(e => e.map_feature_group_id.ToString() == map_feature.map_feature_group_id).ToList();
                    }
                    if(map_feature.m_project_application_id != 0)
                    {
                        query = query.Where(e => _context.map_feature_group_project.Where(a=> a.m_project_application_id == map_feature.m_project_application_id).Any(a=> a.map_feature_group_project_id == e.map_feature_group_project_id)).ToList();
                    }
                    if (map_feature.map_feature_group_project_id != null)
                    {
                        query = result_data.Where(e => e.map_feature_group_project_id.ToString() == map_feature.map_feature_group_project_id).ToList();
                    }
                    if (map_feature.m_group_user_id != 0)
                    {
                        query = query.Where(e => e.m_group_user_id == map_feature.m_group_user_id).ToList();
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
        [HttpGet(nameof(list_map_feature_group_icon_pagination))]
        [SwaggerOperation(Tags = new[] { "Map-Feature-Group-Icon" })]
        public async Task<IActionResult> list_map_feature_group_icon_pagination([FromQuery] map_feature_group_body map_feature, [FromQuery] PaginationFilter filter)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            List<map_feature_group> query = new List<map_feature_group>();
            var route = Request.Path.Value;
            int totalRecords = 0;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    query = _context.map_feature_group.ToList();

                    if (filter.Order == "desc")
                    {
                        if (filter.OrderBy == "map_feature_group_id") { query = query.OrderByDescending(e => e.map_feature_group_id).ToList(); }
                        else if (filter.OrderBy == "m_group_user_id") { query = query.OrderByDescending(e => e.m_group_user_id).ToList(); }
                        else if (filter.OrderBy == "action_feature") { query = query.OrderByDescending(e => e.action_feature).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderByDescending(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "map_feature_group_project_id") { query = query.OrderByDescending(e => e.map_feature_group_project_id).ToList(); }
                        else { query = query.OrderByDescending(e => e.create_at).ToList(); }
                    }
                    else
                    {
                        if (filter.OrderBy == "map_feature_group_id") { query = query.OrderBy(e => e.map_feature_group_id).ToList(); }
                        else if (filter.OrderBy == "m_group_user_id") { query = query.OrderBy(e => e.m_group_user_id).ToList(); }
                        else if (filter.OrderBy == "action_feature") { query = query.OrderBy(e => e.action_feature).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderBy(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "map_feature_group_project_id") { query = query.OrderBy(e => e.map_feature_group_project_id).ToList(); }
                        else { query = query.OrderBy(e => e.create_at).ToList(); }
                    }

                    if (filter.StartDate != null) { query = query.Where(e => e.create_at >= filter.StartDate).ToList(); }
                    if (filter.EndDate != null) { query = query.Where(e => e.create_at <= filter.EndDate).ToList(); }

                    if (map_feature.map_feature_group_id != null) { query = query.Where(e => e.map_feature_group_id.ToString() == map_feature.map_feature_group_id).ToList(); }
                    if (map_feature.m_group_user_id > 0) { query = query.Where(e => e.m_group_user_id == map_feature.m_group_user_id).ToList(); }
                    if (map_feature.action_feature != null) { query = query.Where(e => e.action_feature == map_feature.action_feature).ToList(); }
                    if (map_feature.create_by != null) { query = query.Where(e => e.create_by == map_feature.create_by).ToList(); }
                    if (map_feature.map_feature_group_project_id != null) { query = query.Where(e => e.map_feature_group_project_id.ToString() == map_feature.map_feature_group_project_id).ToList(); }

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

            return new JsonResult(PaginationHelper.CreatePagedReponse<map_feature_group>(query, validFilter, totalRecords, uriService, route));
        }


        /// <summary>
        /// add data map feature group project
        /// </summary>
        /// <param name="map_project_feature"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost(nameof(add_map_feature_group_icon))]
        [SwaggerOperation(Tags = new[] { "Map-Feature-Group-Icon" })]
        public async Task<IActionResult> add_map_feature_group_icon([FromBody] List<map_feature_group_body> map_project_feature)
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

                    if (map_project_feature != null)
                    {
                        foreach (var item in map_project_feature)
                        {
                            map_feature_group item_data = new map_feature_group()
                            {
                                map_feature_group_id = Guid.NewGuid(),
                                m_group_user_id = item.m_group_user_id,
                                action_feature = item.action_feature,
                                map_feature_group_project_id = new Guid(item.map_feature_group_project_id),
                                create_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                                create_by = ""
                            };
                            _context.map_feature_group.Add(item_data);
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
        /// <param name="map_project_feature"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut(nameof(update_map_feature_group_icon))]
        [SwaggerOperation(Tags = new[] { "Map-Feature-Group-Icon" })]
        public async Task<IActionResult> update_map_feature_group_icon([FromBody] List<map_feature_group_body_delete> map_project_feature)
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

                    if (map_project_feature != null)
                    {
                        foreach (var item in map_project_feature)
                        {
                            var data = _context.map_feature_group.AsNoTracking().FirstOrDefault(e => e.map_feature_group_project_id.ToString() == item.map_feature_group_project_id && e.m_group_user_id == item.m_group_user_id);

                            if(data != null)
                            {
                                data.action_feature = item.action_feature;
                                data.update_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                data.update_by = "";
                               _context.map_feature_group.Update(data);
                            }
                        
                        }
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    result.data = null;
                    result.message = "Berhasil Mengubah Data";
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
        /// <param name="map_feature"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete(nameof(delete_map_feature_group_icon))]
        [SwaggerOperation(Tags = new[] { "Map-Feature-Group-Icon" })]
        public async Task<IActionResult> delete_map_feature_group_icon([FromBody] List<map_feature_group_body_delete> map_feature)
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
                  
                    if (map_feature != null)
                    {
                        var single_data = map_feature.FirstOrDefault();
                        
                        if((single_data.map_feature_group_id == "" || single_data.map_feature_group_id == null)  && single_data.m_project_application_id != 0 && single_data.m_group_user_id != 0 && single_data.map_feature_group_project_id != "" && single_data.map_feature_group_project_id != null)
                        {
                            foreach (var item in map_feature)
                            {
                                var data_remove = _context.map_feature_group.SingleOrDefault(e => e.m_group_user_id == single_data.m_group_user_id && e.map_feature_group_project_id.ToString() == item.map_feature_group_project_id &&
                                        _context.map_feature_group_project.Any(a => a.m_project_application_id == single_data.m_project_application_id && e.map_feature_group_project_id == a.map_feature_group_project_id));

                                _context.map_feature_group.Remove(data_remove);
                            }
                           
                        }
                        else if ((single_data.map_feature_group_id == "" || single_data.map_feature_group_id == null) && single_data.m_project_application_id != 0 && single_data.m_group_user_id != 0)
                        {
                            var map = _context.map_feature_group.Where(e => e.m_group_user_id == single_data.m_group_user_id &&
                                      _context.map_feature_group_project.Any(a => a.m_project_application_id == single_data.m_project_application_id && e.map_feature_group_project_id == a.map_feature_group_project_id));
                            foreach (var item in map)
                            {
                                _context.map_feature_group.Remove(item);
                            }
                        }

                        else if (single_data.map_feature_group_id != "" && single_data.map_feature_group_id != null)
                        {
                            foreach (var item in map_feature)
                            {
                                var data_remove = _context.map_feature_group.AsNoTracking().SingleOrDefault(e => e.map_feature_group_id.ToString() == item.map_feature_group_id);

                                _context.map_feature_group.Remove(data_remove);
                            }
                        }

                    }
                    
                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    result.data = null;
                    result.message = "Berhasil Menghapus Data";
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

    

    public partial class map_feature_group_body
    {

        public virtual string map_feature_group_id
        {
            get;
            set;
        }

        public virtual int m_group_user_id
        {
            get;
            set;
        }

        public virtual string action_feature
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

        public virtual string map_feature_group_project_id
        {
            get;
            set;
        }
        public virtual int m_project_application_id
        {
            get;
            set;
        }
        
    }

    public partial class map_feature_group_body_delete
    {

        public virtual string map_feature_group_id
        {
            get;
            set;
        }

        public virtual int m_group_user_id
        {
            get;
            set;
        }

        public virtual string action_feature
        {
            get;
            set;
        }

        public virtual string map_feature_group_project_id
        {
            get;
            set;
        }
        public virtual int m_project_application_id
        {
            get;
            set;
        }

    }
}
