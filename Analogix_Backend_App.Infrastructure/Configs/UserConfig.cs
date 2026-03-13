using Analogix_Backend_App.Domain.Enums;
using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Configs
{
    public class UserConfig : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Tables
            builder.ToTable("Users");

            // Primary Key
            builder.HasKey(u => u.Id)
                   .HasName("PK_Users")
                   .IsClustered(true);

            // Properties
            builder.Property(u => u.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(u => u.Username)
                   .HasColumnName("Username")
                   .HasMaxLength(50)
                   .IsRequired()
                   .IsUnicode();

            builder.Property(u => u.Email)
                   .HasColumnName("Email")
                   .HasMaxLength(100)
                   .IsRequired()
                   .IsUnicode(false);

            builder.Property(u => u.Password)
                   .HasColumnName("Password")
                   .HasMaxLength(200)
                   .IsRequired();

            builder.Property(u => u.Role)
                   .HasColumnName("Role")
                   .HasDefaultValue(UserRoles.User) // Set the default value for the Role property to User
                   .HasConversion<string>() // Store the enum as a string in the database for better readability
                   .HasSentinel(0) // Set the sentinel value for the enum to prevent issues with default values
                   .HasMaxLength(20)
                   .IsRequired();

            // Indexes

            // ↓ Prevent duplicate emails
            builder.HasIndex(u => u.Email)
                   .HasDatabaseName("IX_Users_Email")
                   .IsUnique();

            // ↓ Prevent duplicate usernames
            builder.HasIndex(u => u.Username)
                   .HasDatabaseName("IX_Users_Username")
                   .IsUnique();

        }
    }
}
