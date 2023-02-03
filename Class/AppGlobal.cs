using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

//using MySQL.Data.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using web_api_managemen_user;
using web_api_managemen_user.Class;

namespace appglobal
{
    public static class AppGlobal
    {

        public static IConfiguration _configuration = Startup.Configuration;
        //internal static string DEFAULT_SECRET = "JTP_cloud_api";
        internal static string BASE_URL = "";
        internal static string LOG_DIR = @"./LOGS";
        //internal static string MySQL_CS = Startup.Configuration["ConnectionStrings:MySQL"];
        internal static string MySQL_CS = Startup.Configuration["ConnectionStrings:PgSQL"];
        //internal static string MySQL_CS = "Server=localhost;Port=3306;Database=jasuindo_api_db;Uid=root;Pwd=root";
        //internal static string SQLServer_CS = "Server=.\\SQLSERVER2016;Database=kapal_api_db;user id=sa; password=GL-System123;";
        internal static string OVERRIDE_CS = "";
        internal static string OVERRIDE_TM = "";
        
        //internal static string DEFAULT_FILE_SERVER = "http://localhost:51010/JTP_file_storage";
        internal static string DEFAULT_FILE_SERVER = "";
        internal static string OVERRIDE_FILE_SERVER = "";

        internal static List<token_geerator> list_token = new List<token_geerator>();

        public class token_geerator
        {
            public string user_name { get; set; }
            public string token { get; set; }
        }

        public class DatatablesResult
        {
            public int draw { get; set; }
            public int recordsTotal { get; set; }
            public int recordsFiltered { get; set; }
            public Object data { get; set; }
        }

      
        public static string get_path_file_materi_multimedia()
        {
            string file_server = OVERRIDE_FILE_SERVER == "" ? DEFAULT_FILE_SERVER : OVERRIDE_FILE_SERVER;
            return file_server ;
        }

   
        static internal string get_connection_string(string db_server = "MySQL")
        {
            string connection = "";
            if (db_server == "MySQL")
            {
                string file_setting = OVERRIDE_CS;
                connection = file_setting == "" ? MySQL_CS : file_setting;
            }
            return connection;
        }

        static internal string get_connection_string_sqlserver(string db_server = "SqlServer")
        {
            string connection = "";
            if (db_server == "SqlServer")
            {
                string file_setting = OVERRIDE_CS;
                connection = file_setting == "" ? MySQL_CS : file_setting;
            }
            return connection;
        }

        /// <summary>
        /// Get primary working directory for application path searching
        /// </summary>
        /// <returns></returns>
        public static string get_working_directory()
        {
            return BASE_URL; //Path.GetDirectoryName(Assembly.GetEntryAssembly().Location);
        }

       

        // / <summary>
        // / Standardize console logging
        // / </summary>
        public static void console_log(string name, string content)
        {
            Console.WriteLine("======================================================");
            Console.Write(name + " >> ");
            Console.WriteLine(content);
            Console.WriteLine("======================================================");
        }

        public static byte[] SerializeAndCompress(this object obj)
        {
            using (var memoryStream = new MemoryStream())
            {
                using (var gZipStream = new GZipStream(memoryStream, CompressionMode.Compress))
                {
                    BinaryFormatter binaryFormatter = new BinaryFormatter();
                    binaryFormatter.Serialize(gZipStream, obj);
                }
                return memoryStream.ToArray();
            }
        }

        public static string CreateRandomPassword(int length = 15)
        {
            // Create a string of characters, numbers, special characters that allowed in the password  
            string validChars = "ABCDEFGHJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789#$&*_";
            Random random = new Random();

            // Select one random character at a time from the string  
            // and create an array of chars  
            char[] chars = new char[length];
            for (int i = 0; i < length; i++)
            {
                chars[i] = validChars[random.Next(0, validChars.Length)];
            }
            return new string(chars);
        }

        public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo target = null)
        {
            

            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }

       public static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false; // suggested by @TK-421
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }

        public static byte[] get_file_to_byte(string path_foto)
        {
            byte[] image_value = null;
           
            try
            {
                using (var streamReader = new StreamReader(path_foto))
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        streamReader.BaseStream.CopyTo(memoryStream);
                        image_value = memoryStream.ToArray();
                        streamReader.Close();
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                image_value = null;
            }
            return image_value;
        }

    }
}