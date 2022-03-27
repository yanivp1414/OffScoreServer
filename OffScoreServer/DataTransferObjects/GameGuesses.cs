using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace OffScoreServer.DataTransferObjects
{
    public class GameGuesses
    {
        public int GameId { get; set; }
        public int Team1Guess { get; set; }
        public int Team2Guess { get; set; }
        public int UserId { get; set; }
    }
}
