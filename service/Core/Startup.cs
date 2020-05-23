using Core.Controllers;
using Core.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Core
{
    public class Startup
    {
        private IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<IConfiguration>(_configuration);
            services.AddSingleton<IImageClassifierFactory, ImageClassifierFactory>();
            services.AddSingleton<IImageClassifierService, ImageClassifierService>();
            services.AddHostedService<ImageClassifierServiceInitializer>();
            services.AddGrpc();
            services.AddControllers();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            app.UseRouting();

            app.UseEndpoints(endpoints => { endpoints.MapGrpcService<ImageClassifierController>(); });
        }
    }
}