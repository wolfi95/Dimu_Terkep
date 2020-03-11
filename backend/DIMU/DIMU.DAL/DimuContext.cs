using DIMU.DAL.Entities.Authentication;
using DIMU.DAL.Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace DIMU.DAL
{
    public class DimuContext : IdentityDbContext<AdminUser>
    {
        public DbSet<Intezmeny> Intezmenyek { get; set; }
        public DbSet<Muvesz> Muveszek { get; set; }

        public DimuContext(DbContextOptions<DimuContext> options) : base(options)
        {
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
           
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //one-to-many relations (lehet h magatol is kitalalja mivel ugyanaz a neve de igy a biztos)
            modelBuilder.Entity<Intezmeny>()
                .HasMany<IntezmenyVezeto>(intezmeny => intezmeny.IntezmenyVezetok)
                .WithOne(iv => iv.Intezmeny);

            modelBuilder.Entity<Intezmeny>()
                .HasMany<Muvesz>(intezmeny => intezmeny.Muveszek)
                .WithOne(muv => muv.Intezmeny);
        }
    }
}
