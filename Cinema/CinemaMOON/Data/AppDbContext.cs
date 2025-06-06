using Microsoft.EntityFrameworkCore;
using CinemaMOON.Configurations;
using System;
using CinemaMOON.Models;

namespace CinemaMOON.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Movie> Movies { get; set; }
        public DbSet<Hall> Halls { get; set; }
        public DbSet<Schedule> Schedules { get; set; }
        public DbSet<Order> Orders { get; set; }

        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfiguration());
            modelBuilder.ApplyConfiguration(new MovieConfiguration());
            modelBuilder.ApplyConfiguration(new HallConfiguration());
            modelBuilder.ApplyConfiguration(new ScheduleConfiguration());
            modelBuilder.ApplyConfiguration(new OrderConfiguration());

            modelBuilder.Entity<Hall>().HasData(
                new Hall
                {
                    Id = Guid.Parse("DA291A5E-71C2-46A6-965C-02D3E2C59431"),
                    Name = "HallPage_SmallHallName", 
                    Capacity = 45,
                    Type = "small"
                },
                new Hall
                {
                    Id = Guid.Parse("B68871D6-35E5-432C-B0E0-3178586333E9"),
                    Name = "HallPage_MediumHallName",
                    Capacity = 60,
                    Type = "medium"
                },
                new Hall
                {
                    Id = Guid.Parse("14EC82A0-460F-4BE2-9BC9-1C56E949A17E"),
                    Name = "HallPage_LargeHallName",
                    Capacity = 70,
                    Type = "large"
                }
            );
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer("Server=(localdb)\\MSSQLLocalDB;Database=CinemaMOON;Trusted_Connection=True;TrustServerCertificate=True;");
            }
        }
    }
}