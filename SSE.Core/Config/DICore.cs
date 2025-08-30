using Dapper;
using Microsoft.Extensions.DependencyInjection;
using SSE.Core.Common.Factories;
using SSE.Core.Common.Logger;
using SSE.Core.Services.Caches;
using SSE.Core.Services.Dapper;
using SSE.Core.Services.Files;
using SSE.Core.Services.JwT;
using SSE.Core.Services.Mail;

namespace SSE.Core.Config
{
    public static class DICore
    {
        public static void RegisterDICore(this IServiceCollection services)
        {
            // Dapper service
            services.AddScoped<DynamicParameterMap, DynamicParameterMap>();
            services.AddScoped<IDapperService, DapperService>();
            SqlMapper.AddTypeHandler(new TrimmedStringHandler());
            // Email service
            services.AddScoped<IMailService, MailService>();
            // File service
            services.AddScoped<IFileService, FileService>();
            
            // JwtService
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<CodeFactory, CodeFactory>();
            services.AddScoped<TokenFactory, TokenFactory>();
            // Redis cache
            //services.AddSingleton<RedisPool>();
            services.AddScoped<ICached, RedisCached>();
            services.AddScoped<ILoggerManager, LoggerManager>();
        }
    }
}