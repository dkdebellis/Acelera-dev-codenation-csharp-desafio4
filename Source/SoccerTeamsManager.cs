using System;
using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Exceptions;

namespace Codenation.Challenge
{
    public class SoccerTeamsManager : IManageSoccerTeams
    {

        public List<Teams> AllTeams = new List<Teams>();
        public List<Players> AllPlayers = new List<Players>();
        public List<Captain> Cap = new List<Captain>();

        public SoccerTeamsManager()
        {
        }

        public bool ValidateTeam (long teamId) => AllTeams.Any(x => x.Id == teamId);
        
        public bool ValidadePlayer (long playerId) => AllPlayers.Any(x => x.Id == playerId);
        
        public void AddTeam(long id, string name, DateTime createDate, string mainShirtColor, string secondaryShirtColor)
        {
            if (ValidateTeam(id))
                throw new UniqueIdentifierException();

            AllTeams.Add(new Teams
            {
                Id = id,
                Name = name,
                DateCreation = createDate,
                MainUniform = mainShirtColor,
                SecondaryUniform = secondaryShirtColor

            });
        }

        public void AddPlayer(long id, long teamId, string name, DateTime birthDate, int skillLevel, decimal salary)
        {
            if (ValidadePlayer(id))
                throw new UniqueIdentifierException();

            if (!ValidateTeam(teamId))
                throw new TeamNotFoundException();

            AllPlayers.Add(new Players
            {
                Id = id,
                TeamId = teamId,
                Name = name,
                BirthDate = birthDate,
                SkillLevel = skillLevel,
                Salary = salary

            });
        }

        public void SetCaptain(long playerId)
        {

            if (!ValidadePlayer(playerId))
                throw new PlayerNotFoundException();

            long IdTeam = AllPlayers
                .Where(x => x.Id == playerId)
                .Select(x => x.TeamId)
                .FirstOrDefault();

            Captain CapExist = Cap.FirstOrDefault(x => x.TeamId == IdTeam);

            if (CapExist != null)
                Cap.Remove(CapExist);

            Cap.Add(new Captain
            {
                PlayerId = playerId,
                TeamId = IdTeam

            });
        }

        public long GetTeamCaptain(long teamId)
        {
            if (!ValidateTeam(teamId))
                throw new TeamNotFoundException();

            Captain CapExist = Cap.FirstOrDefault(x => x.TeamId == teamId);

            if (CapExist == null)
                throw new CaptainNotFoundException();

            return CapExist.PlayerId;
        }

        public string GetPlayerName(long playerId)
        {
            if (!ValidadePlayer(playerId))
                throw new PlayerNotFoundException();

            return AllPlayers
                .Where(x => x.Id == playerId)
                .Select(x => x.Name)
                .FirstOrDefault().ToString();
        }

        public string GetTeamName(long teamId)
        {
            if (!ValidateTeam(teamId))
                throw new TeamNotFoundException();

            return AllTeams
                .Where(x => x.Id == teamId)
                .Select(x => x.Name)
                .FirstOrDefault().ToString();
        }

        public List<long> GetTeamPlayers(long teamId)
        {
            if (!ValidateTeam(teamId))
                throw new TeamNotFoundException();

            return AllPlayers
                .OrderBy(x => x.Id)
                .Where(x => x.TeamId == teamId)
                .Select(x => x.Id)
                .ToList();
        }

        public long GetBestTeamPlayer(long teamId)
        {
            if (!ValidateTeam(teamId))
                throw new TeamNotFoundException();

            return AllPlayers
                .OrderByDescending(x => x.SkillLevel)
                .ThenBy(x => x.Id)
                .Where(x => x.TeamId == teamId)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        public long GetOlderTeamPlayer(long teamId)
        {
            if (!ValidateTeam(teamId))
                throw new TeamNotFoundException();

            return AllPlayers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.Id)
                .Where(x => x.TeamId == teamId)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        public List<long> GetTeams()
        {
            return AllTeams
                .OrderBy(x => x.Id)
                .Select(x => x.Id)
                .ToList();
        }

        public long GetHigherSalaryPlayer(long teamId)
        {
            if (!ValidateTeam(teamId))
                throw new TeamNotFoundException();

            return AllPlayers
                .OrderByDescending(x => x.Salary)
                .ThenBy(x => x.Id)
                .Where(x => x.TeamId == teamId)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        public decimal GetPlayerSalary(long playerId)
        {
            if (!ValidadePlayer(playerId))
                throw new PlayerNotFoundException();

            return AllPlayers
                .Where(x => x.Id == playerId)
                .Select(x => x.Salary)
                .FirstOrDefault();
        }

        public List<long> GetTopPlayers(int top)
        {
            return AllPlayers
                .OrderByDescending(x => x.SkillLevel)
                .ThenBy(x => x.Id)
                .Select(x => x.Id)
                .Take(top)
                .ToList();
        }

        public string GetVisitorShirtColor(long teamId, long visitorTeamId)
        {
            if (!ValidateTeam(teamId) || !ValidateTeam(visitorTeamId))
                throw new TeamNotFoundException();

            string HomeUniform =
                AllTeams
                .Where(x => x.Id == teamId)
                .Select(x => x.MainUniform)
                .FirstOrDefault();

            string VisitantUniform =
                AllTeams
                .Where(x => x.Id == visitorTeamId)
                .Select(x => x.MainUniform)
                .FirstOrDefault();

            if (HomeUniform == VisitantUniform)
                return AllTeams
                    .Where(x => x.Id == visitorTeamId)
                    .Select(x => x.SecondaryUniform)
                    .FirstOrDefault();

            return VisitantUniform;
        }
    }
}

