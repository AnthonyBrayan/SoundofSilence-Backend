﻿using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class ServiceContext : DbContext
    {
        public ServiceContext(DbContextOptions<ServiceContext> options) : base(options) { }
        public DbSet<Rol> Rol { get; set; }
        public DbSet<Users> Users { get; set; }
        public DbSet<AudioFiles> AudioFiles { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<UserAudio> UserAudio { get; set; }



        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Rol>(entity =>
            {
                entity.ToTable("Rol");
            });

            builder.Entity<Users>(entity =>
            {
                entity.ToTable("Users");
            });

            builder.Entity<AudioFiles>(entity =>
            {
                entity.ToTable("AudioFiles");
            });

            builder.Entity<Category>(entity =>
            {
                entity.ToTable("Category");
            });

            builder.Entity<UserAudio>(entity =>
            {
                entity.ToTable("UserAudio");
            });
        }

    }

    public class ServiceContextFactory : IDesignTimeDbContextFactory<ServiceContext>
    {
        public ServiceContext CreateDbContext(string[] args)
        {
            var builder = new ConfigurationBuilder()
                   .SetBasePath(Directory.GetCurrentDirectory())
                   .AddJsonFile("appsettings.json", false, true);
            var config = builder.Build();
            var connectionString = config.GetConnectionString("ServiceContext");
            var optionsBuilder = new DbContextOptionsBuilder<ServiceContext>();
            optionsBuilder.UseSqlServer(config.GetConnectionString("ServiceContext"));

            return new ServiceContext(optionsBuilder.Options);
        }
    }

}


