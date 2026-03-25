using Analogix_Backend_App.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace Analogix_Backend_App.Infrastructure.Database.Configs
{
    public class PlayerReportConfig : IEntityTypeConfiguration<PlayerReport>
    {
        public void Configure(EntityTypeBuilder<PlayerReport> builder)
        {
            // Table name
            builder.ToTable("PlayerReports");

            // Primary key
            builder.HasKey(pr => pr.Id)
                   .HasName("PK_PlayerReports")
                   .IsClustered();

            // Properties
            builder.Property(pr => pr.Id)
                   .ValueGeneratedOnAdd();

            builder.Property(pr => pr.Description)
                   .IsRequired()
                   .HasMaxLength(2000);

            builder.Property(pr => pr.Reason)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(pr => pr.ReportStatus)
                   .IsRequired()
                   .HasConversion<string>();

            builder.Property(pr => pr.CreatedAt)
                   .IsRequired();

            builder.Property(pr => pr.ReviewNote)
                   .HasMaxLength(2000)
                   .IsRequired(false);

            // Indexes

            builder.HasIndex(r =>r.EventId)
                   .HasDatabaseName("IX_PlayerReports_EventId");
            builder.HasIndex(r=> r.ReportStatus)
                   .HasDatabaseName("IX_PlayerReports_ReportStatus");
            builder.HasIndex(r => r.ReportedPlayerId)
                   .HasDatabaseName("IX_PlayerReports_ReportedPlayerId");
            builder.HasIndex(r => new { r.EventId, r.ReporterId, r.ReportedPlayerId })
                   .HasDatabaseName("IX_PlayerReports_Event_Reporter_ReportedPlayer");

            // Relationships

            // One-to-many: PlayerReport -> Event

            builder.HasOne (pr => pr.Event)
                   .WithMany()
                   .HasForeignKey(pr => pr.EventId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_PlayerReports_Event");

            // One-to-many: PlayerReport -> Reporter (User)

            builder.HasOne(pr => pr.Reporter)
                   .WithMany()
                   .HasForeignKey(pr => pr.ReporterId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_PlayerReports_Reporter");

            // One-to-many: PlayerReport -> ReportedPlayer (User)

            builder.HasOne(pr => pr.ReportedPlayer)
                   .WithMany()
                   .HasForeignKey(pr => pr.ReportedPlayerId)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_PlayerReports_ReportedPlayer");

            // One-to-many: PlayerReport -> Reviewer (User) 
            builder.HasOne(pr => pr.Reviewer)
                   .WithMany()
                   .HasForeignKey(pr => pr.ReviewerId)
                   .OnDelete(DeleteBehavior.SetNull) 
                   .HasConstraintName("FK_PlayerReports_Reviewer");
        }
    }
}
