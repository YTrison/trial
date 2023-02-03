using jasuindo_models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace web_api.JWT
{
    public interface IUserService
    {
        m_user IsValidUserInformation(LoginModel model);
    }

    public class UserService : IUserService
    {

        private model_user_managemen _context = new model_user_managemen();
        public m_user IsValidUserInformation(LoginModel model)
        {
            var password = generateMD5(model.Password);
            var user = _context.m_user.SingleOrDefault(e => e.m_username == model.UserName && e.m_user_password == password);
            return user;
        }

        private string generateMD5(string password)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes = Encoding.ASCII.GetBytes(password);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < hashBytes.Length; i++)
            {
                sb.Append(hashBytes[i].ToString("X2"));
            }
            return sb.ToString().ToLower();
            //return password;
        }


    }

    public class LoginModel
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
        [Required]
        public int app_id { get; set; }
    }
}
