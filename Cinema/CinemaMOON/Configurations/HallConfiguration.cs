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
	public class HallConfiguration : IEntityTypeConfiguration<Hall>
	{
		public void Configure(EntityTypeBuilder<Hall> builder)
		{
			builder.ToTable("Halls");

			builder.HasKey(h => h.Id);
			builder.Property(h => h.Name).IsRequired().HasMaxLength(64);
			builder.Property(h => h.Capacity).IsRequired();

			builder.HasMany(h => h.Schedules)
				   .WithOne(s => s.Hall)
				   .HasForeignKey(s => s.HallId)
				   .OnDelete(DeleteBehavior.Restrict);
		}
	}
}
