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
	public class UserConfiguration : IEntityTypeConfiguration<User>
	{
		public void Configure(EntityTypeBuilder<User> builder)
		{
			builder.ToTable("Users");

			builder.HasKey(u => u.Id);
			builder.Property(u => u.Name).IsRequired().HasMaxLength(64);
			builder.Property(u => u.Surname).IsRequired().HasMaxLength(64);
			builder.Property(u => u.Email).IsRequired().HasMaxLength(128);

			builder.Property(u => u.PasswordHash)
				   .IsRequired()
				   .HasMaxLength(128);

			builder.Property(u => u.Salt)
				   .IsRequired()
				   .HasMaxLength(128);

			builder.Property(u => u.IsAdmin).IsRequired();

			builder.HasMany(u => u.Orders)
				   .WithOne(o => o.User)
				   .HasForeignKey(o => o.UserId)
				   .OnDelete(DeleteBehavior.Restrict);
		}
	}
}
