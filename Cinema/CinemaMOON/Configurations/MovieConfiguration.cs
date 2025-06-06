using CinemaMOON.Models;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CinemaMOON.Configurations
{
	public class MovieConfiguration : IEntityTypeConfiguration<Movie>
	{
		public void Configure(EntityTypeBuilder<Movie> builder)
		{
			builder.ToTable("Movies");

			builder.HasKey(m => m.Id);
			builder.Property(m => m.Title).IsRequired().HasMaxLength(128);
			builder.Property(m => m.Director).IsRequired().HasMaxLength(128);
			builder.Property(m => m.Description).HasMaxLength(1024);
			builder.Property(m => m.Genre).IsRequired().HasMaxLength(64);
			builder.Property(m => m.Duration).IsRequired();
			builder.Property(m => m.Rating).HasMaxLength(16);
			builder.Property(m => m.Photo).IsRequired();
			builder.Property(m => m.IsActive).IsRequired();

			builder.HasMany(m => m.Schedules)
				   .WithOne(s => s.Movie)
				   .HasForeignKey(s => s.MovieId)
				   .OnDelete(DeleteBehavior.Restrict);
		}
	}
}
