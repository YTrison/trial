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
    [Route("master/[controller]",Name = "Master-Lokasi")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]


    public class m_lokasiController : ControllerBase
    {
        private readonly ILogger<m_lokasiController> _logger;
        private string INVALID = "INVALID";
        private string VALID = "VALID";
        private model_user_managemen _context;
        private static string DEFAULT_PATH = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static string DOCUMENT_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        private readonly IUriService uriService;

        public m_lokasiController(ILogger<m_lokasiController> logger, IConfiguration configuration, IUriService uriService)
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
        //
        [Authorize]
        [HttpGet(nameof(list_m_lokasi))]
        [SwaggerOperation(Tags = new[] { "Master-Lokasi" })]
        public async Task<IActionResult> list_m_lokasi([FromQuery] m_lokasi_body lokasi)
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
                    
                    var query = await _context.m_lokasi.OrderBy(e=> e.parent_index).AsNoTracking().AsQueryable()
                        .Include(e=>e.m_organisasi)
                        .ToListAsync();
                    
                    if(lokasi.m_lokasi_id !=0)
                    {
                        query = query.Where(e => e.m_lokasi_id == lokasi.m_lokasi_id).ToList();
                    }
                    else if(lokasi.m_organisasi_id != 0)
                    {
                        query = query.Where(e => e.m_organisasi_id == lokasi.m_organisasi_id).ToList();
                    }
                    else if (lokasi.nama_lokasi != null)
                    {
                        query = query.Where(e => e.nama_lokasi == lokasi.nama_lokasi).ToList();
                    }
                    else if (lokasi.parent_lokasi_id != 0)
                    {
                        query = query.Where(e => e.parent_lokasi_id == lokasi.parent_lokasi_id).ToList();
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
        /// List data master organiasi dengan status aktif
        /// </summary>
        /// <returns></returns>
        //
        [Authorize]
        [HttpGet(nameof(list_m_lokasi_pagination))]
        [SwaggerOperation(Tags = new[] { "Master-Lokasi" })]
        public async Task<IActionResult> list_m_lokasi_pagination([FromQuery] m_lokasi_body lokasi, [FromQuery] PaginationFilter filter)
        {
            if (ModelState.IsValid) ;

            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;

            var validFilter = new PaginationFilter(filter.PageNumber, filter.PageSize);
            List<m_lokasi> query = new List<m_lokasi>();
            var route = Request.Path.Value;
            int totalRecords = 0;

            using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    query = _context.m_lokasi.ToList();

                    if (filter.Order == "desc")
                    {
                        if (filter.OrderBy == "m_lokasi_id") { query = query.OrderByDescending(e => e.m_lokasi_id).ToList(); }
                        else if (filter.OrderBy == "m_organisasi_id") { query = query.OrderByDescending(e => e.m_organisasi_id).ToList(); }
                        else if (filter.OrderBy == "parent_lokasi_id") { query = query.OrderByDescending(e => e.parent_lokasi_id).ToList(); }
                        else if (filter.OrderBy == "parent_index") { query = query.OrderByDescending(e => e.parent_index).ToList(); }
                        else if (filter.OrderBy == "nama_lokasi") { query = query.OrderByDescending(e => e.nama_lokasi).ToList(); }
                        else if (filter.OrderBy == "kode_lokasi") { query = query.OrderByDescending(e => e.kode_lokasi).ToList(); }
                        else if (filter.OrderBy == "provinsi") { query = query.OrderByDescending(e => e.provinsi).ToList(); }
                        else if (filter.OrderBy == "kabupaten") { query = query.OrderByDescending(e => e.kabupaten).ToList(); }
                        else if (filter.OrderBy == "kecamatan") { query = query.OrderByDescending(e => e.kecamatan).ToList(); }
                        else if (filter.OrderBy == "kelurahan") { query = query.OrderByDescending(e => e.kelurahan).ToList(); }
                        else if (filter.OrderBy == "alamat") { query = query.OrderByDescending(e => e.alamat).ToList(); }
                        else if (filter.OrderBy == "status_aktif") { query = query.OrderByDescending(e => e.status_aktif).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderByDescending(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "name_location_secondary") { query = query.OrderByDescending(e => e.name_location_secondary).ToList(); }
                        else { query = query.OrderByDescending(e => e.create_at).ToList(); }
                    }
                    else
                    {
                        if (filter.OrderBy == "m_lokasi_id") { query = query.OrderBy(e => e.m_lokasi_id).ToList(); }
                        else if (filter.OrderBy == "m_organisasi_id") { query = query.OrderBy(e => e.m_organisasi_id).ToList(); }
                        else if (filter.OrderBy == "parent_lokasi_id") { query = query.OrderBy(e => e.parent_lokasi_id).ToList(); }
                        else if (filter.OrderBy == "parent_index") { query = query.OrderBy(e => e.parent_index).ToList(); }
                        else if (filter.OrderBy == "nama_lokasi") { query = query.OrderBy(e => e.nama_lokasi).ToList(); }
                        else if (filter.OrderBy == "kode_lokasi") { query = query.OrderBy(e => e.kode_lokasi).ToList(); }
                        else if (filter.OrderBy == "provinsi") { query = query.OrderBy(e => e.provinsi).ToList(); }
                        else if (filter.OrderBy == "kabupaten") { query = query.OrderBy(e => e.kabupaten).ToList(); }
                        else if (filter.OrderBy == "kecamatan") { query = query.OrderBy(e => e.kecamatan).ToList(); }
                        else if (filter.OrderBy == "kelurahan") { query = query.OrderBy(e => e.kelurahan).ToList(); }
                        else if (filter.OrderBy == "alamat") { query = query.OrderBy(e => e.alamat).ToList(); }
                        else if (filter.OrderBy == "status_aktif") { query = query.OrderBy(e => e.status_aktif).ToList(); }
                        else if (filter.OrderBy == "create_by") { query = query.OrderBy(e => e.create_by).ToList(); }
                        else if (filter.OrderBy == "name_location_secondary") { query = query.OrderBy(e => e.name_location_secondary).ToList(); }
                        else { query = query.OrderBy(e => e.create_at).ToList(); }
                    }

                    if (filter.StartDate != null) { query = query.Where(e => e.create_at >= filter.StartDate).ToList(); }
                    if (filter.EndDate != null) { query = query.Where(e => e.create_at <= filter.EndDate).ToList(); }

                    if (lokasi.m_lokasi_id > 0) { query = query.Where(e => e.m_lokasi_id == lokasi.m_lokasi_id).ToList(); }
                    if (lokasi.m_organisasi_id > 0) { query = query.Where(e => e.m_organisasi_id == lokasi.m_organisasi_id).ToList(); }
                    if (lokasi.parent_lokasi_id > 0) { query = query.Where(e => e.parent_lokasi_id == lokasi.parent_lokasi_id).ToList(); }
                    if (lokasi.parent_index > 0) { query = query.Where(e => e.parent_index == lokasi.parent_index).ToList(); }
                    if (lokasi.nama_lokasi != null) { query = query.Where(e => e.nama_lokasi == lokasi.nama_lokasi).ToList(); }
                    if (lokasi.kode_lokasi != null) { query = query.Where(e => e.kode_lokasi == lokasi.kode_lokasi).ToList(); }
                    if (lokasi.provinsi != null) { query = query.Where(e => e.provinsi == lokasi.provinsi).ToList(); }
                    if (lokasi.kabupaten != null) { query = query.Where(e => e.kabupaten == lokasi.kabupaten).ToList(); }
                    if (lokasi.kecamatan != null) { query = query.Where(e => e.kecamatan == lokasi.kecamatan).ToList(); }
                    if (lokasi.kelurahan != null) { query = query.Where(e => e.kelurahan == lokasi.kelurahan).ToList(); }
                    if (lokasi.alamat != null) { query = query.Where(e => e.alamat == lokasi.alamat).ToList(); }
                    if (lokasi.status_aktif == true) query = query.Where(e => e.status_aktif == true).ToList();
                    if (lokasi.status_aktif == false) query = query.Where(e => e.status_aktif == false).ToList();
                    if (lokasi.create_by != null) { query = query.Where(e => e.create_by == lokasi.create_by).ToList(); }
                    if (lokasi.name_location_secondary != null) { query = query.Where(e => e.name_location_secondary == lokasi.name_location_secondary).ToList(); }

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
            return new JsonResult(PaginationHelper.CreatePagedReponse<m_lokasi>(query, validFilter, totalRecords, uriService, route));
        }



        /// <summary>
        /// add data master lokasi
        /// </summary>
        /// <param name="lokasi"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost(nameof(add_m_lokasi))]
        [SwaggerOperation(Tags = new[] { "Master-Lokasi" })]
        public async Task<IActionResult> add_m_lokasi([FromForm] m_lokasi_body lokasi)
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

                    var valided = _context.m_lokasi.AsNoTracking().FirstOrDefaultAsync(e => e.kode_lokasi == lokasi.kode_lokasi);

                    if (valided.Result != null)
                    {
                        result.message = "Nama/Kode Lokasi Sudah Terdaftar";
                        return new JsonResult(result);
                    }
                    int parent_index = 0;
                    string name_secondary = lokasi.kode_lokasi;
                    if (lokasi.parent_lokasi_id != 0)
                    {
                        var parent_lokasi = _context.m_lokasi.AsNoTracking().SingleOrDefaultAsync(e => e.m_organisasi_id == lokasi.m_organisasi_id && e.m_lokasi_id == lokasi.parent_lokasi_id);
                        if(parent_lokasi.Result == null)
                        {
                            result.message = "Lokasi Parent Tidak Ada Di Organisasi !";
                            return new JsonResult(result);
                        }
                        var data_lokasi = _context.m_lokasi.SingleOrDefault(e => e.m_lokasi_id == lokasi.parent_lokasi_id);
                        parent_index = data_lokasi.parent_index + 1;
                        name_secondary = data_lokasi.name_location_secondary + "-" + lokasi.kode_lokasi;
                    }
                    // data input master satuan
                    m_lokasi data = new m_lokasi
                    {
                        nama_lokasi = lokasi.nama_lokasi,
                        parent_lokasi_id = lokasi.parent_lokasi_id,
                        parent_index = parent_index,
                        kode_lokasi = lokasi.kode_lokasi,
                        provinsi = lokasi.provinsi,
                        kabupaten = lokasi.kabupaten,
                        kecamatan = lokasi.kecamatan,
                        kelurahan = lokasi.kelurahan,
                        alamat = lokasi.alamat,
                        m_organisasi_id = lokasi.m_organisasi_id,
                        name_location_secondary = name_secondary,
                        status_aktif =true,
                        create_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")),
                        create_by = ""
                    };
                    _context.m_lokasi.Add(data);
                   
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
          return  new JsonResult(result);
        }

        /// <summary>
        /// update master lokasi
        /// </summary>
        /// <param name="lokasi"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPut(nameof(update_m_lokasi))]
        [SwaggerOperation(Tags = new[] { "Master-Lokasi" })]
        public async Task<IActionResult> update_m_lokasi([FromBody] m_lokasi_body lokasi)
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
                    
                    var query = _context.m_lokasi.AsNoTracking().SingleOrDefault(e => e.m_lokasi_id == lokasi.m_lokasi_id);
                    if(query != null)
                    {

                        int parent_index = 0;
                        string name_secondary = lokasi.kode_lokasi;
                        if (lokasi.parent_lokasi_id != 0)
                        {
                            var parent_lokasi = _context.m_lokasi.AsNoTracking().SingleOrDefaultAsync(e => e.m_organisasi_id == lokasi.m_organisasi_id && e.m_lokasi_id == lokasi.parent_lokasi_id);
                            if (parent_lokasi.Result == null)
                            {
                                result.message = "Lokasi Parent Tidak Ada Di Organisasi !";
                                return new JsonResult(result);
                            }
                            var data_lokasi = _context.m_lokasi.SingleOrDefault(e => e.m_lokasi_id == lokasi.parent_lokasi_id);
                            parent_index = data_lokasi.parent_index + 1;
                            name_secondary = data_lokasi.name_location_secondary + "-" + lokasi.kode_lokasi;
                        }
                        // data input m_lokasi
                        query.nama_lokasi = lokasi.nama_lokasi;
                        query.kabupaten = lokasi.kabupaten;
                        query.provinsi = lokasi.provinsi;
                        query.parent_index = parent_index;
                        query.parent_lokasi_id = lokasi.parent_lokasi_id;
                        query.kecamatan = lokasi.kecamatan;
                        query.kelurahan = lokasi.kelurahan;
                        query.alamat = lokasi.alamat;
                        query.kode_lokasi = lokasi.kode_lokasi;
                        query.name_location_secondary = name_secondary;
                        query.status_aktif = lokasi.status_aktif;
                        query.m_organisasi_id = lokasi.m_organisasi_id;
                        query.update_by = "";
                        query.update_at = Convert.ToDateTime(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                                               
                        _context.m_lokasi.Update(query);
                        await _context.SaveChangesAsync();
                        transaction.Commit();
                        result.message = "Berhasil Menggubah Data";
                        result.status = true;
                        result.data = lokasi.m_lokasi_id;
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
                    if (ex.ToString().Contains("cannot be tracked because another instance with the same key value for"))
                    {
                        result.message = "Gagal Menggubah ! ()";
                    }
                    else
                    {
                        result.message = ex.Message;
                    }
                    Console.WriteLine(ex);
                    transaction.Rollback();
                    Console.WriteLine(ex.Message);
                    result.data = null;
                   
                    result.status = false;
                    return BadRequest(result);
                }
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// delete data master lokasi
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete(nameof(delete_m_lokasi))]
        [SwaggerOperation(Tags = new[] { "Master-Lokasi" })]
        public async Task<IActionResult> delete_m_lokasi(int id)
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
                    var query = _context.m_lokasi.AsNoTracking().SingleOrDefault(e => e.m_lokasi_id == id);
                    if (query != null)
                    {
                        var child_data = _context.m_lokasi.Where(e => e.parent_lokasi_id == id).ToList();
                        if(child_data.Count() > 0)
                        {
                            result.message = "Data lokasi menjadi parent di lokasi lainya !";
                            result.status = false;
                            result.data = child_data;
                            return new JsonResult(result);
                        }
                        // data input madrasah
                        _context.m_lokasi.Remove(query);
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

    }
    public partial class m_lokasi_body
    {

       

        public virtual int m_lokasi_id
        {
            get;
            set;
        }

        public virtual int parent_lokasi_id
        {
            get;
            set;
        }

        public virtual int parent_index
        {
            get;
            set;
        }

        public virtual string nama_lokasi
        {
            get;
            set;
        }

        public virtual string kode_lokasi
        {
            get;
            set;
        }

        public virtual string provinsi
        {
            get;
            set;
        }

        public virtual string kabupaten
        {
            get;
            set;
        }

        public virtual string kecamatan
        {
            get;
            set;
        }

        public virtual string kelurahan
        {
            get;
            set;
        }

        public virtual string alamat
        {
            get;
            set;
        }

        public virtual bool status_aktif
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

        public virtual int m_organisasi_id
        {
            get;
            set;
        }

        public virtual string name_location_secondary
        {
            get;
            set;
        }

       

    }

}
