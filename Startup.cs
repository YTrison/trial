using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Reflection;
using System.IO;
using Microsoft.EntityFrameworkCore;
using appglobal;
using web_api_managemen_user.JWT;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using FluentValidation.AspNetCore;
using web_api_managemen_user.Class;
using jasuindo_models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.FileProviders;
using Microsoft.AspNetCore.Diagnostics;
using web_api.JWT;
using Microsoft.EntityFrameworkCore.Migrations;
using web_api_managemen_user.Class.Service;

namespace web_api_managemen_user
{
    public class Startup
    {
        readonly string MyAllowSpecificOrigins = "AllowAllOrigins";
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;

            //AppGlobal._configuration = configuration;
        }

        public static IConfiguration Configuration;

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy(MyAllowSpecificOrigins, builder =>
                {
                    builder.AllowAnyOrigin();
                    builder.AllowAnyMethod();
                    builder.AllowAnyHeader();
                });
            });
            services.AddControllersWithViews();
            services.AddMvc(options =>
            {
                options.Filters.Add(typeof(ReformatValidationProblemAttribute));
                options.OutputFormatters.Add(new HtmlOutputFormatter());
            });
            
            services.AddControllers(o =>
            {
                o.Conventions.Add(new ControllerDocumentationConvention());
            });
            services.AddHttpClient();
            services.AddSingleton<IUriService>(o =>
            {
                var accessor = o.GetRequiredService<IHttpContextAccessor>();
                var request = accessor.HttpContext.Request;
                var uri = string.Concat(request.Scheme, "://", request.Host.ToUriComponent());
                return new UriService(uri);
            });

            services.AddControllers()
                .AddFluentValidation(s =>
                {
                    s.RunDefaultMvcValidationAfterFluentValidationExecutes = false;
                    s.RegisterValidatorsFromAssemblyContaining<Startup>();
                });
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddSwaggerGen(options =>
            {
                options.EnableAnnotations();
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Version = "v1",
                    Title = "User Management API Develoment",
                    Description = "An ASP.NET Core Web API for managing ToDo items",
                    TermsOfService = new Uri("https://example.com/terms"),
                    Contact = new OpenApiContact
                    {
                        Name = "Example Contact",
                        Url = new Uri("https://example.com/contact")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "Example License",
                        Url = new Uri("https://example.com/license")
                    }
                });

                if (Startup.Configuration["SwaggerView:status"].ToLower() == "false")
                    options.DocumentFilter<CustomSwaggerFilter>();

                if (Startup.Configuration["SwaggerView:status"].ToLower() == "true")
                {
                    options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme()
                    {
                        Name = "Authorization",
                        Type = SecuritySchemeType.ApiKey,
                        Scheme = "Bearer",
                        BearerFormat = "JWT",
                        In = ParameterLocation.Header,
                        Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                    });
                    options.AddSecurityRequirement(new OpenApiSecurityRequirement
                {

                    {
                          new OpenApiSecurityScheme
                            {
                                Reference = new OpenApiReference
                                {
                                    Type = ReferenceType.SecurityScheme,
                                    Id = "Bearer"
                                }
                            },
                            new string[] {}
                    }
                });
                }

                // using System.Reflection;
                var xmlFilename = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, xmlFilename));
            });

            #region Authentication
            services.AddAuthentication(option =>
            {
                option.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                option.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = false,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Audience"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"])) //Configuration["JwtToken:SecretKey"]
                };
            });
            #endregion

            
            services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore);
            services.AddTransient<IUserService, UserService>();

            string test = Startup.Configuration["ConnectionStrings:PgSQL"];
            services.AddDbContext<model_user_managemen>(options =>options.UseNpgsql(Startup.Configuration["ConnectionStrings:PgSQL"]));

        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHttpContextAccessor httpContextAccessor)
        {

            

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
               
            }
            app.UseExceptionHandler(errorApp =>
            {
                errorApp.Use((context, next) =>
                {
                    context.Response.StatusCode = 400;
                    var error = context.Features.Get<IExceptionHandlerFeature>();
                    var response = new BadRequestObjectResult(error.Error.Message);

                    return context.WriteModelAsync(response);
                    //return Executor.ExecuteAsync(actionContext, result);
                });
            });
            app.UseHsts();
            if (Startup.Configuration["SwaggerView:status"].ToLower() == "true")
            {
                app.UseSwagger();

                app.UseSwaggerUI(c =>
                {
                    c.SwaggerEndpoint("/swagger/v1/swagger.json", "WEB API V1 JTP");

                    c.DefaultModelsExpandDepth(-1);

                    // To serve SwaggerUI at application's root page, set the RoutePrefix property to an empty string.
                    c.RoutePrefix = string.Empty;
                });
            }

            app.UseMiddleware<JWTMiddleware>();

            app.UseHttpsRedirection();

            PhysicalFileProvider fileProvider = new PhysicalFileProvider(
            Path.Combine(env.ContentRootPath, @"Assetts"));

            DefaultFilesOptions defoptions = new DefaultFilesOptions();
            defoptions.DefaultFileNames.Clear();
            defoptions.FileProvider = fileProvider;
            defoptions.DefaultFileNames.Add("index.cshtml");
            app.UseDefaultFiles(defoptions);

            app.UseStaticFiles();

            app.UseRouting();
            app.UseCors(MyAllowSpecificOrigins);
            app.UseAuthentication();
            app.UseAuthorization();

            //app.UseStaticFiles(new StaticFileOptions()
            //{
            //    FileProvider =new PhysicalFileProvider(
            //    Path.Combine(env.ContentRootPath, @"Assetts")),
            //    RequestPath = new PathString(""),
            //});
            app.UseStaticFiles(new StaticFileOptions
            {
                FileProvider = new PhysicalFileProvider(
                     Path.Combine(env.ContentRootPath, "Assetts")),
                RequestPath = new PathString(""),
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });

            UpdateDatabase(app);

            app.UseHttpContext();
        }

        private static void UpdateDatabase(IApplicationBuilder app)
        {
            using (var serviceScope = app.ApplicationServices
                .GetRequiredService<IServiceScopeFactory>()
                .CreateScope())
            {
                using (var context = serviceScope.ServiceProvider.GetService<model_user_managemen>())
                {
                    context.Database.Migrate();
                    Init_data_kelurahan.Initialize();
                    //Init_data_user_management.Initialize();
                    
                }
            }
        }




    }
}
