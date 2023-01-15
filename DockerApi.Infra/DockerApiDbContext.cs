using DockerApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DockerApi.Infra
{
    public class DockerApiDbContext : DbContext
    {
        public DockerApiDbContext() 
        {

        }
        public DockerApiDbContext(DbContextOptions<DockerApiDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("DataSource=Api.db;Cache=Shared");
        }

       public DbSet<Customer> Customers { get; set; }
    }
}
