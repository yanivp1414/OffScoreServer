using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace OffScoreServerBL.Models
{
    partial class OffScoreContext
    {
        public void AddAccount(Account acc)
        {
            this.Accounts.Add(acc);
            this.SaveChanges();
        }
        public Account Login(string email, string password)
        {
            return this.Accounts
                .Include(a => a.Guesses).ThenInclude(b => b.Game).ThenInclude(c => c.Team1)
                .Include(a => a.Guesses).ThenInclude(b => b.Game).ThenInclude(c => c.Team2)
                .FirstOrDefault(x => x.Email == email && x.Pass == password);
        }

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
            return this.Accounts
                .Include(a => a.Guesses).ThenInclude(b => b.Game).ThenInclude(c => c.Team1)
                .Include(a => a.Guesses).ThenInclude(b => b.Game).ThenInclude(c => c.Team2)
                .FirstOrDefault(x => x.AccountId == id);
        }

        public Guess GetGuess(int userId, int gameId)
        {
            return this.Guesses
            .Include(a => a.Game).ThenInclude(b => b.Team1).ThenInclude(c => c.GlobalLeague)
            .Include(a => a.Game).ThenInclude(b => b.Team2).ThenInclude(c => c.GlobalLeague)
            .Include(d => d.Account).ThenInclude(e => e.Guesses)
            .FirstOrDefault(x => x.AccountId == userId && x.GameId == gameId);
        }

        public List<Game> GetActiveGames() => this.Games.Include(a => a.Guesses).Include(b => b.Team1).Include(c => c.Team2).Where(x => x.ActivityStatus == 1).ToList();
        public List<League> GetLeagues()
        {
            return this.Leagues.ToList();
        }

        public List<Team> GetTeams()
        { 
            return this.Teams
                .Include(d => d.GlobalLeague)
                .Include(d => d.LocalLeague)
                .ToList(); 
        }
        public void AddGames(List<Game> games)
        {
            foreach (Game g in games)
                this.Games.Add(g);
            SaveChanges();
        }

        public List<Guess> GetGuessesByDateAndId(int days, int id)
        {
            var lst = this.Guesses.Include(a => a.Game).ThenInclude(b => b.Team1).ThenInclude(c => c.GlobalLeague)
            .Include(a => a.Game).ThenInclude(b => b.Team2).ThenInclude(c => c.GlobalLeague)
            .Include(d => d.Account).ThenInclude(e => e.Guesses)
            .Where(x => x.GuessingTime.Date > (DateTime.Now.AddDays(-1 * days)) && x.AccountId == id).ToList();
            return lst;
        }
        public Game GetGameById(int id) => this.Games.Include(a => a.Guesses).Include(b => b.Team1).Include(c => c.Team2).FirstOrDefault(x => x.GameId == id);
        
        public void AddGuess(Guess g)
        {
            this.Guesses.Add(g);
            this.SaveChanges();
        }
        public List<Guess> GetGuessesForGame(int gameId) => this.Guesses.Include(x => x.Account).Where(b => b.GameId == gameId).ToList();
        public List<Account> GetLeaderboard() => this.Accounts.OrderBy(x => x.Points).Take(100).ToList();
    }

}
