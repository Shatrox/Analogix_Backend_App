using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Configs
{
    public class PlayerProfileConfig : IEntityTypeConfiguration<PlayerProfile>  
    {
        public void Configure(EntityTypeBuilder<PlayerProfile> builder) 
        {
            // Tables
            builder.ToTable("PlayerProfiles");

            // Primary Key
            builder.HasKey(pp => pp.Id)
                   .HasName("PK_PlayerProfiles")
                   .IsClustered(true);

            //  Properties
            builder.Property(pp => pp.Id)
                   .ValueGeneratedOnAdd()
                   .IsRequired();

            builder.Property(pp => pp.Biography)
                   .HasMaxLength(5_000)
                   .IsRequired(false);

            builder.Property(pp => pp.FavoriteGames)
                   .HasMaxLength(1000)
                   .IsUnicode()
                   .IsRequired();

            builder.Property(pp => pp.MasteryLevel)
                   .HasConversion<string>()
                   .HasMaxLength(50)
                   .IsRequired();

            builder.Property(pp => pp.UserId)
                   .HasColumnName("UserId")
                   .IsRequired();
            
           // Indexes
            builder.HasIndex(pp => pp.UserId)
                   .HasDatabaseName("IX_PlayerProfiles_UserId")
                   .IsUnique();

            // Relationships 1:1 with User
            builder.HasOne(pp => pp.User)
                   .WithOne(u => u.PlayerProfile)
                   .HasForeignKey<PlayerProfile>(pp => pp.UserId)
                   .HasConstraintName("FK_PlayerProfiles_Users_UserId")
                   .OnDelete(DeleteBehavior.Cascade); // When a User is deleted, the associated PlayerProfile will also be deleted (cascading delete).

            // Relationships with GameTags (Many-to-Many)

            builder.HasMany(e => e.FavoriteGameTags)
                   .WithMany(gt => gt.PlayerProfiles)
                   .UsingEntity<Dictionary<string, object>>(
                        "PlayerProfileGameTag",
                        jr => jr.HasOne<GameTag>() // each entry in the join table has one GameTag
                                .WithMany()
                                .HasForeignKey("GameTagId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .HasConstraintName("FK_ProfileGameTags_GameTags_GameTagId"),
                        jl => jl.HasOne<PlayerProfile>() // each entry in the join table has one PlayerProfile
                                .WithMany()
                                .HasForeignKey("PlayerProfileId")
                                .OnDelete(DeleteBehavior.Cascade)
                                .HasConstraintName("FK_ProfileGameTags_PlayerProfiles_PlayerProfileId"),
                        join =>
                        {
                            join.ToTable("PlayerProfileGameTags");
                            join.HasKey("PlayerProfileId", "GameTagId").HasName("PK_PlayerProfileGameTags");
                        });

        }
    }
}
