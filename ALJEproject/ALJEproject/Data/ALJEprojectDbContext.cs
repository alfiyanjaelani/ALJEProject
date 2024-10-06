using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ALJEproject.Models;
using Microsoft.EntityFrameworkCore;


namespace ALJEproject.Data
{
    public class ALJEprojectDbContext : DbContext
    {
        public ALJEprojectDbContext(DbContextOptions<ALJEprojectDbContext> options) : base(options)
        {
        }
        public DbSet<User> Users { get; set; }  // Pastikan Anda memiliki model User

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            // Optional: Konfigurasi entitas jika dibutuhkan
            modelBuilder.Entity<User>().ToTable("Users");  // Menentukan nama tabel di database
        }

    }
}
