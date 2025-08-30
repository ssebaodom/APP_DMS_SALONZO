using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Threading.Tasks;
using Quartz;

using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.AspNetCore.Hosting;
using WindowService_POC.Contract;
using WindowService_POC.Implementation;
using WindowService_POC.Tasks;
namespace SSE.PushNotificationService
{

    class Program
    {
        public static IConfiguration _configuration;
        static async Task Main(string[] args)
        {
            IHost Host = CreateHostBuilder(args).Build();

            await Host.RunAsync();
        }
        public static IHostBuilder CreateHostBuilder(string[] args) => Host.CreateDefaultBuilder(args).UseWindowsService().ConfigureServices(services => {

            InitializeConfigure(args);
            ConfigureQuartzService(services);
            services.AddScoped<ITaskLogTime, TaskLogTime>();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddHttpClient();
            //services.AddSingleton<IWebHostEnvironment>(env => env.GetService<IWebHostEnvironment>());
            //services.RegisterDICore();
            //services.RegisterDIDataAccess();
            //services.RegisterDIBusiness();
        });

       

        private static void ConfigureQuartzService(IServiceCollection services)
        {
            // Add the required Quartz.NET services
            services.AddQuartz(q =>
            {
                // Use a Scoped container to create jobs.
                q.UseMicrosoftDependencyInjectionJobFactory();

                // Create a "key" for the job
                var jobKey = new JobKey("Task1");

                // Register the job with the DI container
                q.AddJob<Task1>(opts => opts.WithIdentity(jobKey));

                // Create a trigger for the job
                q.AddTrigger(opts => opts
                    .ForJob(jobKey) // link to the Task1
                    .WithIdentity("Task1-trigger") // give the trigger a unique name
                    .WithCronSchedule("0 0 8,14 * * ?")); //Bắn lúc 8 giờ sáng, Và bắn lúc 2 giờ chiều, mỗi ngày 0 0 8,14 * * ?
                /// - 0/5 * * * * ?
                //int repeat = Convert.ToInt32(_configuration["NotificationConfig:SendNotification:Repeat"]);
                //q.AddTrigger(opts => opts.ForJob(jobKeyJobSendNotificationBirthday) // link to the Task1
                //    .WithIdentity("triggerJobSendNotificationBirthday") // give the trigger a unique name
                //    .StartNow()
                //    .WithSimpleSchedule(x => x.WithIntervalInMinutes(repeat).RepeatForever().Build()));
            });

            // Add the Quartz.NET hosted service
            services.AddQuartzHostedService(
                q => q.WaitForJobsToComplete = true);
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
