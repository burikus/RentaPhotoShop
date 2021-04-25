using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RentaPhotoDbConnector.Abstractions;
using RentaPhotoDbConnector.EFData;
using RentaPhotoDbConnector.Services;
using RentaPhotoServer.Abstractions;
using RentaPhotoServer.Helpers;
using RentaPhotoServer.Services;

namespace RentaPhotoServer
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext<DataContext>(opt =>
                opt.UseInMemoryDatabase("RentaPhotoDb"));
            services.AddControllers();
            services.AddSwaggerGen();

            var OrderParametersSection = Configuration.GetSection("OrderParameters");
            OrdersSettings config = new OrdersSettings();
            OrderParametersSection.Bind(config);
            services.Configure<OrdersSettings>(OrderParametersSection);

            services.AddSingleton(typeof(IGoods), typeof(Goods));
            services.AddSingleton(typeof(IOrders), typeof(Orders));
            services.AddSingleton(typeof(IDbGoods), typeof(DbGoods));
            services.AddSingleton(typeof(IDbOrders), typeof(DbOrders));
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app.UseSwagger();

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/swagger/v1/swagger.json", "My API V1");
                c.RoutePrefix = string.Empty;
            });

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
