using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OffScoreServerBL.ModelsBL
{
    public partial class OffScoreContext : DbContext
    {
        public OffScoreContext()
        {
        }

        public OffScoreContext(DbContextOptions<OffScoreContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Account> Accounts { get; set; }
        public virtual DbSet<ActivityStatus> ActivityStatuses { get; set; }
        public virtual DbSet<Game> Games { get; set; }
        public virtual DbSet<Guess> Guesses { get; set; }
        public virtual DbSet<League> Leagues { get; set; }
        public virtual DbSet<Team> Teams { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
                optionsBuilder.UseSqlServer("Server = localhost\\SQLEXPRESS; Database=OffScore; Trusted_Connection=true");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasAnnotation("Relational:Collation", "SQL_Latin1_General_CP1_CI_AS");

            modelBuilder.Entity<Account>(entity =>
            {
                entity.Property(e => e.AccountId).ValueGeneratedNever();

                entity.HasOne(d => d.ActivitySatusNavigation)
                    .WithMany(p => p.Accounts)
                    .HasForeignKey(d => d.ActivitySatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("account_activitysatus_foreign");
            });

            modelBuilder.Entity<ActivityStatus>(entity =>
            {
                entity.HasKey(e => e.StatusId)
                    .HasName("activitystatus_statusid_primary");

                entity.Property(e => e.StatusId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.GameId).ValueGeneratedNever();

                entity.HasOne(d => d.ActivityStatusNavigation)
                    .WithMany(p => p.Games)
                    .HasForeignKey(d => d.ActivityStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("game_activitystatus_foreign");

                entity.HasOne(d => d.Team1)
                    .WithMany(p => p.GameTeam1s)
                    .HasForeignKey(d => d.Team1Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("game_team1id_foreign");

                entity.HasOne(d => d.Team2)
                    .WithMany(p => p.GameTeam2s)
                    .HasForeignKey(d => d.Team2Id)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("game_team2id_foreign");
            });

            modelBuilder.Entity<Guess>(entity =>
            {
                entity.Property(e => e.GuessId).ValueGeneratedNever();

                entity.HasOne(d => d.Account)
                    .WithMany(p => p.Guesses)
                    .HasForeignKey(d => d.AccountId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("guess_accountid_foreign");

                entity.HasOne(d => d.ActivityStatusNavigation)
                    .WithMany(p => p.Guesses)
                    .HasForeignKey(d => d.ActivityStatus)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("guess_activitystatus_foreign");

                entity.HasOne(d => d.Game)
                    .WithMany(p => p.Guesses)
                    .HasForeignKey(d => d.GameId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("guess_gameid_foreign");
            });

            modelBuilder.Entity<League>(entity =>
            {
                entity.Property(e => e.LeagueId).ValueGeneratedNever();
            });

            modelBuilder.Entity<Team>(entity =>
            {
                entity.Property(e => e.TeamId).ValueGeneratedNever();

                entity.HasOne(d => d.League)
                    .WithMany(p => p.Teams)
                    .HasForeignKey(d => d.LeagueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("team_leagueid_foreign");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
