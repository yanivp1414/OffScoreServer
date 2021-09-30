using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OffScoreServerBL.ModelsBL
{
    [Table("ActivityStatus")]
    public partial class ActivityStatus
    {
        public ActivityStatus()
        {
            Accounts = new HashSet<Account>();
            Games = new HashSet<Game>();
            Guesses = new HashSet<Guess>();
        }

        [Key]
        public int StatusId { get; set; }
        [Required]
        [StringLength(255)]
        public string StatusName { get; set; }

        [InverseProperty(nameof(Account.ActivitySatusNavigation))]
        public virtual ICollection<Account> Accounts { get; set; }
        [InverseProperty(nameof(Game.ActivityStatusNavigation))]
        public virtual ICollection<Game> Games { get; set; }
        [InverseProperty(nameof(Guess.ActivityStatusNavigation))]
        public virtual ICollection<Guess> Guesses { get; set; }
    }
}
