using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OffScoreServerBL.Models
{
    [Table("Guess")]
    public partial class Guess
    {
        [Key]
        public int GuessId { get; set; }
        public int AccountId { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime GuessingTime { get; set; }
        public int Team1Guess { get; set; }
        public int Team2Guess { get; set; }
        public int GameId { get; set; }
        public int ActivityStatus { get; set; }

        [ForeignKey(nameof(AccountId))]
        [InverseProperty("Guesses")]
        public virtual Account Account { get; set; }
        [ForeignKey(nameof(ActivityStatus))]
        [InverseProperty("Guesses")]
        public virtual ActivityStatus ActivityStatusNavigation { get; set; }
        [ForeignKey(nameof(GameId))]
        [InverseProperty("Guesses")]
        public virtual Game Game { get; set; }
    }
}
