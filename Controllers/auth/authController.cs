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
using System.Net.Mail;
using System.Net.Mime;
using System.Text;
using Newtonsoft.Json.Linq;
using appglobal;
using CoreEncryption;
using Swashbuckle.AspNetCore.Annotations;
using static appglobal.AppGlobal;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using HashKey;

namespace web_api_managemen_user.Controllers
{
    //[Authorize]
    [ApiController]
    [Route("auth/[controller]", Name = "User Management-Auth")]
    [ProducesResponseType(typeof(string), StatusCodes.Status500InternalServerError)]
    [ProducesResponseType(typeof(IEnumerable), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]


    public class authController : ControllerBase
    {
        private readonly ILogger<authController> _logger;
        private string INVALID = "INVALID";
        private string VALID = "VALID";
        private model_user_managemen _context;
        private static string DEFAULT_PATH = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
        private static string DOCUMENT_PATH = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

        public authController(ILogger<authController> logger, IConfiguration configuration)
        {
            ///AppGlobal._configuration = configuration;
            _logger = logger;
            _context = new model_user_managemen();
        }

        /// <summary>
        /// Login User 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost(nameof(login_user))]
        [SwaggerOperation(Tags = new[] { "User Management-Auth" })]
        public async Task<IActionResult> login_user([FromForm] login_data user_data)
        {
            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.message = "invalid";
            result.status = true;
            try
            {
                string password = KeyGenerator.GetPassword(user_data.password);
                var data_user = _context.m_user.SingleOrDefault(e => e.m_user_email == user_data.email ||
                                e.m_username == user_data.email
                                && e.m_user_password == password && e.status == true);
                output_data_login data_ouput = new output_data_login();
                if (data_user != null)
                {
                    // generate token jwt user login
                    #region generate token jwt
                    var m_group_user = _context.map_group_user.FirstOrDefault(e => e.m_user_id == data_user.m_user_id);
                    string key_project = _context.m_project_application.SingleOrDefault(e => e.m_project_application_id == user_data.application_id).key_project;
                    if (m_group_user == null)
                    {
                        jtw_data jtw_data_new = new jtw_data
                        {
                            username = data_user.m_username,
                            email = data_user.m_user_email,
                            role = "Administrator",
                            Expires = 1,
                            App_id = key_project
                        };
                        string token_new = jwt_token.GenerateJwtToken(jtw_data_new);

                        string ref1 = data_user.ref1;
                        string ref2 = data_user.ref2;
                        string ref3 = data_user.ref3;


                        data_user user_data_item = new data_user
                        {
                            m_user_id = data_user.m_user_id,
                            m_username = data_user.m_username,
                            status_aktif = data_user.status_aktif,
                            m_user_email = data_user.m_user_email,
                            ref1 = ref1,
                            ref2 = ref2,
                            ref3 = ref3,
                            m_group_user_id = 1
                        };

                        data_ouput.token = token_new;
                        data_ouput.map_role_feature = null;
                        data_ouput.m_user = user_data_item;
                        data_ouput.lokasi = null;
                        result.data = data_ouput;
                        return new JsonResult(result);
                    }
                    var m_group_user_name = _context.m_group_user.SingleOrDefault(e => e.m_group_user_id == m_group_user.m_group_user_id).m_group_user_name;

                    // data token jwt
                    jtw_data jtw_data = new jtw_data
                    {
                        username = data_user.m_username,
                        email = data_user.m_user_email,
                        role = m_group_user_name,
                        Expires = 1,
                        App_id = key_project
                    };
                    string token = jwt_token.GenerateJwtToken(jtw_data);

                    var data_token_list = list_token.SingleOrDefault(e => e.user_name == data_user.m_username);

                    if (data_token_list != null)
                    {
                        list_token.Remove(data_token_list);
                    }
                    #endregion

                    // query untuk mendapatkan lokasi user yang terdaftar
                    #region query lokasi user terdaftar
                    var query_lokasi = from map_user_lokasi in _context.map_user_lokasi
                                       join lokasi in _context.m_lokasi on map_user_lokasi.m_lokasi_id equals lokasi.m_lokasi_id
                                       join organisasi in _context.m_organisasi on lokasi.m_organisasi_id equals organisasi.m_organisasi_id
                                       where map_user_lokasi.m_user_id == data_user.m_user_id && map_user_lokasi.m_project_application_id == user_data.application_id
                                       select new
                                       {
                                           organisasi.m_organisasi_id,
                                           organisasi.nama_oganisasi,
                                           lokasi.m_lokasi_id,
                                           lokasi.nama_lokasi
                                       };
                    #endregion

                    // query untuk mendapatkan data fitur sesuai dengan login user
                    #region query untuk mendapatkan data fitur sesuai dengan login user
                    var query_grub_user = _context.map_group_user.Where(e => e.m_user_id == data_user.m_user_id && e.m_project_application_id == user_data.application_id);

                    // query untuk medapatkan data map_feature_group sesuai dengan fitur yang telah didaftarkan
                    List<map_feature_group> query_map_feature = new List<map_feature_group>();
                    query_map_feature = await _context.map_feature_group.Where(e => query_grub_user.Any(j => j.m_group_user_id == e.m_group_user_id) &&
                                             _context.map_feature_group_project.Any(a => a.m_project_application_id == user_data.application_id && e.map_feature_group_project_id == a.map_feature_group_project_id)).AsNoTracking().AsQueryable()
                        .Include(e => e.map_feature_group_project).ThenInclude(e => e.m_feature)
                        .Include(e => e.map_feature_group_project).ThenInclude(e => e.m_project_application)
                        .Include(e => e.map_feature_group_project).ThenInclude(e => e.m_group_feature)
                        .Include(e => e.m_group_user).ToListAsync();

                    // membuat data baru sesuai dengan map_feature_group yang telah didapatkan
                    List<feature_group_custom> list_feature_group_custom = new List<feature_group_custom>();
                    foreach (var item in query_grub_user.ToList())
                    {
                        var role = await _context.m_group_user.FirstOrDefaultAsync(e => e.m_group_user_id == item.m_group_user_id);
                        var data_new = from map_feature in query_map_feature
                                       where map_feature.m_group_user_id == item.m_group_user_id
                                       select new
                                       {
                                           id = map_feature.map_feature_group_project_id,
                                           map_feature.action_feature
                                       };

                        List<m_group_feature_custom> data_m_group_feature = new List<m_group_feature_custom>();

                        foreach (var item_detail in data_new)
                        {
                            var map_feature_group_project = _context.map_feature_group_project.SingleOrDefault(e => e.map_feature_group_project_id == item_detail.id);
                            var check = data_m_group_feature.Any(e => e.m_group_feature_id == map_feature_group_project.m_group_feature_id);

                            if (check == false)
                            {
                                var group_feature = _context.m_group_feature.SingleOrDefault(e => map_feature_group_project.m_group_feature_id == e.m_group_feature_id);
                                List<m_feature_custom> m_feature_custom_list = new List<m_feature_custom>();
                                foreach (var item_feature in data_new)
                                {
                                    var m_feature_id_status = _context.map_feature_group_project.Any(e => e.map_feature_group_project_id == item_feature.id && e.m_group_feature_id == group_feature.m_group_feature_id);
                                    if (m_feature_id_status != false)
                                    {
                                        var m_feature_id = _context.map_feature_group_project.SingleOrDefault(e => e.map_feature_group_project_id == item_feature.id && e.m_group_feature_id == group_feature.m_group_feature_id).m_feature_id;
                                        if (map_feature_group_project.m_group_feature_id == group_feature.m_group_feature_id)
                                        {
                                            var feature = _context.m_feature.SingleOrDefault(e => e.m_feature_id == m_feature_id);
                                            m_feature_custom m_feature_custom_data = new m_feature_custom()
                                            {
                                                m_feature_id = feature.m_feature_id,
                                                name_link_feature = feature.name_link_feature,
                                                m_feature_name = feature.m_feature_name,
                                                feature_icon = feature.feature_icon,
                                                feature_action = item_feature.action_feature,
                                                index = feature.index
                                            };
                                            m_feature_custom_list.Add(m_feature_custom_data);
                                        }
                                    }
                                }
                                m_group_feature_custom data = new m_group_feature_custom()
                                {
                                    m_group_feature_id = group_feature.m_group_feature_id,
                                    group_feature_name = group_feature.group_feature_name,
                                    index = group_feature.index,
                                    status = group_feature.status,
                                    feature_custom = m_feature_custom_list
                                };
                                data_m_group_feature.Add(data);
                            }

                        }

                        m_group_user_custom m_group_user_custom_data = new m_group_user_custom()
                        {
                            m_group_user_id = role.m_group_user_id,
                            m_group_user_name = role.m_group_user_name,
                            status = role.status,
                            group_feature = data_m_group_feature

                        };

                        feature_group_custom data_feature_group_custom = new feature_group_custom()
                        {
                            group_user = m_group_user_custom_data,

                        };
                        list_feature_group_custom.Add(data_feature_group_custom);
                    }

                    var data_project = _context.m_project_application.SingleOrDefault(e => e.m_project_application_id == user_data.application_id);
                    m_project_application_custom m_project_application_custom_data = new m_project_application_custom()
                    {
                        m_project_application_id = data_project.m_project_application_id,
                        key_project = data_project.key_project,
                        m_project_application_name = data_project.m_project_application_name,
                        status_aktif = data_project.status_aktif,
                        feature_group_user = list_feature_group_custom

                    };
                    m_application_custom data_m_application_custom = new m_application_custom();

                    data_m_application_custom.m_project_application = m_project_application_custom_data;

                    #endregion


                    data_user data_user_item = new data_user
                    {
                        m_username = data_user.m_username,
                        status_aktif = data_user.status_aktif,
                        m_user_email = data_user.m_user_email,
                        m_group_user_id = m_group_user.m_group_user_id
                    };

                    data_ouput.token = token;
                    data_ouput.map_role_feature = data_m_application_custom;
                    data_ouput.m_user = data_user_item;
                    data_ouput.lokasi = query_lokasi;
                    result.data = data_ouput;

                    token_geerator data_token = new token_geerator
                    {
                        user_name = data_user.m_username,
                        token = token
                    };
                    list_token.Add(data_token);

                    
                    result.message = "Login Sukses";
                    result.status = true;
                }
                else
                {
                   
                    result.message = "Login Gagal";
                    result.status = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result.data = null;
                result.message = "User Tidak Mempunyai Role Akses";
                result.status = false;
                return BadRequest(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// Login User 
        /// </summary>
        /// <returns></returns>
        [AllowAnonymous]
        [HttpPost(nameof(login_user_test))]
        [SwaggerOperation(Tags = new[] { "User Management-Auth" })]
        public async Task<IActionResult> login_user_test([FromForm] login_data user_data)
        {
            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.message = "invalid";
            result.status = true;
            try
            {
                string password = KeyGenerator.GetPassword(user_data.password);
                var data_user = _context.m_user.SingleOrDefault(e => e.m_user_email == user_data.email ||
                                e.m_username == user_data.email
                                && e.m_user_password == password && e.status == true);
                string key_project = _context.m_project_application.SingleOrDefault(e => e.m_project_application_id == user_data.application_id).key_project;
                output_data_login data_ouput = new output_data_login();
                if (data_user != null)
                {
                 
                    jtw_data jtw_data = new jtw_data
                    {
                        username = data_user.m_username,
                        email = data_user.m_user_email,
                        role = "Admin",
                        Expires = 10,
                        App_id = key_project
                    };
                    string token = jwt_token.GenerateJwtToken(jtw_data);
                    var query_feature = _context.m_feature.Take(3);
                    string json = JsonConvert.SerializeObject(query_feature);
                    string data_feature = EncryptHandler.ProcessTextToText(json, "", mode: "crypt");
                    data_ouput.token = token;
                    data_ouput.m_user =null;
                    data_ouput.lokasi = null;
                    result.data = data_ouput;
                    result.message = "Login Sukses";
                    result.status = true;
                }
                else
                {

                    result.message = "Login Gagal";
                    result.status = false;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result.data = null;
                result.message = "User Tidak Mempunyai Role Akses";
                result.status = false;
                return BadRequest(result);
            }
            return new JsonResult(result);
        }

        /// <summary>
        /// post change password 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost(nameof(change_password))]
        [SwaggerOperation(Tags = new[] { "User Management-Auth" })]
        public async Task<IActionResult> change_password([FromForm] reset_password_data data)
        {
            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;
            try
            {

                if (
                    data.Password_baru.Length >= 8 &&  //if length is >= 6
                    data.Password_baru.Any(char.IsUpper) &&  //if any character is upper case
                    data.Password_baru.Any(char.IsNumber) //better to have a digit then Number
                  )
                {
                    //valid
                }
                else
                {
                    result.message = "Password Minimal 8 karakter,huruf besar dan angka";
                    result.status = false;
                    return new JsonResult(result);
                }

                var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                //int appID = int.Parse(claims[Constants.APPID]);
                var name = claims["username"];
                var email = User.Claims.SingleOrDefault(e => e.Type.Contains("email")).Value;
                string role = User.Claims.SingleOrDefault(e => e.Type.Contains("role")).Value;

                string passwordLama = KeyGenerator.GetPassword(data.password_lama);
                var data_user = _context.m_user.SingleOrDefault(e => e.m_username == name && e.m_user_email == email && e.m_user_password == passwordLama);
                if (data_user == null)
                {
                    result.message = "Cek username,email dan password lama";
                    result.status = false;
                    return new JsonResult(result);
                }
                data_user.m_user_password = KeyGenerator.GetPassword(data.Password_baru);
                data_user.status_aktif = "valid";
                _context.m_user.Update(data_user);
                await _context.SaveChangesAsync();
                result.data = data_user.m_username;
                result.status = true;
                result.message = VALID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result.data = null;
                result.message = ex.Message;
                result.status = false;
                return BadRequest(result);
            }

            return new JsonResult(result);
        }


        /// <summary>
        /// post reset password 
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        //[Authorize]
        [HttpPost(nameof(reset_password))]
        [SwaggerOperation(Tags = new[] { "User Management-Auth" })]
        public async Task<IActionResult> reset_password([FromForm] reset_password data)
        {
            ValidationAPIResult result = new ValidationAPIResult();
            result.data = null;
            result.status = false;
            result.message = INVALID;
            try
            {



                /*var claims = User.Claims.ToDictionary(x => x.Type, x => x.Value);
                //int appID = int.Parse(claims[Constants.APPID]);
                var name = claims["username"];
                var email = User.Claims.SingleOrDefault(e => e.Type.Contains("email")).Value;
                string role = User.Claims.SingleOrDefault(e => e.Type.Contains("role")).Value;*/

                //string passwordDefault = KeyGenerator.GetPassword("1234");
                var data_user = _context.m_user.SingleOrDefault(e => e.m_user_email == data.user_email);
                var key_project = _context.m_project_application.SingleOrDefault(e => e.m_project_application_id == data.application_id);
                if (data_user == null)
                {
                    result.message = "Email Anda Salah";
                    result.status = false;
                    return BadRequest(result);
                }

                if (key_project == null)
                {
                    result.message = "Maaf anda bukan user aplikasi ini";
                    result.status = false;
                    return BadRequest(result);
                }

                data_user.m_user_password = KeyGenerator.GetPassword("1234");
                data_user.status_aktif = "valid";
                _context.m_user.Update(data_user);
                await _context.SaveChangesAsync();
                result.data = data_user.m_username;
                result.status = true;
                result.message = VALID;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                result.data = null;
                result.message = ex.Message;
                result.status = false;
                return BadRequest(result);
            }

            return new JsonResult(result);
        }


        private partial class m_project_application_custom
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

            public virtual bool status_aktif
            {
                get;
                set;
            }

            public virtual string key_project
            {
                get;
                set;
            }
            public List<feature_group_custom> feature_group_user { set; get; }
        }


        private partial class m_group_user_custom
        {

           
            public virtual int m_group_user_id
            {
                get;
                set;
            }

            public virtual string m_group_user_name
            {
                get;
                set;
            }

            public virtual bool status
            {
                get;
                set;
            }
            public List<m_group_feature_custom> group_feature { set; get; }

        }


        private partial class m_group_feature_custom
        {

            public virtual int m_group_feature_id
            {
                get;
                set;
            }

            public virtual bool status
            {
                get;
                set;
            }

            public virtual int index
            {
                get;
                set;
            }

           
            public virtual string group_feature_name
            {
                get;
                set;
            }

            public List<m_feature_custom> feature_custom { set; get; }

        }

        private partial class m_feature_custom
        {

         
            public virtual int m_feature_id
            {
                get;
                set;
            }

            public virtual string m_feature_name
            {
                get;
                set;
            }

            public virtual int? index
            {
                get;
                set;
            }

            public virtual string name_link_feature
            {
                get;
                set;
            }

            public virtual bool status
            {
                get;
                set;
            }

           
            public virtual string feature_icon
            {
                get;
                set;
            }

            public virtual string feature_action
            {
                get;
                set;
            }

        }

        private class feature_group_custom
        {
            public m_group_user_custom group_user { set; get; }
           
        }
        private class m_application_custom
        {
           public m_project_application_custom m_project_application { set; get; }

            
        }

       
        private class output_data_login
        {
            public string token { set; get; }

            public dynamic m_user { set; get; }

            public dynamic lokasi { set; get; }

            public m_application_custom map_role_feature { set; get; }

        }

        

        private class data_reset_password
        {
            public string username { set; get; }

            public string user_email { set; get; }

            public string link_id { set; get; }
        }

        private class data_user
        {

            public System.Guid m_user_id { set; get; }
            public string m_username { set; get; }

            public string status_aktif { set; get; }

            public string m_user_email { set; get; }
            public string ref1 { set; get; }
            public string ref2 { set; get; }

            public string ref3 { set; get; }

            public int m_group_user_id { set; get; }
        }
    }

    public class login_data
    {
        [Required]
        public string email { set; get; }

        [Required]
        public string password { set; get; }

        [Required]
        public int application_id { set; get; }
    }

    public class forget_login_data
    {
        [Required]
        public string username { set; get; }

        [Required]
        public string user_email { set; get; }

        [Required]
        public string application_id { set; get; }
    }

    public class reset_password_data
    {
        [Required]
        public string password_lama { set; get; }

        [Required]
        public string Password_baru { set; get; }
    }

    public class reset_password
    {
        [Required]
        public string user_email { set; get; }

        [Required]
        public int application_id { set; get; }
    }
}
