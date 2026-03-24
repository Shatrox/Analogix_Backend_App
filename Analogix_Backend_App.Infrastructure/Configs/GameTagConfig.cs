using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Configs
{
    public class GameTagConfig : IEntityTypeConfiguration<GameTag>
    {
        public void Configure(EntityTypeBuilder<GameTag> builder)
        {
            // Table name
            builder.ToTable("GameTags");

            // Primary key
            builder.HasKey(gt =>gt.Id)
                   .HasName("PK_GameTags");

            // Properties

            builder.Property(gt => gt.Id)
                   .HasColumnName("Id")
                   .ValueGeneratedOnAdd();

            builder.Property(gt => gt.Name)
                   .HasColumnName("Name")
                   .IsRequired()
                   .HasMaxLength(50);
            
            builder.Property(gt => gt.NormalizedName)
                   .HasColumnName("NormalizedName")
                   .IsRequired()
                   .HasMaxLength(50);

            // Indexes

            //allows search by normalized name
            builder.HasIndex(gt => gt.NormalizedName)
                   .HasDatabaseName("IX_GameTags_NormalizedName")
                   .IsUnique();

           
        }
    }
}
