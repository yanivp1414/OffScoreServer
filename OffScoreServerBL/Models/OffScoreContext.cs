using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

#nullable disable

namespace OffScoreServerBL.Models
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
                entity.Property(e => e.ActivitySatus).HasDefaultValueSql("((1))");

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
            });

            modelBuilder.Entity<Game>(entity =>
            {
                entity.Property(e => e.ActivityStatus).HasDefaultValueSql("((1))");

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
                entity.Property(e => e.ActivityStatus).HasDefaultValueSql("((1))");

                entity.Property(e => e.GuessingTime).HasDefaultValueSql("(getdate())");

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

            modelBuilder.Entity<Team>(entity =>
            {
                entity.HasOne(d => d.GlobalLeague)
                    .WithMany(p => p.TeamGlobalLeagues)
                    .HasForeignKey(d => d.GlobalLeagueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("team_globalleagueid_foreign");

                entity.HasOne(d => d.LocalLeague)
                    .WithMany(p => p.TeamLocalLeagues)
                    .HasForeignKey(d => d.LocalLeagueId)
                    .OnDelete(DeleteBehavior.ClientSetNull)
                    .HasConstraintName("team_localleagueid_foreign");
            });

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}
