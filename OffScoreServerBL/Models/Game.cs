using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OffScoreServerBL.ModelsBL
{
    [Table("Game")]
    public partial class Game
    {
        public Game()
        {
            Guesses = new HashSet<Guess>();
        }

        [Key]
        public int GameId { get; set; }
        public int Team1Id { get; set; }
        public int Team2Id { get; set; }
        [Required]
        [StringLength(255)]
        public string FinalScore { get; set; }
        public int ActivityStatus { get; set; }

        [ForeignKey(nameof(ActivityStatus))]
        [InverseProperty("Games")]
        public virtual ActivityStatus ActivityStatusNavigation { get; set; }
        [ForeignKey(nameof(Team1Id))]
        [InverseProperty(nameof(Team.GameTeam1s))]
        public virtual Team Team1 { get; set; }
        [ForeignKey(nameof(Team2Id))]
        [InverseProperty(nameof(Team.GameTeam2s))]
        public virtual Team Team2 { get; set; }
        [InverseProperty(nameof(Guess.Game))]
        public virtual ICollection<Guess> Guesses { get; set; }
    }
}
