using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SSE.Core.UoW;
using SSE.DataAccess.Api.v1.Implements;
using SSE.DataAccess.Api.v1.Interfaces;
using SSE.DataAccess.Context;
using SSE.DataAccess.Services.v1.Implements;
using SSE.DataAccess.Services.v1.Interfaces;

namespace SSE.DataAccess.Config
{
    public static class DIDataAccess
    {
        public static void RegisterDIDataAccess(this IServiceCollection services)
        {
            // Authentication Context
            services.AddDbContext<DbContext, GlobalDbContext>();
            services.AddScoped<IUnitOfWork<GlobalDbContext>, UnitOfWork<GlobalDbContext>>();
            services.AddScoped<IUserDAL, UserDAL>();
            services.AddScoped<IUserDALService, UserDALService>();
            services.AddScoped<IHomeDAL, HomeDAL>();
            services.AddScoped<IReportDAL, ReportDAL>();
            services.AddScoped<IOrderDAL, OrderDAL>();
            services.AddScoped<ILetterAuthoDAL, LetterAuthoDAL>();
            services.AddScoped<ICustomerDAL, CustomerDAL>();
            services.AddScoped<IDisCountDAL, DisCountDAL>();
            services.AddScoped<IFulfillmentDAL, FulfillmentDAL>();
            services.AddScoped<IDMSDAL, DMSDAL>();
            services.AddScoped<IHRDAL, HRDAL>();
            services.AddScoped<IManufacturingDAL, ManufacturingDAL>();
        }
    }
}