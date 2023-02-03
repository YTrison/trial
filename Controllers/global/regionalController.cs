using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections;
using System.Linq;
using web_api_managemen_user.Class;
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
using web_api_managemen_user.Class.JWt;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Net;
using System.Net.Mime;
using System.Text;
using appglobal;
using Swashbuckle.AspNetCore.Annotations;

namespace web_api_managemen_user.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("global/[controller]", Name = "Global-Regional")]

    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]


    public class regionalController : ControllerBase
    {
        private readonly ILogger<regionalController> _logger;
        private string INVALID = "INVALID";
        private string VALID = "VALID";
        private model_user_managemen _context;
        private static string DEFAULT_PATH = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static string DOCUMENT_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);


        public regionalController(ILogger<regionalController> logger, IConfiguration configuration)
        {
            //jwt_token._configuration = configuration;
            _logger = logger;
            _context = new model_user_managemen();
        }

        /// <summary>
        /// Get data provinsi
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(nameof(list_provinsi))]
        [SwaggerOperation(Tags = new[] { "Global-Regional" })]
        public async Task<IActionResult> list_provinsi()
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
                    var query = await _context.m_kelurahan_desa.Select(o => new { o.nama_propinsi }).Distinct().OrderBy(e=> e.nama_propinsi).ToListAsync();

                    result.data = query;
                    result.message = "Valid";
                    result.status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    result.message = "Invalid";
                    result.status = false;
                }
            }


            return new JsonResult(result);
        }


        /// <summary>
        /// Get data kabupaten/kota
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(nameof(list_kabupaten))]
        [SwaggerOperation(Tags = new[] { "Global-Regional" })]
        public async Task<JsonResult> list_kabupaten(string provinsi)
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
                    var query = _context.m_kelurahan_desa.Where(e=> e.nama_propinsi.ToLower() == provinsi.ToLower())
                        .Select(e => new { e.jenis_kabupaten_kota, e.nama_kabupaten_kota }).Distinct().OrderBy(e=>e.nama_kabupaten_kota);

                    result.data = query;
                    result.message = "Valid";
                    result.status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    result.message = "Invalid";
                    result.status = false;
                }
            }


            return new JsonResult(result);
        }

        /// <summary>
        /// Get data kecamatan
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(nameof(list_kecamatan))]
        [SwaggerOperation(Tags = new[] { "Global-Regional" })]
        public async Task<JsonResult> list_kecamatan(string nama_kabupaten_kota)
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
                    var query = _context.m_kelurahan_desa.Where(e => e.nama_kabupaten_kota.ToLower() == nama_kabupaten_kota.ToLower())
                        .Select(e => new { e.nama_kecamatan }).Distinct().OrderBy(e=> e.nama_kecamatan);

                    result.data = query;
                    result.message = "Valid";
                    result.status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    result.message = "Invalid";
                    result.status = false;
                }
            }


            return new JsonResult(result);
        }

        /// <summary>
        /// Get data kelurahan
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpGet(nameof(list_kelurahan))]
        [SwaggerOperation(Tags = new[] { "Global-Regional" })]
        public async Task<JsonResult> list_kelurahan(string nama_kecamatan)
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
                    var query = _context.m_kelurahan_desa.Where(e => e.nama_kecamatan.ToLower() == nama_kecamatan.ToLower()).Select(e => new { e.nama_kelurahan_desa,e.kode_pos }).Distinct().OrderBy(e=> e.nama_kelurahan_desa );

                    result.data = query;
                    result.message = "Valid";
                    result.status = true;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                    result.message = "Invalid";
                    result.status = false;
                }
            }


            return new JsonResult(result);
        }
    }

}
