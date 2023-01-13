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
        protected DockerApiDbContext() : base()
        {

        }
        
        DbSet<Customer> Customers { get; set; }
    }
}
