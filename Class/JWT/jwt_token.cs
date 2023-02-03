using appglobal;
using jasuindo_models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static appglobal.AppGlobal;

namespace web_api_managemen_user.Class.JWt
{
    public class jwt_token 
    {
        private IHttpContextAccessor m_httpContextAccessor;
        public static string GenerateJwtToken(jtw_data user)
        {
            string ip_address = MyHttpContext.Current.Request.HttpContext.Request.Headers["User-Agent"].ToString();
            var browser_pv4 = MyHttpContext.Current.Request.HttpContext.Connection.RemoteIpAddress.MapToIPv4().ToString();
            //ip = MyHttpContext.Current.Request.HttpContext.Request.["REMOTE_ADDR"];
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(AppGlobal._configuration["Jwt:key"]);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("username", user.username),
                    new Claim("email", user.email),
                    new Claim("App_id", user.App_id),
                    new Claim("role", user.role),
                    new Claim("ip_address", ip_address),
                    new Claim("browser", browser_pv4)

                }),

                Expires = DateTime.UtcNow.AddHours(user.Expires),
                Issuer = AppGlobal._configuration["Jwt:Issuer"],
                Audience = AppGlobal._configuration["Jwt:Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
                        
            return tokenHandler.WriteToken(token);
        }

        private static string GetIPAddress()
        {
            
            string remoteIpAddress = MyHttpContext.Current.Request.HttpContext.Connection.RemoteIpAddress.ToString();
          
            return remoteIpAddress;
        }
    }

    public class jtw_data
    {
        public string username { set; get; }
        public string email { set; get; }
        public string App_id { set; get; }
        public string role { set; get; }
        public int Expires { set; get; }
    }
}
