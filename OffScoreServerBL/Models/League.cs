using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OffScoreServerBL.Models
{
    [Table("League")]
    [Index(nameof(Country), Name = "league_country_index")]
    [Index(nameof(LeagueName), Name = "league_leguename_unique", IsUnique = true)]
    public partial class League
    {
        public League()
        {
            TeamGlobalLeagues = new HashSet<Team>();
            TeamLocalLeagues = new HashSet<Team>();
        }

        [Key]
        public int LeagueId { get; set; }
        [Required]
        [StringLength(255)]
        public string Country { get; set; }
        [Required]
        [StringLength(255)]
        public string LeagueName { get; set; }

        [InverseProperty(nameof(Team.GlobalLeague))]
        public virtual ICollection<Team> TeamGlobalLeagues { get; set; }
        [InverseProperty(nameof(Team.LocalLeague))]
        public virtual ICollection<Team> TeamLocalLeagues { get; set; }
    }
}
