using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

#nullable disable

namespace OffScoreServerBL.Models
{
    [Table("Account")]
    [Index(nameof(Birthday), Name = "account_birthday_index")]
    [Index(nameof(Email), Name = "account_email_unique", IsUnique = true)]
    [Index(nameof(IsAdmin), Name = "account_isadmin_index")]
    public partial class Account
    {
        public Account()
        {
            Guesses = new HashSet<Guess>();
        }

        [Key]
        public int AccountId { get; set; }
        [Required]
        [StringLength(255)]
        public string FullName { get; set; }
        [Required]
        [StringLength(255)]
        public string Email { get; set; }
        [Required]
        [StringLength(255)]
        public string Pass { get; set; }
        [Column(TypeName = "date")]
        public DateTime Birthday { get; set; }
        public int Points { get; set; }
        public bool IsAdmin { get; set; }
        public int ActivitySatus { get; set; }

        [ForeignKey(nameof(ActivitySatus))]
        [InverseProperty(nameof(ActivityStatus.Accounts))]
        public virtual ActivityStatus ActivitySatusNavigation { get; set; }
        [InverseProperty(nameof(Guess.Account))]
        public virtual ICollection<Guess> Guesses { get; set; }
    }
}
