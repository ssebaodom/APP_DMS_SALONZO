using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Quartz;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace SSE.WindowService
{
    class Program
    {
        public static IConfiguration _configuration;
        static async Task Main(string[] args)
        {
            IHost Host = CreateHostBuilder(args).Build();
            await Host.RunAsync();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).UseWindowsService().ConfigureServices(services =>
        {
            InitializeConfigure(args);
            ConfigureQuartzService(services);
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();

            services.RegisterDICore();
            services.RegisterDIDataAccess();
            services.RegisterDIBusiness();
        });
        private static void ConfigureQuartzService(IServiceCollection services)
        {
            // Add the required Quartz.NET services
            services.AddQuartz(q =>
            {
                // Use a Scoped container to create jobs.
                q.UseMicrosoftDependencyInjectionJobFactory();
                // Create a "key" for the job
                var jobKeyWarningNotAcceptCustomer = new JobKey("JobWarningNotAcceptCustomer");
                // Register the job with the DI container
                //q.AddJob<JobWarningNotAcceptCustomer>(opts => opts.WithIdentity(jobKeyWarningNotAcceptCustomer));
                // Create a trigger for the job
                int repeat = Convert.ToInt32(_configuration["NotificationConfig:CanhBaoKhachChuaXuLy:Repeat"]);
                q.AddTrigger(opts => opts.ForJob(jobKeyWarningNotAcceptCustomer) // link to the Task1
                    .WithIdentity("triggerWarningNotAcceptCustomer") // give the trigger a unique name
                    .StartNow()
                    .WithSimpleSchedule(x => x.WithIntervalInMinutes(repeat).RepeatForever().Build())); // run every 5 seconds
            });
            // Add the Quartz.NET hosted service
            services.AddQuartzHostedService(q => q.WaitForJobsToComplete = true);
        }
        public static void InitializeConfigure(string[] args)
        {
            _configuration = new ConfigurationBuilder()
             .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
             .AddEnvironmentVariables()
             .AddCommandLine(args)
              .Build();
        }
    }
}
