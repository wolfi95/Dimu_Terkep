using DIMU.DAL.Entities.Authentication;
using DIMU.DAL.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL
{
    public class DimuContext : IdentityDbContext<AdminUser>
    {
        private readonly IConfiguration _configuration;
        public DbSet<Intezmeny> Intezmenyek { get; set; }
        public DbSet<Esemeny> Esemenyek { get; set; }
        public DbSet<IntezmenyHelyszin> IntezmenyHelyszinek { get; set; }
        public DbSet<IntezmenyVezeto> IntezmenyVezetok { get; set; }

        public DimuContext(DbContextOptions<DimuContext> options, IConfiguration configuration) : base(options)
        {
            _configuration = configuration;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            var connectionString = "";
            if (Environment.GetEnvironmentVariable("DATABASE_URL") == null)
            {
                connectionString = _configuration.GetConnectionString(nameof(DimuContext));
            }
            else
            {
                connectionString = Environment.GetEnvironmentVariable("DATABASE_URL");
            }
            var builder = new PostgreSqlConnectionStringBuilder(connectionString)
            {
                Pooling = true,
                TrustServerCertificate = true,
                SslMode = SslMode.Require
            };
            var connectionUrl = builder.ConnectionString;
            optionsBuilder.UseNpgsql(connectionUrl, builder =>
            {
                builder.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
            });
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //one-to-many relations (lehet h magatol is kitalalja mivel ugyanaz a neve de igy a biztos)
            modelBuilder.Entity<Intezmeny>()
                .HasMany<IntezmenyVezeto>(intezmeny => intezmeny.IntezmenyVezetok)
                .WithOne(iv => iv.Intezmeny);

            modelBuilder.Entity<Intezmeny>()
                .HasMany<IntezmenyHelyszin>(intezmeny => intezmeny.IntezmenyHelyszinek)
                .WithOne(muv => muv.Intezmeny);

            modelBuilder.Entity<Intezmeny>()
                .HasMany<Esemeny>(intezmeny => intezmeny.Esemenyek)
                .WithOne(muv => muv.Intezmeny);
        }
    }
}
