using Hangfire;
using QueueOrderHub.Apis.Extinsions;
using QueueOrderHub.Apis.MiddleWares;
using QueueOrderHub.Core.Application;
using QueueOrderHub.Infrastructure;
using QueueOrderHub.Shared;

namespace QueueOrderHub.Apis
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.




            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.RegesteredPresestantLayer();

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddInfrastructureServices(builder.Configuration);
            builder.Services.AddApplicationServices();
            builder.Services.AddSharedDependency(builder.Configuration);

            var app = builder.Build();
            app.UseMiddleware<ExeptionHandlerMiddleware>();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseStatusCodePagesWithReExecute("/Errors/{0}");
            app.UseStaticFiles();

            app.MapHangfireDashboard("/Dashbord");



            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
