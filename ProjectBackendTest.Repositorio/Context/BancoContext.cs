using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using ProjectBackendTest.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ProjectBackendTest.Repository.Context
{
    public class BancoContext : DbContext
    {
        public BancoContext() { }

        protected override void OnConfiguring(DbContextOptionsBuilder dbContextOptionsBuilder)
        {
            if (!dbContextOptionsBuilder.IsConfigured)
            {
                IConfigurationRoot configuration = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json")
                   .Build();
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                dbContextOptionsBuilder.UseSqlServer(connectionString);
            }
        }
        public BancoContext(DbContextOptions<BancoContext> options)
                : base(options) { }

        public virtual DbSet<Usuario> Usuarios { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Usuario>()
                .HasKey(t => t.Id);
        }
    }
}
