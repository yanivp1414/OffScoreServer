using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffScoreServer.DataTransferObjects;
using OffScoreServerBL.Models;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace OffScoreServer.Controllers
{
    [Route("api")]
    [ApiController]
    public class MainController : ControllerBase
    {
        OffScoreContext context;

        public MainController(OffScoreContext context)
        {
            this.context = context;
        }
        [Route("Login")]
        [HttpGet]
        public Account Login([FromQuery] string email, [FromQuery] string password)
        {
            try
            {
                Account acc = context.Login(email, password);

                if(acc != null)
                {
                    HttpContext.Session.SetObject("user", acc);
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return acc;
                }
                else
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.Unauthorized;
                    return null;
                }
            }
            catch
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
        }

        // This method adds the specified user to the database
        [Route("SignUp")]
        [HttpPost]
        public bool SignUp([FromBody] Account acc)
        {
            Account user = HttpContext.Session.GetObject<Account>("user");           

            if (user == null)
            {
                Account newAcc = new Account()
                {
                    FullName = acc.FullName,
                    Email = acc.Email,
                    Pass = acc.Pass,
                    Birthday = acc.Birthday,
                };

                try
                {
                    bool exists = context.Exists(acc.Email);
                    if (!exists)
                    {
                        context.AddAccount(newAcc);
                        Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                        return true;
                    }
                    else
                        Response.StatusCode = Response.StatusCode = (int)System.Net.HttpStatusCode.Conflict;
                }
                catch (Exception e)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                }
            }
            else
                Response.StatusCode = (int)System.Net.HttpStatusCode.Forbidden;

            return false;
        }

        [Route("Update")]
        [HttpGet]
        public Account Update([FromQuery] string name, [FromQuery] DateTime birthday, [FromQuery] string pass, [FromQuery] int id)
        {
            try
            {
                Account c = context.FindAccountById(id, name, birthday, pass);
                if(c != null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return c;
                }
                else
                {
                    Response.StatusCode = Response.StatusCode = (int)System.Net.HttpStatusCode.Conflict;
                }
                return null;
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
        }

        [Route("GetGuesses")]
        [HttpGet]
        public List<GameGuesses> GetGuesses([FromQuery] string ids, [FromQuery] string UserId )
        {
            if(ids == null)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return null;
            }
                
            try
            {
                Account acc = HttpContext.Session.GetObject<Account>("user");
                List<string> idsList = ids.Split('_').ToList();
                List<int> idsIntList = new List<int>();
                foreach (string s in idsList)
                    idsIntList.Add(int.Parse(s));

                List<Guess> guesses = new List<Guess>();
                foreach(int id in idsIntList)
                {
                    Guess g = context.GetGuess(int.Parse(UserId), id);
                    if (g == null)
                    {
                        g = new Guess() { AccountId = int.Parse(UserId), GameId = id };
                    }
                    guesses.Add(g);
                }
                List<GameGuesses> toReturn = new List<GameGuesses>();
                foreach (Guess g in guesses)
                {
                    if (g != null)
                        toReturn.Add(new GameGuesses() { Team1Guess = g.Team1Guess, Team2Guess = g.Team2Guess, GameId = g.GameId, UserId = g.AccountId });
                    else
                        toReturn.Add(new GameGuesses() { Team1Guess = -1, Team2Guess = -1, GameId = g.GameId, UserId = g.AccountId });
                }
                    

                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return toReturn;
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
        }

        [Route("GetActiveGames")]
        [HttpGet]
        public List<Game> GetActiveGames()
        {
            try
            {
                List<Game> games = context.GetActiveGames();
                if(games != null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return games;
                }
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
        }

        [Route("GetLeagues")]
        [HttpGet]
        public List<League> GetLeagues()
        {
            try
            {
                List<League> leagues = context.GetLeagues();
                if (leagues != null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return leagues;
                }
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
        }

        [Route("GetTeams")]
        [HttpGet]
        public string GetTeams()
        {
            try
            {
                List<Team> teams = context.GetTeams();
                if (teams != null)
                {

                    string json = Newtonsoft.Json.JsonConvert.SerializeObject(teams, new Newtonsoft.Json.JsonSerializerSettings() { ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore });
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return json;
                }
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return null;
            }
        }

        [Route("CreateGames")]
        [HttpGet]
        public void CreateGames()
        {
            try
            {
                Random rand = new Random();
                List<Team> teams = context.GetTeams();
                var ints = Enumerable.Range(1, 20)
                                     .Select(i => new Tuple<int, int>(rand.Next(20), i))
                                     .OrderBy(i => i.Item1)
                                     .Select(i => i.Item2);
                List<int> teamIds = new List<int>(ints.Take(10));
                List<Game> games = new List<Game>();
                for (int i = 0; i < teamIds.Count; i+=2)
                {
                    games.Add(new Game()
                    {
                        Team1Id = teamIds[i],
                        Team2Id = teamIds[i + 1],
                        ActivityStatus = 1,
                        FinalScore = ""
                    }
                    );
                }
                context.AddGames(games);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            }
        }

        [Route("GetPreviousDays")]
        [HttpGet]
        public List<Guess> GetPreviousDays([FromQuery] int NumberOfDays, [FromQuery] int Id)
        {
            try
            {
                List<Guess> previousDays = context.GetGuessesByDateAndId(NumberOfDays, Id);
                if(previousDays != null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return previousDays;
                }
                
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                
            }
            return null;
        }

        [Route("GetGamesByIds")]
        [HttpGet]
        public List<Game> GetGamesByIds([FromQuery] string ids)
        {
            try
            {
                List<int> gameIds = JsonSerializer.Deserialize<List<int>>(ids);
                List<Game> games = new List<Game>();
                foreach(int i in gameIds)
                {
                    Game g = context.GetGameById(i);
                    if(g != null)
                        games.Add(g);

                }
                if (games != null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return games;
                }

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            }
            return null;
        }

        [Route("GetResults")]
        [HttpGet]
        public void GetResults()
        {
            try
            {
                Random r = new Random();
                List<Game> dailyGames = context.GetActiveGames();
                foreach(Game g in dailyGames)
                    g.SetScore(r.Next(0, 10), r.Next(0, 10));
                context.SaveChanges();
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            }
        }
        [Route("Logout")]
        [HttpGet]
        public void Logout()
        {
            try
            {
                HttpContext.Session.Clear();
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            }
        }

        

        [Route("AddGuess")]
        [HttpPost]
        public bool AddGuess([FromBody] Guess g)
        {
            try
            {
                context.AddGuess(g);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return true;
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            }
            return false;
        }

        
        [Route("CheckGames")]
        [HttpGet]
        public void CheckGames()
        {
            Random r = new Random();
            try
            {
                //randomly draw scores for active games
                List<Game> activeGames = context.GetActiveGames();
                foreach(Game g in activeGames)
                {
                    int score1 = r.Next(0, 10);
                    int score2 = r.Next(0, 10);
                    g.SetScore(score1, score2);
                    g.Deactivate();
                    List<Guess> gameGuesses = context.GetGuessesForGame(g.GameId);
                    foreach(Guess guess in gameGuesses)
                    {
                        if((score1 == score2) == (guess.Team2Guess == guess.Team1Guess))
                        {
                            guess.Account.Points++;
                        }
                        else if(((guess.Team1Guess > guess.Team2Guess) == (score1 > score2)))
                        {
                            guess.Account.Points++;
                        }

                        if(score1 == guess.Team1Guess && score2 == guess.Team2Guess)
                            guess.Account.Points++;

                    }
                }
                context.SaveChanges();
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            }
        }

        [Route("GetGameById")]
        [HttpGet]
        public Game GetGameById([FromQuery] int id)
        {
            
            try
            {
                Game g = context.GetGameById(id);
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return g;

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            }
            return null;
        }

        [Route("GetLeaderboard")]
        [HttpGet]
        public List<Account> GetLeaderboard()
        {

            try
            {
                List<Account> g = context.GetLeaderboard();
                Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                return g;

            }
            catch (Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;

            }
            return null;
        }
    }
}

