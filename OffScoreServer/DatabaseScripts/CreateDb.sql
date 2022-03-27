CREATE DATABASE OffScore;
Go
Use OffScore;
Go
CREATE TABLE Account(
    AccountId INT IDENTITY(1,1) NOT NULL,
    FullName NVARCHAR(255) NOT NULL,
    Email NVARCHAR(255) NOT NULL,
    Pass NVARCHAR(255) NOT NULL,
    Birthday DATE NOT NULL,
    Points INT NOT NULL DEFAULT 0,
    IsAdmin BIT NOT NULL DEFAULT 0,
    ActivitySatus INT NOT NULL DEFAULT 1
);
ALTER TABLE
    Account ADD CONSTRAINT account_accountid_primary PRIMARY KEY(AccountId);
CREATE UNIQUE INDEX account_email_unique ON
    Account(Email);
CREATE INDEX account_birthday_index ON
    Account(Birthday);
CREATE INDEX account_isadmin_index ON
    Account(IsAdmin);
CREATE TABLE Guess(
    GuessId INT IDENTITY(1,1) NOT NULL,
    AccountId INT NOT NULL,
    GuessingTime DATETIME NOT NULL DEFAULT GETDATE(),
    Team1Guess INT NOT NULL,
    Team2Guess INT NOT NULL,
    GameId INT NOT NULL,
    ActivityStatus INT NOT NULL DEFAULT 1
);
ALTER TABLE
    Guess ADD CONSTRAINT guess_guessid_primary PRIMARY KEY(GuessId);
CREATE TABLE League(
    LeagueId INT IDENTITY(1,1) NOT NULL,
    Country NVARCHAR(255) NOT NULL,
    LeagueName NVARCHAR(255) NOT NULL
);
ALTER TABLE
    League ADD CONSTRAINT league_leagueid_primary PRIMARY KEY(LeagueId);
CREATE INDEX league_country_index ON
    League(Country);
CREATE UNIQUE INDEX league_leguename_unique ON
    League(LeagueName);
CREATE TABLE Team(
    TeamId INT IDENTITY(1,1) NOT NULL,
    GlobalLeagueId INT NOT NULL,
	LocalLeagueId Int NOT NULL,
    TeamName NVARCHAR(255) NOT NULL,
    TeamRank INT NOT NULL,
    TeamPoints INT NOT NULL
);
ALTER TABLE
    Team ADD CONSTRAINT team_teamid_primary PRIMARY KEY(TeamId);
CREATE UNIQUE INDEX team_teamname_unique ON
    Team(TeamName);
CREATE TABLE Game(
    GameId INT IDENTITY(1,1) NOT NULL,
    Team1Id INT NOT NULL,
    Team2Id INT NOT NULL,
    FinalScore NVARCHAR(255) NOT NULL,
    ActivityStatus INT NOT NULL DEFAULT 1
);
ALTER TABLE
    Game ADD CONSTRAINT game_gameid_primary PRIMARY KEY(GameId);
CREATE TABLE ActivityStatus(
    StatusId INT IDENTITY(1,1) NOT NULL,
    StatusName NVARCHAR(255) NOT NULL
);
ALTER TABLE
    ActivityStatus ADD CONSTRAINT activitystatus_statusid_primary PRIMARY KEY(StatusId);
ALTER TABLE
    Guess ADD CONSTRAINT guess_accountid_foreign FOREIGN KEY(AccountId) REFERENCES Account(AccountId);
ALTER TABLE
    Team ADD CONSTRAINT team_globalleagueid_foreign FOREIGN KEY(GlobalLeagueId) REFERENCES League(LeagueId);
ALTER TABLE
    Team ADD CONSTRAINT team_localleagueid_foreign FOREIGN KEY(LocalLeagueId) REFERENCES League(LeagueId);
ALTER TABLE
    Guess ADD CONSTRAINT guess_gameid_foreign FOREIGN KEY(GameId) REFERENCES Game(GameId);
ALTER TABLE
    Guess ADD CONSTRAINT guess_activitystatus_foreign FOREIGN KEY(ActivityStatus) REFERENCES ActivityStatus(StatusId);
ALTER TABLE
    Game ADD CONSTRAINT game_activitystatus_foreign FOREIGN KEY(ActivityStatus) REFERENCES ActivityStatus(StatusId);
ALTER TABLE
    Game ADD CONSTRAINT game_team2id_foreign FOREIGN KEY(Team2Id) REFERENCES Team(TeamId);
ALTER TABLE
    Game ADD CONSTRAINT game_team1id_foreign FOREIGN KEY(Team1Id) REFERENCES Team(TeamId);
ALTER TABLE
    Account ADD CONSTRAINT account_activitysatus_foreign FOREIGN KEY(ActivitySatus) REFERENCES ActivityStatus(StatusId);

