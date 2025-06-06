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
	public class OrderConfiguration : IEntityTypeConfiguration<Order>
	{
		public void Configure(EntityTypeBuilder<Order> builder)
		{
			builder.ToTable("Orders");

			builder.HasKey(o => o.Id);
			builder.Property(o => o.Seats).IsRequired();
			builder.Property(o => o.TotalPrice).IsRequired().HasColumnType("decimal(18,2)");
			builder.Property(o => o.OrderStatus).IsRequired().HasMaxLength(32);
			builder.Property(o => o.BookingTimestamp).IsRequired();

			builder.HasOne(o => o.User)
				   .WithMany(u => u.Orders)
				   .HasForeignKey(o => o.UserId)
				   .OnDelete(DeleteBehavior.Restrict);

			builder.HasOne(o => o.Schedule)
				   .WithMany(s => s.Orders)
				   .HasForeignKey(o => o.ScheduleId)
				   .OnDelete(DeleteBehavior.Restrict);
		}
	}
}
