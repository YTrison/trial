using appglobal;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using web_api.JWT;
using web_api_managemen_user.Class;

namespace web_api_managemen_user.JWT
{
    public class JWTMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IConfiguration _configuration;
        private readonly IUserService _userService;

        public JWTMiddleware(RequestDelegate next, IConfiguration configuration, IUserService userService)
        {
            _next = next;
            _configuration = configuration;
            _userService = userService;
        }

        public async Task Invoke(HttpContext context)
        {
            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();

            if (token == null)
            {
                await _next(context);

                return;
            }


            var data_token_list = AppGlobal.list_token.SingleOrDefault(e => e.token == token) != null ? token : null;

            
            if (token != null)
            {
                if (attachAccountToContext(context, token))
                {
                    await _next(context);
                }
               

                return;
            }
           
            context.Response.StatusCode = (int)StatusCodes.Status401Unauthorized;
        }

        private bool attachAccountToContext(HttpContext context, string token)
        {
            
            try
            {
                var tokenHandler = new JwtSecurityTokenHandler();
                string KeyJwt = _configuration["Jwt:Key"].ToString();
                var key = Encoding.ASCII.GetBytes(_configuration["Jwt:Key"]);
                tokenHandler.ValidateToken(token, new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(key),
                    ValidIssuer = _configuration["Jwt:Issuer"],
                    ValidAudience = _configuration["Jwt:Audience"],
                    // set clockskew to zero so tokens expire exactly at token expiration time (instead of 5 minutes later)
                    ClockSkew = TimeSpan.Zero
                }, out SecurityToken validatedToken);

                var jwtToken = (JwtSecurityToken)validatedToken;
                
                var accountId = jwtToken.Claims.First(x => x.Type == "username").Value;

                // attach account to context on successful jwt validation
                context.Items["User"] = accountId;
                var account = context.Items["User"];

                return true;
            }
            catch(Exception ex)
            {
                context.Response.Headers.Add("Message", "Token is Expired");
                context.Response.StatusCode = (int)StatusCodes.Status401Unauthorized;
                return false;
                //context.Features.Get<IHttpResponseFeature>().ReasonPhrase = "Token is Expired";
                //return new JsonResult(new { Error = true, Message = "Token is Expired" }, (int)StatusCodes.Status401Unauthorized);
            }
            
        }


    }
}
