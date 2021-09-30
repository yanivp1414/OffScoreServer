using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OffScoreServerBL.ModelsBL
{
    [Table("League")]
    [Index(nameof(Country), Name = "league_country_index")]
    [Index(nameof(LegueName), Name = "league_leguename_unique", IsUnique = true)]
    public partial class League
    {
        public League()
        {
            Teams = new HashSet<Team>();
        }

        [Key]
        public int LeagueId { get; set; }
        [Required]
        [StringLength(255)]
        public string Country { get; set; }
        [Required]
        [StringLength(255)]
        public string LegueName { get; set; }

        [InverseProperty(nameof(Team.League))]
        public virtual ICollection<Team> Teams { get; set; }
    }
}
