using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Linq;
using Newtonsoft.Json;
using Digilens2DLib;
using Microsoft.AspNetCore.Authorization;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using System.Drawing;
using jasuindo_models;
using System.Web;
using System.IO;
using System.Text.RegularExpressions;

using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Text;
using appglobal;
using Swashbuckle.AspNetCore.Annotations;
using web_api_managemen_user.Class;
using web_api_managemen_user.Class.Service;
using web_api_managemen_user;
using System.Collections.Generic;
using web_api_managemen_user.Class.Helper;

namespace web_api_management_user.Controllers
{
    //[Authorize]

    [ApiController]
    [Route("master/[controller]", Name = "Master-Organisasi")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]


    public class m_organiasiController : ControllerBase
    {
        private readonly ILogger<m_organiasiController> _logger;
        private string INVALID = "INVALID";
        private string VALID = "VALID";
        private model_user_managemen _context;
        private readonly IUriService uriService;

        public m_organiasiController(ILogger<m_organiasiController> logger, IConfiguration configuration, IUriService uriService)
        {
            //jwt_token._configuration = configuration;
            _logger = logger;
            _context = new model_user_managemen();
            this.uriService = uriService;
        }

        /// <summary>
        /// List data master organiasi dengan status aktif
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet(nameof(list_m_organiasi))]
        [SwaggerOperation(Tags = new[] { "Master-Organisasi" })]
        public async Task<IActionResult> list_m_organiasi([FromQuery] m_organisasi_body organisasi)
        {
          

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    //var query = await _context.m_organisasi.OrderBy(e => e.create_at).OrderByDescending(e => e.create_at).ToListAsync();

                    var query =await (from m_organisasi in _context.m_organisasi
                               select new
                               { 
                                  m_organisasi.m_organisasi_id,
                                  m_organisasi.nama_oganisasi,
                                  m_organisasi.alamat_organisasi,
                                  m_organisasi.email_organisasi,
                                  m_organisasi.create_at,
                                  m_organisasi.create_by,
                                  m_organisasi.update_at,
                                  m_organisasi.update_by,
                                  m_organisasi.telepon_organisasi,
                                  m_organisasi.status_aktif,
                                  m_organisasi.path_logo_organiasi,
                                  m_organisasi.npwp_organiasi,
                                  m_organisasi.website_organisasi,
                                  logo_organisasi = AppGlobal.get_file_to_byte("Assetts/upload/organisasi/" + m_organisasi.path_logo_organiasi + @".jpeg")
                               }).ToListAsync();

                    if (organisasi.m_organisasi_id != 0)
                    {
                        query =  query.Where(e => e.m_organisasi_id == organisasi.m_organisasi_id).ToList();
                    }
                    if(organisasi.nama_oganisasi != null)
                    {
                        query = query.Where(e => e.nama_oganisasi == organisasi.nama_oganisasi).ToList();
                    }
                    if(organisasi.alamat_organisasi != null)
                    {
                        query = query.Where(e => e.alamat_organisasi == organisasi.alamat_organisasi).ToList();
                    }
                    result.data = query;
                    result.message = "valid";
                    result.status = true;
                }
                catch (Exception ex)
                {
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
        /// List data master organiasi dengan status aktif
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet(nameof(list_m_organiasi_pagination))]
        [SwaggerOperation(Tags = new[] { "Master-Organisasi" })]
        public async Task<IActionResult> list_m_organiasi_pagination([FromQuery] m_organisasi_form organisasi, [FromQuery] PaginationFilter filter)
        {


            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            List<m_organisasi> query = new List<m_organisasi>();
            var route = Request.Path.Value;
            int totalRecords = 0;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    query = _context.m_organisasi.ToList();

                    if (filter.Order == "desc")
                    {
                        if (filter.OrderBy == "m_organisasi_id") { query = query.OrderByDescending(e => e.m_organisasi_id).ToList(); }
                        else if (filter.OrderBy == "nama_oganisasi") { query = query.OrderByDescending(e => e.nama_oganisasi).ToList(); }
                        else if (filter.OrderBy == "alamat_organisasi") { query = query.OrderByDescending(e => e.alamat_organisasi).ToList(); }
                        else if (filter.OrderBy == "email_organisasi") { query = query.OrderByDescending(e => e.email_organisasi).ToList(); }
                        else if (filter.OrderBy == "website_organisasi") { query = query.OrderByDescending(e => e.website_organisasi).ToList(); }
                        else if (filter.OrderBy == "telepon_organisasi") { query = query.OrderByDescending(e => e.telepon_organisasi).ToList(); }
                        else if (filter.OrderBy == "path_logo_organiasi") { query = query.OrderByDescending(e => e.path_logo_organiasi).ToList(); }
                        else if (filter.OrderBy == "npwp_organiasi") { query = query.OrderByDescending(e => e.npwp_organiasi).ToList(); }
                        else if (filter.OrderBy == "status_aktif") { query = query.OrderByDescending(e => e.status_aktif).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderByDescending(e => e.create_by).ToList(); }
                        else { query = query.OrderByDescending(e => e.create_at).ToList(); }
                    }
                    else
                    {
                        if (filter.OrderBy == "m_organisasi_id") { query = query.OrderBy(e => e.m_organisasi_id).ToList(); }
                        else if (filter.OrderBy == "nama_oganisasi") { query = query.OrderBy(e => e.nama_oganisasi).ToList(); }
                        else if (filter.OrderBy == "alamat_organisasi") { query = query.OrderBy(e => e.alamat_organisasi).ToList(); }
                        else if (filter.OrderBy == "email_organisasi") { query = query.OrderBy(e => e.email_organisasi).ToList(); }
                        else if (filter.OrderBy == "website_organisasi") { query = query.OrderBy(e => e.website_organisasi).ToList(); }
                        else if (filter.OrderBy == "telepon_organisasi") { query = query.OrderBy(e => e.telepon_organisasi).ToList(); }
                        else if (filter.OrderBy == "path_logo_organiasi") { query = query.OrderBy(e => e.path_logo_organiasi).ToList(); }
                        else if (filter.OrderBy == "npwp_organiasi") { query = query.OrderBy(e => e.npwp_organiasi).ToList(); }
                        else if (filter.OrderBy == "status_aktif") { query = query.OrderBy(e => e.status_aktif).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderBy(e => e.create_by).ToList(); }
                        else { query = query.OrderBy(e => e.create_at).ToList(); }
                    }

                    if (filter.StartDate != null) { query = query.Where(e => e.create_at >= filter.StartDate).ToList(); }
                    if (filter.EndDate != null) { query = query.Where(e => e.create_at <= filter.EndDate).ToList(); }

                    if (organisasi.m_organisasi_id > 0) { query = query.Where(e => e.m_organisasi_id == organisasi.m_organisasi_id).ToList(); }
                    if (organisasi.nama_oganisasi != null) { query = query.Where(e => e.nama_oganisasi == organisasi.nama_oganisasi).ToList(); }
                    if (organisasi.alamat_organisasi != null) { query = query.Where(e => e.alamat_organisasi == organisasi.alamat_organisasi).ToList(); }
                    if (organisasi.email_organisasi != null) { query = query.Where(e => e.email_organisasi == organisasi.email_organisasi).ToList(); }
                    if (organisasi.website_organisasi != null) { query = query.Where(e => e.website_organisasi == organisasi.website_organisasi).ToList(); }
                    if (organisasi.telepon_organisasi != null) { query = query.Where(e => e.telepon_organisasi == organisasi.telepon_organisasi).ToList(); }
                    if (organisasi.path_logo_organiasi != null) { query = query.Where(e => e.path_logo_organiasi == organisasi.path_logo_organiasi).ToList(); }
                    if (organisasi.npwp_organiasi != null) { query = query.Where(e => e.npwp_organiasi == organisasi.npwp_organiasi).ToList(); }
                    if (organisasi.status_aktif == true) query = query.Where(e => e.status_aktif == true).ToList();
                    if (organisasi.status_aktif == false) query = query.Where(e => e.status_aktif == false).ToList();
                    if (organisasi.create_by != null) { query = query.Where(e => e.create_by == organisasi.create_by).ToList(); }
                    
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
                    result.message = MessageValue.MessageValueDelete(ex.Message);
                    result.status = false;
                    return BadRequest(result);
                }
            }
            return new JsonResult(PaginationHelper.CreatePagedReponse<m_organisasi>(query, validFilter, totalRecords, uriService, route));
        }



        /// <summary>
        /// add data master organisasi
        /// </summary>
        /// <param name="organisasi"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost(nameof(add_m_organisasi))]
        [SwaggerOperation(Tags = new[] { "Master-Organisasi" })]
        public async Task<IActionResult> add_m_organisasi([FromForm] m_organisasi_form organisasi)
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
                    string guid_path = Guid.NewGuid().ToString();
                    string SavePath = @"Assetts/upload/organisasi/" + guid_path + @".jpeg";
                    var valided_organisasi = _context.m_organisasi.AsNoTracking().FirstOrDefaultAsync(e => e.nama_oganisasi == organisasi.nama_oganisasi);

                    if (valided_organisasi.Result != null)
                    {
                        result.message = "Nama/Kode Organisasi Sudah Terdaftar";
                        return new JsonResult(result);
                    }

                    // data input master satuan
                    m_organisasi data = new m_organisasi
                    {
                        nama_oganisasi = organisasi.nama_oganisasi,
                        alamat_organisasi = organisasi.alamat_organisasi,
                        email_organisasi = organisasi.email_organisasi,
                        telepon_organisasi = organisasi.telepon_organisasi,
                        npwp_organiasi = organisasi.npwp_organiasi,
                        path_logo_organiasi = guid_path,
                        website_organisasi = organisasi.website_organisasi,
                        status_aktif = true,
                        create_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        create_by = ""
                    };
                    _context.m_organisasi.Add(data);

                    if (organisasi.file_logo_organiasi != null)
                    {
                        using (var stream = new FileStream(SavePath, FileMode.Create))
                        {
                            organisasi.file_logo_organiasi.CopyTo(stream);
                        }
                    }

                    await _context.SaveChangesAsync();
                    transaction.Commit();
                    result.data = data.m_organisasi_id;
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
        /// update master organiasi
        /// </summary>
        /// <param name="organisasi"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut(nameof(update_m_organisasi))]
        [SwaggerOperation(Tags = new[] { "Master-Organisasi" })]
        public async Task<IActionResult> update_m_organisasi([FromForm] m_organisasi_form organisasi)
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
                    
                    var query = _context.m_organisasi.AsNoTracking().SingleOrDefault(e => e.m_organisasi_id == organisasi.m_organisasi_id);
                    string guid_path = Guid.NewGuid().ToString();
                    if (query != null)
                    {
                        if(query.path_logo_organiasi == "" || query.path_logo_organiasi == null)
                        {
                            query.path_logo_organiasi = guid_path;
                        }
                        string SavePath = @"Assetts/upload/organisasi/" + query.path_logo_organiasi + @".jpeg";
                        // data input m_organiasi
                        query.nama_oganisasi = organisasi.nama_oganisasi;
                        query.email_organisasi = organisasi.email_organisasi;
                        query.alamat_organisasi = organisasi.alamat_organisasi;
                        query.telepon_organisasi = organisasi.telepon_organisasi;
                        query.website_organisasi = organisasi.website_organisasi;
                        query.npwp_organiasi = organisasi.npwp_organiasi;
                        query.status_aktif = organisasi.status_aktif;
                        query.update_by = "";
                        query.update_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));

                        if (organisasi.file_logo_organiasi != null)
                        {
                            using (var stream = new FileStream(SavePath, FileMode.Create))
                            {
                                organisasi.file_logo_organiasi.CopyTo(stream);
                            }
                        }

                        _context.m_organisasi.Update(query);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        result.message = "Berhasil Menggubah Data";
                        result.status = true;
                        result.data = organisasi.m_organisasi_id;

                        
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
        /// delete data master organiasi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete(nameof(delete_m_organiasi))]
        [SwaggerOperation(Tags = new[] { "Master-Organisasi" })]
        public async Task<IActionResult> delete_m_organiasi(int id)
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
                    var query = _context.m_organisasi.AsNoTracking().SingleOrDefault(e => e.m_organisasi_id == id);
                    if (query != null)
                    {
                        string SavePath = @"Assetts/upload/organisasi/" + query.path_logo_organiasi + @".jpeg";
                        // data input madrasah
                        _context.m_organisasi.Remove(query);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        result.message = "Berhasil Menghapus Data";
                        result.status = true;
                        FileInfo file = new FileInfo(SavePath);
                      
                        if (file.Exists)//check file exsit or not  
                        {
                            file.Delete();
                            
                        }
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

        

        public partial class m_organisasi_form
        {

            public virtual int m_organisasi_id
            {
                get;
                set;
            }

            public virtual string nama_oganisasi
            {
                get;
                set;
            }

            public virtual string alamat_organisasi
            {
                get;
                set;
            }

            public virtual bool status_aktif
            {
                get;
                set;
            }

            public virtual string email_organisasi
            {
                get;
                set;
            }

            public virtual string website_organisasi
            {
                get;
                set;
            }

            public virtual string telepon_organisasi
            {
                get;
                set;
            }

            public virtual string path_logo_organiasi
            {
                get;
                set;
            }

            public virtual IFormFile file_logo_organiasi
            {
                get;
                set;
            }

            public virtual string npwp_organiasi
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

        }

        public class m_organisasi_body
        {
            public virtual int m_organisasi_id
            {
                get;
                set;
            }

            public virtual string nama_oganisasi
            {
                get;
                set;
            }

            public virtual string alamat_organisasi
            {
                get;
                set;
            }
        }

    }
    

}
