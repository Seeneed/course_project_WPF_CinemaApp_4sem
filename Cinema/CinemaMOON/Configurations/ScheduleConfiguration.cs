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
	public class ScheduleConfiguration : IEntityTypeConfiguration<Schedule>
	{
		public void Configure(EntityTypeBuilder<Schedule> builder)
		{
			builder.ToTable("Schedules");

			builder.HasKey(s => s.Id);
			builder.Property(s => s.ShowTime).IsRequired();
			builder.Property(s => s.AvailableSeats).IsRequired();

			builder.HasOne(s => s.Movie)
				   .WithMany(m => m.Schedules)
				   .HasForeignKey(s => s.MovieId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(s => s.Hall)
				   .WithMany(h => h.Schedules)
				   .HasForeignKey(s => s.HallId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.HasMany(s => s.Orders)
				   .WithOne(o => o.Schedule)
				   .HasForeignKey(o => o.ScheduleId)
				   .OnDelete(DeleteBehavior.Restrict);
		}
	}
}
