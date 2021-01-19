using LaprTrackr.Backend.Infrastructure;
using LaprTrackr.Backend.Models;
using LaprTrackr.Backend.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Serilog;
using System;
using System.Linq;
using System.Text;
using System.Text.Json;

namespace LaprTrackr.Backend
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddControllers()
                .AddJsonOptions(options => options.JsonSerializerOptions.WriteIndented = true)
                .ConfigureApiBehaviorOptions(options => options.InvalidModelStateResponseFactory = context =>
                {
                    var exception = new LaprTrackrException(LaprTrackrStatusCodes.BodyNotValid, "The specified body is invalid.");
                    exception.ExtraData = context.ModelState.Select(x => x.Value.Errors.First().ErrorMessage).ToList();
                    return exception.GetActionResult();
                });

            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo()
                {
                    Title = "LaprTrackr.Backend",
                    Version = "v1"
                });
                OpenApiSecurityScheme securityScheme = new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "Bearer",
                    In = ParameterLocation.Header,
                    BearerFormat = "JWT",
                    Name = "Authorization",
                    Description = "Please insert JWT with Bearer into field",
                    Reference = new OpenApiReference()
                    {
                        Id = "Bearer",
                        Type = new ReferenceType?(ReferenceType.SecurityScheme)
                    }
                };

                c.AddSecurityDefinition(securityScheme.Reference.Id, securityScheme);
                c.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                  {
                    securityScheme,
                     Array.Empty<string>()
                  }
                });
            });

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = Configuration["Jwt:Issuer"],
                    ValidAudience = Configuration["Jwt:Issuer"],
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Configuration["Jwt:Key"]))
                };
            });

            //services.AddDbContext<LaprTrackrContext>(options => options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));
            services.AddDbContext<LaprTrackrContext>(options => options.UseSqlite("Data Source=database.db"));
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IFoodService, FoodService>();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "LaprTrackr.Backend v1"));
            }           

            app.UseSerilogRequestLogging();
            app.UseHttpsRedirection();
            app.UseCors(options =>
            {
                options.AllowAnyOrigin()
                    .AllowAnyMethod()
                    .AllowAnyHeader();
            });

            app.UseRouting();
            app.UseAuthentication();
            app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapGet("/", req => req.Response.WriteAsync("The service is working properly."));
            });

            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    Log.Error(ex, "Specified path is not found");
                    var exception = new LaprTrackrException(LaprTrackrStatusCodes.NotFound, "Specified path is not found");
                    var dto = exception.GetResponse();

                    context.Response.StatusCode = 404;
                    await context.Response.WriteAsync(JsonSerializer.Serialize(dto));
                }
            });
        }
    }
}
