using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OIG_Test.Models;

namespace OIG_Test.DBInfra
{
    public class DataContext : DbContext
    {
        public DbSet<Research> Research { get; set; }

        public DataContext()
        {
        }
        public DataContext(DbContextOptions
            <DataContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=.\sqlexpress;Initial Catalog=OigTestDB;Integrated Security=True");
        }
    }
}
