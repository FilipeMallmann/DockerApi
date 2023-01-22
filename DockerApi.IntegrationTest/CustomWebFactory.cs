using DockerApi.Infra;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerApi.IntegrationTest
{
    public class CustomWebFactory<TSetup> : WebApplicationFactory<TSetup> where TSetup : class
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(service =>
            {
                var descriptor = service.SingleOrDefault(des => des.ServiceType.Equals(typeof(DbContext)));
                service.Remove(descriptor);

                service.AddDbContext<DockerApiDbContext>(opt =>
                {
                    opt.UseInMemoryDatabase("DBForTesting");
                });
                var sp = service.BuildServiceProvider();
                using(var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    var db = scopedServices.GetRequiredService<DockerApiDbContext>();
                    var logger = scopedServices.GetRequiredService<ILogger<CustomWebFactory<TSetup>>>();
                    db.Database.EnsureCreated();
                }
            });
        }
    }
}
