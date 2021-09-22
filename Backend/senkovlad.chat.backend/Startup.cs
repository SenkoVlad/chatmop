using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using senkovlad.chat.backend.Managers;
using senkovlad.chat.backend.Services;
using senkovlad.chat.data;
using System;

namespace senkovlad.chat.backend
{
    //TEST
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddGrpc();
            services.AddDbContext<AppDbContext>(option =>
            {
                option.UseSqlite("Data Source=chat.db");
            }, ServiceLifetime.Singleton);

            services.AddSingleton<ChatRoomManager>();
            services.AddSingleton<ChatGrpcManager>();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IServiceProvider serviceProvider)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            //app.UseHttpsRedirection();
            app.UseBlazorFrameworkFiles();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseGrpcWeb();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapGrpcService<GreeterService>().EnableGrpcWeb();
                endpoints.MapGrpcService<ChatRoomService>().EnableGrpcWeb();
                endpoints.MapGrpcService<ChatGrpcService>().EnableGrpcWeb();
                endpoints.MapFallbackToFile("index.html");
            });

            serviceProvider.GetService<AppDbContext>().Database.EnsureCreated();
        }
    }
}
