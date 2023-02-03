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
    [Route("map_feature_user/[controller]", Name = "Map-Feature-Group-Project")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public class map_feature_group_projectController : ControllerBase
    {
        private readonly ILogger<map_feature_group_projectController> _logger;
        private string INVALID = "INVALID";
        private string VALID = "VALID";
        private model_user_managemen _context;
        private readonly IUriService uriService;

        public map_feature_group_projectController(ILogger<map_feature_group_projectController> logger, IConfiguration configuration, IUriService uriService)
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
        [HttpGet(nameof(list_map_feature_group_project))]
        [SwaggerOperation(Tags = new[] { "Map-Feature-Group-Project" })]
        public async Task<IActionResult> list_map_feature_group_project([FromQuery] map_feature_group_project_form map_feature)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {

                    List<map_feature_group_project> query = new List<map_feature_group_project>();
                    var  result_data = await _context.map_feature_group_project.AsNoTracking().AsQueryable()
                        .Include(e=> e.m_feature)
                        .Include(e=> e.m_group_feature)
                        .Include(e=> e.m_project_application).ToListAsync();
                    query = result_data;
                    if (map_feature.map_feature_group_project_id != null)
                    {
                        query = result_data.Where(e => e.map_feature_group_project_id.ToString() == map_feature.map_feature_group_project_id).ToList();
                    }
                    if (map_feature.m_project_application_id != 0)
                    {
                        query = result_data.Where(e => e.m_project_application_id == map_feature.m_project_application_id).ToList();
                    }
                    if (map_feature.m_group_feature_id != 0)
                    {
                        query = query.Where(e => e.m_group_feature_id == map_feature.m_group_feature_id).ToList();
                    }
                    if (map_feature.m_feature_id != 0)
                    {
                        query = query.Where(e => e.m_feature_id == map_feature.m_feature_id).ToList();
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
        [HttpGet(nameof(list_map_feature_group_project_pagination))]
        [SwaggerOperation(Tags = new[] { "Map-Feature-Group-Project" })]
        public async Task<IActionResult> list_map_feature_group_project_pagination([FromQuery] map_feature_group_project_form map_feature, [FromQuery] PaginationFilter filter)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            List<map_feature_group_project> query = new List<map_feature_group_project>();
            var route = Request.Path.Value;
            int totalRecords = 0;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    query = _context.map_feature_group_project.ToList();

                    if (filter.Order == "desc")
                    {
                        if (filter.OrderBy == "map_feature_group_project_id") { query = query.OrderByDescending(e => e.map_feature_group_project_id).ToList(); }
                        else if (filter.OrderBy == "m_group_feature_id") { query = query.OrderByDescending(e => e.m_group_feature_id).ToList(); }
                        else if (filter.OrderBy == "m_feature_id") { query = query.OrderByDescending(e => e.m_feature_id).ToList(); }
                        else if (filter.OrderBy == "feature_icon") { query = query.OrderByDescending(e => e.feature_icon).ToList(); }
                        else if (filter.OrderBy == "status_aktif") { query = query.OrderByDescending(e => e.status_aktif).ToList(); }
                        else { query = query.OrderByDescending(e => e.m_project_application_id).ToList(); }
                    }
                    else
                    {
                        if (filter.OrderBy == "map_feature_group_project_id") { query = query.OrderBy(e => e.map_feature_group_project_id).ToList(); }
                        else if (filter.OrderBy == "m_group_feature_id") { query = query.OrderBy(e => e.m_group_feature_id).ToList(); }
                        else if (filter.OrderBy == "m_feature_id") { query = query.OrderBy(e => e.m_feature_id).ToList(); }
                        else if (filter.OrderBy == "feature_icon") { query = query.OrderBy(e => e.feature_icon).ToList(); }
                        else if (filter.OrderBy == "status_aktif") { query = query.OrderBy(e => e.status_aktif).ToList(); }
                        else { query = query.OrderBy(e => e.m_project_application_id).ToList(); }
                    }

                    if (map_feature.map_feature_group_project_id != null) { query = query.Where(e => e.map_feature_group_project_id.ToString() == map_feature.map_feature_group_project_id).ToList(); }
                    if (map_feature.m_project_application_id > 0) { query = query.Where(e => e.m_project_application_id == map_feature.m_project_application_id).ToList(); }
                    if (map_feature.m_group_feature_id > 0) { query = query.Where(e => e.m_group_feature_id == map_feature.m_group_feature_id).ToList(); }
                    if (map_feature.m_feature_id > 0) { query = query.Where(e => e.m_feature_id == map_feature.m_feature_id).ToList(); }
                    if (map_feature.feature_icon != null) { query = query.Where(e => e.feature_icon == map_feature.feature_icon).ToList(); }
                    if (map_feature.status_aktif == true) query = query.Where(e => e.status_aktif == true).ToList();
                    if (map_feature.status_aktif == false) query = query.Where(e => e.status_aktif == false).ToList();

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

            return new JsonResult(PaginationHelper.CreatePagedReponse<map_feature_group_project>(query, validFilter, totalRecords, uriService, route));
        }

        /// <summary>
        /// add data map feature group project
        /// </summary>
        /// <param name="map_project_feature"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost(nameof(add_map_feature_group_project))]
        [SwaggerOperation(Tags = new[] { "Map-Feature-Group-Project" })]
        public async Task<IActionResult> add_map_feature_group_project([FromBody] List<map_feature_group_project_form> map_project_feature)
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

                    if(map_project_feature != null)
                    {
                        foreach(var item in map_project_feature)
                        {
                            map_feature_group_project item_data = new map_feature_group_project()
                            { 
                                map_feature_group_project_id = Guid.NewGuid(),
                                m_feature_id = item.m_feature_id,
                                m_group_feature_id = item.m_group_feature_id,
                                m_project_application_id = item.m_project_application_id,
                                feature_icon = item.feature_icon,
                                status_aktif = true,
                            };
                            _context.map_feature_group_project.Add(item_data);
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
        [HttpDelete(nameof(delete_map_feature_group_project))]
        [SwaggerOperation(Tags = new[] { "Map-Feature-Group-Project" })]
        public async Task<IActionResult> delete_map_feature_group_project([FromBody] List<map_feature_group_project_delete> data)
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

                    //List<map_feature_group_project> data_remove = new List<map_feature_group_project>();
                    List<map_feature_group_project> list_data = new List<map_feature_group_project>();
                    if (data != null)
                    {
                        var single_data = data.FirstOrDefault();
                        if(single_data.m_project_application_id != 0)
                        {
                            if(single_data.m_feature_id != 0 && single_data.m_group_feature_id != 0)
                            {
                                foreach (var item in data)
                                {
                                    var data_remove = _context.map_feature_group_project.AsNoTracking().SingleOrDefault(e => 
                                    e.m_group_feature_id == item.m_group_feature_id 
                                    && e.m_feature_id == item.m_feature_id
                                    && e.m_project_application_id == item.m_project_application_id
                                    );

                                    _context.map_feature_group_project.Remove(data_remove);
                                }
                            }
                            else
                            {
                                list_data = _context.map_feature_group_project.Where(e => e.m_project_application_id == single_data.m_project_application_id).ToList();
                                if (single_data.m_group_feature_id != 0)
                                {
                                    list_data = list_data.Where(e => e.m_group_feature_id == single_data.m_group_feature_id).ToList();
                                }

                                foreach (var item in list_data)
                                {
                                    _context.map_feature_group_project.Remove(item);
                                }
                            }
                            
                        }
                        else
                        {
                            foreach (var item in data)
                            {
                                var data_remove = _context.map_feature_group_project.AsNoTracking().SingleOrDefault(e => e.map_feature_group_project_id.ToString() == item.map_feature_group_project_id);

                                _context.map_feature_group_project.Remove(data_remove);
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
                    result.message = MessageValue.MessageValueDelete(ex.Message.ToString());
                    result.status = false;
                    return BadRequest(result);
                }
            }


            return new JsonResult(result);
        }

       
    }

    public partial class map_feature_group_project_form
    {

     
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

        public virtual bool status_aktif
        {
            get;
            set;
        }

        public virtual int m_group_feature_id
        {
            get;
            set;
        }

        public virtual int m_feature_id
        {
            get;
            set;
        }

        public virtual string feature_icon
        {
            get;
            set;
        }

    }

    public partial class map_feature_group_project_delete
    {


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

        public virtual int m_group_feature_id
        {
            get;
            set;
        }

        public virtual int m_feature_id
        {
            get;
            set;
        }

    }
}
