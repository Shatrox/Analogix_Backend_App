using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Configs
{
    public class RatingConfig : IEntityTypeConfiguration<Rating> 
    { 
        public void Configure(EntityTypeBuilder<Rating> builder) 
        {
            //Table name
            builder.ToTable("Ratings");

            //Primary key
            builder.HasKey(r => r.Id)
                   .IsClustered()
                   .HasName("PK_Ratings_Id");

            // Properties
            builder.Property(r => r.Id)
                   .HasColumnName("id")
                   .ValueGeneratedOnAdd();

            builder.Property(ev => ev.EventId)
                   .IsRequired();

            builder.Property(r => r.RaterUserId)
                   .IsRequired();   

            builder.Property(r => r.TargetUserId)
                   .IsRequired(false); // TargetUserId is optional because it can be null when TargetType is Event

            builder.Property(r => r.TargetType)
                   .IsRequired()
                   .HasConversion<string>(); // Store enum as string for better readability
            
            builder.Property(r => r.Score)
                   .IsRequired();

            builder.Property(r => r.CreatedAt)
                   .IsRequired();

            // Indexes

            // Allows efficient querying of ratings for a specific target user (when TargetType is User)
            builder.HasIndex(r => r.TargetUserId) 
                   .HasDatabaseName("IX_Ratings_TargetUserId");

            // Composite index to efficiently query ratings for a specific event and rater, filtered by TargetType = Event
            builder.HasIndex(r => new { r.EventId, r.RaterUserId, r.TargetType})
                   .HasDatabaseName("IX_Ratings_EventId_RaterUserId_TargetType")
                   .HasFilter("[TargetUserId] IS NULL"); 

            // Composite index to efficiently query ratings for a specific event and rater, filtered by TargetType = User   
            builder.HasIndex(r => new { r.EventId, r.RaterUserId, r.TargetType, r.TargetUserId})
                   .HasDatabaseName("IX_Ratings_EventId_RaterUserId_TargetType_TargetUserId")
                   .HasFilter("[TargetUserId] IS NOT NULL");

            // Relationships

            // Many-to-one relationship between Rating and Event
            builder.HasOne(r => r.Event)
                   .WithMany(e => e.Ratings)
                   .HasForeignKey(r => r.EventId)
                   .OnDelete(DeleteBehavior.Restrict) // Prevent cascade delete to avoid accidentally deleting ratings when an event is deleted.
                   .HasConstraintName("FK_Ratings_Events_EventId");

            // Many-to-one relationship between Rating and User (RaterUser)
            builder.HasOne(r => r.RaterUser)
                   .WithMany(u => u.GivenRatings)
                   .HasForeignKey(r => r.RaterUserId)
                   .OnDelete(DeleteBehavior.Restrict) // Prevent cascade delete to avoid accidentally deleting ratings given when a user is deleted.
                   .HasConstraintName("FK_Ratings_Users_RaterUserId");

            // Many-to-one relationship between Rating and User (TargetUser)
            builder.HasOne(r => r.TargetUser)
                   .WithMany(u => u.ReceivedRatings)
                   .HasForeignKey(r =>r.TargetUserId)
                   .OnDelete(DeleteBehavior.Restrict) // Prevent cascade delete to avoid accidentally deleting ratings received when a user is deleted.
                   .HasConstraintName("FK_Ratings_Users_TargetUserId"); 

        }


    } 
    
    
}
