using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Configs
{
    public class EventConfig : IEntityTypeConfiguration<Event>
    {
        public void Configure(EntityTypeBuilder<Event> builder)
        {
            // Table name
            builder.ToTable("Events");

            // Primary key
            builder.HasKey(e => e.Id)
                   .HasName("PK_Events")
                   .IsClustered();

            // Properties
            builder.Property(e => e.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(e => e.Title)
                   .IsRequired()
                   .HasMaxLength(120);

            builder.Property(e => e.Description)
                   .HasMaxLength(4_000)
                   .IsRequired(false);

            builder.Property(e => e.Location)
                   .IsRequired()
                   .HasMaxLength(120);

            builder.Property(e => e.StartDate)
                   .IsRequired()
                   .HasColumnName("StartDate");

            builder.Property(e => e.EndDate)
                   .HasColumnName("EndDate");

            builder.Property(e => e.MaxParticipants)
                   .IsRequired()
                   .HasColumnName("MaxParticipants");

            builder.Property(e => e.CreatorId)
                   .IsRequired()
                   .HasColumnName("CreatorId");

            // Indexes

            // Allow efficient querying of events by their creator
            builder.HasIndex(e => e.CreatorId)
                   .HasDatabaseName("IX_Events_CreatorId");

            // Allows efficient querying of events by their start date
            builder.HasIndex(e => e.StartDate)
                   .HasDatabaseName("IX_Events_StartDate");

            //Allows efficient querying of events by their location
            builder.HasIndex(e => e.Location)
                   .HasDatabaseName("IX_Events_Location");

            // Relationships

            builder.HasOne(e => e.Creator)
                   .WithMany(u => u.CreatedEvents)
                   .HasForeignKey(e => e.CreatorId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_Events_Users_CreatorId");


        }
    }
}
