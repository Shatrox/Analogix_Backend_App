using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Configs
{
    public class EventSubscriptionConfig : IEntityTypeConfiguration<EventSubscription>
    {
        public void Configure(EntityTypeBuilder<EventSubscription> builder)
        {
            // Table name
            builder.ToTable("EventSubscriptions");

            // Primary key
            builder.HasKey(es => es.Id)
                   .HasName("PK_EventSubscriptions")
                   .IsClustered();


            //Properties
            builder.Property(es => es.Id)
                   .ValueGeneratedOnAdd()
                   .HasColumnName("Id");
                   

            builder.Property(es => es.Status)
                   .IsRequired()
                   .HasConversion<string>()
                   .HasColumnName("Status");

            builder.Property(es => es.CreatedAt)
                   .IsRequired()
                   .HasColumnName("CreatedAt");

            builder.Property(es => es.ResponseAt)
                   .HasColumnName("ResponseAt");

            // Indexes

            // Unique index on EventId and UserId to prevent duplicate subscriptions + improve query performance when looking up a user's subscription to an event
            builder.HasIndex(es  => new { es.EventId, es.UserId})
                   .IsUnique()
                   .HasDatabaseName("IX_EventSubscriptions_EventId_UserId");

            // Allows Status-based queries to be more efficient
            builder.HasIndex(es => es.Status)
                   .HasDatabaseName("IX_EventSubscriptions_Status");


            // Relationships

            // Each subscription is for one event, but an event can have many subscriptions
            builder.HasOne(es => es.Event)
                   .WithMany(e => e.Subscriptions)
                   .HasForeignKey(es => es.EventId)
                   .HasConstraintName("FK_EventSubscriptions_Events_EventId")
                   .OnDelete(DeleteBehavior.Cascade);


            // Each subscription is for one user, but a user can have many subscriptions
            builder.HasOne(es => es.User)
                   .WithMany(u => u.EventSubscriptions)
                   .HasForeignKey(es => es.UserId)
                   .HasConstraintName("FK_EventSubscriptions_Users_UserId")
                   .OnDelete(DeleteBehavior.Cascade);



        }
    }
}
