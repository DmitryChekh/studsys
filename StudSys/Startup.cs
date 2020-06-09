using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.EntityFrameworkCore;
using StudSys.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using StudSys.Options;
using StudSys.Services.Interfaces;
using StudSys.Services;
using StudSys.Models;
using Microsoft.OpenApi.Models;
using StudSys.Models.DbModels;

namespace StudSys
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            var connectionString = Configuration.GetConnectionString("DefaultConnection");

            services.AddDbContext<DataContext>(
                options => options.UseSqlServer(connectionString));

            services.AddIdentity<UserModel, IdentityRole>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddEntityFrameworkStores<DataContext>();

            services.AddScoped<IIdentityService, IdentityService>();
            services.AddScoped<IGroupDataService, GroupDataService>();
            services.AddScoped<IClientDataService, ClientDataService>();
            services.AddScoped<ISubjectService, SubjectService>();
            services.AddScoped<ILessonService, LessonService>();

            services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.RequireHttpsMetadata = false;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,

                        IssuerSigningKey = JwtOptions.GetSymmetricSecurityKey(),
                        ValidIssuer = JwtOptions.Issuer,
                        ValidAudience = JwtOptions.Audience

                    };
                });

            services.AddSwaggerGen(x =>
            {
                x.SwaggerDoc("v0.1", new OpenApiInfo { Title = "StudSys Api", Version = "v0.1" });

                x.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "Jwt authorization header using bearer scheme",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                });

                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            },
                            Scheme = "oauth2",
                            Name = "Bearer",
                            In = ParameterLocation.Header,
                        },
                        new List<string>()
                    }
               });
            });


                services.AddControllers();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseDeveloperExceptionPage();

            app.UseHttpsRedirection();

            app.UseRouting();

            var swaggerOptions = new Options.SwaggerOptions();
            Configuration.GetSection(nameof(Options.SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JSonRoute; });
            app.UseSwaggerUI(option => { option.SwaggerEndpoint(swaggerOptions.UIEndPoint, swaggerOptions.Description); });

            app.UseAuthentication();
            app.UseAuthorization();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
