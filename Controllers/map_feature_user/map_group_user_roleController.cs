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
    [Route("map_role_user/[controller]", Name = "Map-User-Role")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
    public class map_group_user_roleController : ControllerBase
    {
        private readonly ILogger<map_group_user_roleController> _logger;
        private string INVALID = "INVALID";
        private string VALID = "VALID";
        private model_user_managemen _context;
        private readonly IUriService uriService;

        public map_group_user_roleController(ILogger<map_group_user_roleController> logger, IConfiguration configuration, IUriService uriService)
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
        [AllowAnonymous]
        [HttpGet(nameof(list_map_group_user_role))]
        [SwaggerOperation(Tags = new[] { "Map-User-Role" })]
        public async Task<IActionResult> list_map_group_user_role([FromQuery] map_group_user_role_body map_role)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                                      
                    var query = await _context.map_group_user.AsNoTracking().AsQueryable()
                        .Include(e=> e.m_group_user)
                        .Include(e => e.m_project_application)
                        .Include(e => e.m_user)
                        .ToListAsync();

                  
                    if (map_role.map_group_user_id != Guid.Empty)
                    {
                        query = query.Where(e => e.map_group_user_id == map_role.map_group_user_id).ToList();
                    }
                    if (map_role.m_project_application_id != 0)
                    {
                        query = query.Where(e => e.m_project_application_id == map_role.m_project_application_id).ToList();
                    }
                    if (map_role.m_group_user_id != 0)
                    {
                        query = query.Where(e => e.m_group_user_id == map_role.m_group_user_id).ToList();
                    }
                    if (map_role.m_user_id != Guid.Empty)
                    {
                        query = query.Where(e => e.m_user_id == map_role.m_user_id).ToList();
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
        [AllowAnonymous]
        [HttpGet(nameof(list_map_group_user_role_pagination))]
        [SwaggerOperation(Tags = new[] { "Map-User-Role" })]
        public async Task<IActionResult> list_map_group_user_role_pagination([FromQuery] map_group_user_role_body map_role, [FromQuery] PaginationFilter filter)
        {

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            List<map_group_user> query = new List<map_group_user>();
            var route = Request.Path.Value;
            int totalRecords = 0;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    query = _context.map_group_user.ToList();

                    if (filter.Order == "desc")
                    {
                        if (filter.OrderBy == "map_group_user_id") { query = query.OrderByDescending(e => e.map_group_user_id).ToList(); }
                        else if (filter.OrderBy == "m_group_user_id") { query = query.OrderByDescending(e => e.m_group_user_id).ToList(); }
                        else if (filter.OrderBy == "m_project_application_id") { query = query.OrderByDescending(e => e.m_project_application_id).ToList(); }
                        else if (filter.OrderBy == "m_user_id") { query = query.OrderByDescending(e => e.m_user_id).ToList(); }
                        else if (filter.OrderBy == "status_aktif") { query = query.OrderByDescending(e => e.status_aktif).ToList(); }
                        else { query = query.OrderByDescending(e => e.create_at).ToList(); }
                    }
                    else
                    {
                        if (filter.OrderBy == "map_group_user_id") { query = query.OrderBy(e => e.map_group_user_id).ToList(); }
                        else if (filter.OrderBy == "m_group_user_id") { query = query.OrderBy(e => e.m_group_user_id).ToList(); }
                        else if (filter.OrderBy == "m_project_application_id") { query = query.OrderBy(e => e.m_project_application_id).ToList(); }
                        else if (filter.OrderBy == "m_user_id") { query = query.OrderBy(e => e.m_user_id).ToList(); }
                        else if (filter.OrderBy == "status_aktif") { query = query.OrderBy(e => e.status_aktif).ToList(); }
                        else { query = query.OrderBy(e => e.create_at).ToList(); }
                    }

                    if (filter.StartDate != null) { query = query.Where(e => e.create_at >= filter.StartDate).ToList(); }
                    if (filter.EndDate != null) { query = query.Where(e => e.create_at <= filter.EndDate).ToList(); }

                    if (map_role.map_group_user_id != Guid.Empty) { query = query.Where(e => e.map_group_user_id == map_role.map_group_user_id).ToList(); }
                    if (map_role.m_group_user_id > 0) { query = query.Where(e => e.m_group_user_id == map_role.m_group_user_id).ToList(); }
                    if (map_role.m_project_application_id > 0) { query = query.Where(e => e.m_project_application_id == map_role.m_project_application_id).ToList(); }
                    if (map_role.m_user_id != Guid.Empty) { query = query.Where(e => e.m_user_id == map_role.m_user_id).ToList(); }
                    if (map_role.status_aktif == true) query = query.Where(e => e.status_aktif == true).ToList();
                    if (map_role.status_aktif == false) query = query.Where(e => e.status_aktif == false).ToList();

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
            return new JsonResult(PaginationHelper.CreatePagedReponse<map_group_user>(query, validFilter, totalRecords, uriService, route));
        }



        /// <summary>
        /// add data map feature group project
        /// </summary>
        /// <param name="map_role"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost(nameof(add_map_group_user_role))]
        [SwaggerOperation(Tags = new[] { "Map-User-Role" })]
        public async Task<IActionResult> add_map_group_user_role([FromForm] map_group_user_body map_role)
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
                    var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                    var name = claims["username"];
                    string role = User.Claims.SingleOrDefault(e => e.Type.Contains("role")).Value;


                    map_group_user item_data = new map_group_user()
                    {
                        map_group_user_id = Guid.NewGuid(),
                        m_group_user_id = map_role.m_group_user_id,
                        m_user_id = map_role.m_user_id,
                        status_aktif = map_role.status_aktif,
                        m_project_application_id = map_role.m_project_application_id,
                        create_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        create_by = name
                    };
                    _context.map_group_user.Add(item_data);
                  

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
        /// <param name="map_role"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut(nameof(update_map_group_user_role))]
        [SwaggerOperation(Tags = new[] { "Map-User-Role" })]
        public async Task<IActionResult> update_map_group_user_role([FromForm] map_group_user_body map_role)
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
                    var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                    var name = claims["username"];
                    string role = User.Claims.SingleOrDefault(e => e.Type.Contains("role")).Value;


                    var data = _context.map_group_user.AsNoTracking().FirstOrDefault(e => e.map_group_user_id == map_role.map_group_user_id);

                    if(data != null)
                    {
                        data.m_group_user_id = map_role.m_group_user_id;
                        data.m_project_application_id = map_role.m_project_application_id;
                        data.status_aktif = map_role.status_aktif;
                        data.update_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                        data.update_by = name;
                        _context.map_group_user.Update(data);
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
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete(nameof(delete_map_group_user_role))]
        [SwaggerOperation(Tags = new[] { "Map-User-Role" })]
        public async Task<IActionResult> delete_map_group_user_role(string id)
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
                    var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                    var name = claims["username"];
                    string role = User.Claims.SingleOrDefault(e => e.Type.Contains("role")).Value;


                    //List<map_role_group_project> data_remove = new List<map_role_group_project>();
                    if (id != null)
                    {
                      
                            var data_remove = _context.map_group_user.AsNoTracking().SingleOrDefault(e => e.map_group_user_id.ToString() == id);

                            _context.map_group_user.Remove(data_remove);
                      
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


    public partial class map_group_user_body
    {

      

        public virtual System.Guid map_group_user_id
        {
            get;
            set;
        }

        public virtual bool status_aktif
        {
            get;
            set;
        }

        public virtual int m_group_user_id
        {
            get;
            set;
        }

        public virtual int m_project_application_id
        {
            get;
            set;
        }

        public virtual System.Guid m_user_id
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

        public virtual m_group_user m_group_user
        {
            get;
            set;
        }

        public virtual m_user m_user
        {
            get;
            set;
        }

        public virtual m_project_application m_project_application
        {
            get;
            set;
        }

       
    }
    public partial class map_group_user_role_body
    {

        public virtual System.Guid map_group_user_id
        {
            get;
            set;
        }

        public virtual bool status_aktif
        {
            get;
            set;
        }

        public virtual int m_group_user_id
        {
            get;
            set;
        }

        public virtual int m_project_application_id
        {
            get;
            set;
        }

        public virtual System.Guid m_user_id
        {
            get;
            set;
        }
    }
}
