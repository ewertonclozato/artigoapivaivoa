using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GeradorCartaoAPI.Model
{
    public class AdmDbContext : DbContext
    {
        public AdmDbContext(DbContextOptions options) : base(options)
        {

        }

        public DbSet<Cliente> Cliente { get; set; }
        public DbSet<Cartao> Cartao { get; set; }
    }

}
