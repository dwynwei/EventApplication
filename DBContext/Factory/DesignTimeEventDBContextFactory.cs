using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Factory
{
    public class DesignTimeEventDBContextFactory : IDesignTimeDbContextFactory<EventDBContext>
    {
        public EventDBContext CreateDbContext(string[] args)
        {
            IConfigurationRoot configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json")
                .Build();
            var optionsBuilder = new DbContextOptionsBuilder<EventDBContext>();
            optionsBuilder.UseSqlServer(configuration.GetConnectionString("EtkinlikYonetimDb"));

            return new EventDBContext(optionsBuilder.Options);
        }
    }
}
