using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore;

namespace OffScoreServerBL.Models
{
    partial class Game
    {
        public void SetScore(int team1, int team2) => this.FinalScore = $"{team1}-{team2}";
        public void Deactivate() => this.ActivityStatus = 2;
    }
}
