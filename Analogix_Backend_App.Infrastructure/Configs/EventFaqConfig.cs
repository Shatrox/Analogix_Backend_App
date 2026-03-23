using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Configs
{
    public class EventFaqConfig : IEntityTypeConfiguration<EventFaq>
    {
        public void Configure(EntityTypeBuilder<EventFaq> builder)
        {
            // Table name
            builder.ToTable("EventFaqs");

            // Primary key
            builder.HasKey(ef => ef.Id);

            // Properties
            builder.Property(ef => ef.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(ef => ef.Question)
                   .IsRequired()
                   .HasMaxLength(1000);

            builder.Property(ef => ef.AskedAtUtc)
                   .IsRequired();

            builder.Property(ef => ef.Answer)
                   .IsRequired(false)
                   .HasMaxLength(1000);

            builder.Property(ef => ef.AnsweredAtUtc)
                   .IsRequired(false);

            // Indexes
            builder.HasIndex(ef => ef.EventId)
                   .HasDatabaseName("IX_EventFaqs_EventId"); 
            
            builder.HasIndex(ef => ef.AuthorUserId)
                   .HasDatabaseName("IX_EventFaqs_AuthorUserId");

            builder.HasIndex(ef => ef.AnsweredUserId)
                   .HasDatabaseName("IX_EventFaqs_AnsweredUserId");

            // Relationships

            // Many-to-one with Event
            builder.HasOne(ef => ef.Event)
                   .WithMany(e => e.EventFaq)
                   .HasForeignKey(ef => ef.EventId)
                   .OnDelete(DeleteBehavior.Cascade)
                   .HasConstraintName("FK_EventFaqs_Events_EventId");


            // Many-to-one with User (Author)
            builder.HasOne(ef => ef.AuthorUser)
                   .WithMany(u => u.AskedFaqs)
                   .HasForeignKey(ef => ef.AuthorUserId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_EventFaqs_Users_AuthorUserId");

            // Many-to-one with User (Event Owner)
            builder.HasOne(ef => ef.AnsweredUser)
                   .WithMany(u => u.AnsweredFaqs)
                   .HasForeignKey(ef => ef.AnsweredUserId)
                   .OnDelete(DeleteBehavior.SetNull)
                   .HasConstraintName("FK_EventFaqs_Users_AnsweredUserId");
        }
    }
}
