using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OffScoreServerBL.Models
{
    [Table("Team")]
    [Index(nameof(TeamName), Name = "team_teamname_unique", IsUnique = true)]
    public partial class Team
    {
        public Team()
        {
            GameTeam1s = new HashSet<Game>();
            GameTeam2s = new HashSet<Game>();
        }

        [Key]
        public int TeamId { get; set; }
        public int GlobalLeagueId { get; set; }
        public int LocalLeagueId { get; set; }
        [Required]
        [StringLength(255)]
        public string TeamName { get; set; }
        public int TeamRank { get; set; }
        public int TeamPoints { get; set; }

        [ForeignKey(nameof(GlobalLeagueId))]
        [InverseProperty(nameof(League.TeamGlobalLeagues))]
        public virtual League GlobalLeague { get; set; }
        [ForeignKey(nameof(LocalLeagueId))]
        [InverseProperty(nameof(League.TeamLocalLeagues))]
        public virtual League LocalLeague { get; set; }
        [InverseProperty(nameof(Game.Team1))]
        public virtual ICollection<Game> GameTeam1s { get; set; }
        [InverseProperty(nameof(Game.Team2))]
        public virtual ICollection<Game> GameTeam2s { get; set; }
    }
}
