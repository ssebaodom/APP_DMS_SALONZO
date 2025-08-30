using Microsoft.Extensions.DependencyInjection;
using SSE.Business.Api.v1.Implements;
using SSE.Business.Api.v1.Interfaces;
using SSE.Business.Services.v1.Implements;
using SSE.Business.Services.v1.Interfaces;
using Transshipment.Business.Api.v1.Implements;

namespace SSE.Business.Config
{
    public static class DIBusiness
    {
        public static void RegisterDIBusiness(this IServiceCollection services)
        {
            services.AddScoped<ISmsBLL, SmsBLL>();
            services.AddScoped<IUserBLL, UserBLL>();
            services.AddScoped<IUserBLLService, UserBLLService>();
            services.AddScoped<IReportBLL, ReportBLL>();
            services.AddScoped<IHomeBLL, HomeBLL>();
            services.AddScoped<IOrderBLL, OrderBLL>();
            services.AddScoped<ILetterAuthoBLL, LetterAuthoBLL>();
            services.AddScoped<ICustomerBLL, CustomerBLL>();
            services.AddScoped<IDisCountBLL, DisCountBLL>();
            services.AddScoped<IFulfillmentBLL, FulfillmentBLL>();
            services.AddScoped<IDMSBLL, DMSBLL>();
            services.AddScoped<IHRBLL, HRBLL>();
            services.AddTransient<INotificationBLL, NotificationBLL>();
            services.AddTransient<INotificationService, NotificationService>();
            services.AddTransient<IManufacturingBLL, ManufacturingBLL>();
        }
    }
}