using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using OffScoreServer.DataTransferObjects;
using OffScoreServerBL.Models;

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
        public List<Team> GetTeams()
        {
            try
            {
                List<Team> teams = context.GetTeams();
                if (teams != null)
                {
                    Response.StatusCode = (int)System.Net.HttpStatusCode.OK;
                    return teams;
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


    }
}
