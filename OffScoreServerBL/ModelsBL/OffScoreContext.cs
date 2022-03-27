using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OffScoreServerBL.Models
{
    partial class OffScoreContext
    {
        public void AddAccount(Account acc)
        {
            this.Accounts.Add(acc);
            this.SaveChanges();
        }
        public Account Login(string email, string password) => this.Accounts.FirstOrDefault(x => x.Email == email && x.Pass == password);
        public bool Exists(string email) => this.Accounts.Any(x => x.Email == email);
        public Account FindAccountById(int id, string fullName, DateTime birthDay, string pass) 
        { 
            Account c = this.Accounts.FirstOrDefault(x => x.AccountId == id);
            if (c.FullName != fullName)
                c.FullName = fullName;
            if (c.Birthday != birthDay)
                c.Birthday = birthDay;
            if (c.Pass != pass)
                c.Pass = pass;
            this.SaveChanges();
            return this.Accounts.FirstOrDefault(x => x.AccountId == id);
        }

        public Guess GetGuess(int userId, int gameId) => this.Guesses.FirstOrDefault(x => x.AccountId == userId && x.GameId == gameId);

        public List<Game> GetActiveGames() => this.Games.Where(x => x.ActivityStatus == 1).ToList();
        public List<League> GetLeagues() => this.Leagues.ToList();
        public List<Team> GetTeams() => this.Teams.ToList();


    }

}
