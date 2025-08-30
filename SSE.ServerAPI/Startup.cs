using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.FileProviders;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using SSE.Business.Config;
using SSE.Core.Common.Constants;
using SSE.Core.Config;
using SSE.Core.Services.JwT;
using SSE.Core.Services.Versioning;
using SSE.DataAccess.Config;
using SSE.DataAccess.Context;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.IdentityModel.Tokens.Jwt;
using System.IO;

namespace SSE_Server
{
    public class Startup
    {
        public IConfiguration Configuration { get; }
        public IJwtService jwtService { set; get; }

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            this.jwtService = new JwtService();
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("Policy1",
                    builder =>
                    {
                        builder.WithOrigins("http://localhost:3000");
                    });

            });
            services.AddControllers();

            services.AddControllers().AddNewtonsoftJson();

            services.TryAddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            // ===== Add Versioning Service ========
            services.AddApiVersioning(options =>
            {
                // Add the headers "api-supported-versions" and "api-deprecated-versions"
                // This is better for discoverability
                options.ReportApiVersions = true;

                // AssumeDefaultVersionWhenUnspecified should only be enabled when supporting legacy services that did not previously
                // support API versioning. Forcing existing clients to specify an explicit API version for an
                // existing service introduces a breaking change. Conceptually, clients in this situation are
                // bound to some API version of a service, but they don't know what it is and never explicit request it.
                options.AssumeDefaultVersionWhenUnspecified = true;
                options.DefaultApiVersion = new ApiVersion(2, 0);

                // Defines how an API version is read from the current HTTP request
                // options.ApiVersionReader = ApiVersionReader.Combine(
                //    new QueryStringApiVersionReader("api-version"),
                //    new HeaderApiVersionReader("api-version"));
            });

            services.AddVersionedApiExplorer(options =>
            {
                // add the versioned api explorer, which also adds IApiVersionDescriptionProvider service
                // note: the specified format code will format the version as "'v'major[.minor][-status]"
                options.GroupNameFormat = "'v'VVV";

                // note: this option is only necessary when versioning by url segment. the SubstitutionFormat
                // can also be used to control the format of the API version in route templates
                options.SubstituteApiVersionInUrl = true;
            });
            services.AddTransient<IConfigureOptions<SwaggerGenOptions>, ConfigureSwaggerOptions>();
            services.AddSwaggerGen(options =>
            {
                options.AddSecurityDefinition("bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "JWT Authorization header using the Bearer scheme.",
                });

                /////Add Operation Specific Authorization///////
                options.OperationFilter<AuthOperationFilter>();
                options.OperationFilter<SwaggerDefaultValues>();
                ////////////////////////////////////////////////
            });

            // ===== Add Jwt Authentication ========
            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(cfg =>
            {
                cfg.RequireHttpsMetadata = false;
                cfg.SaveToken = true;
                cfg.TokenValidationParameters =
                this.jwtService.GetValidationParameters(
                    this.Configuration[string.Concat(CONFIGURATION_KEYS.JWT, ":", CONFIGURATION_KEYS.JWT_ISSUER)],
                    this.Configuration[string.Concat(CONFIGURATION_KEYS.JWT, ":", CONFIGURATION_KEYS.JWT_AUDIENCE)],
                    this.Configuration[string.Concat(CONFIGURATION_KEYS.JWT, ":", CONFIGURATION_KEYS.JWT_KEY)]);
            });

            // ===== Add Db Context ========
            services.AddDbContextPool<GlobalDbContext>(options =>
            {
                string con = Configuration.GetConnectionString(CONFIGURATION_KEYS.GLOBAL_CONNECTION_STRING);
                options.UseSqlServer(con);
            }, 1000);

            // ===== Add Dependency Injection ========
            services.RegisterDICore();
            services.RegisterDIDataAccess();
            services.RegisterDIBusiness();

            // ===== Register HttpContext Dependency Injection ========
            services.AddHttpContextAccessor();

           
            services.AddAuthorization(options =>
            {
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build();
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
                public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IApiVersionDescriptionProvider provider)
                {
                    if (env.IsDevelopment())
                    {
                        app.UseDeveloperExceptionPage();

                        // Register the Swagger generator and the Swagger UI middlewares
                        app.UseSwagger();
                        app.UseSwaggerUI(                           
                            options =>
                            {
                                // build a swagger endpoint for each discovered API version
                                foreach (var description in provider.ApiVersionDescriptions)
                                {
                                    options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", description.GroupName.ToUpperInvariant());
                                }
                            });
                    }
                    else
                    {
                        app.UseExceptionHandler("/exception");
                    }

                    app.UseStaticFiles();
                    app.UseStaticFiles(new StaticFileOptions
                    {
                        FileProvider = new PhysicalFileProvider(
                              Path.Combine(env.ContentRootPath, "fsdUpload")),
                        RequestPath = "/fsdUpload"
                    });
                    app.UseFileServer();

                    app.UseMiddleware<SSE.Common.Services.ApiLoggingMiddleware>();

                    app.UseHttpsRedirection();

                    app.UseRouting();

                    app.UseCors("Policy1");

                    app.UseAuthentication();

                    app.UseAuthorization();

                    app.UseEndpoints(endpoints =>
                    {
                        endpoints.MapControllers();
                        endpoints.MapGet("/", async context =>
                        {
                            await context.Response.WriteAsync("ERP server is started!");
                        });
                    });
                }
    }
}