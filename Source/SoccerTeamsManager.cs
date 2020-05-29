using System;
using System.Collections.Generic;
using System.Linq;
using Codenation.Challenge.Exceptions;

namespace Codenation.Challenge
{
    public class SoccerTeamsManager : IManageSoccerTeams
    {

        public List<Teams> allTimes = new List<Teams>();
        public List<Players> allPlayers = new List<Players>();
        public List<Captain> cap = new List<Captain>();

        public SoccerTeamsManager()
        {
        }

        public bool IsThereATeam (long teamId)
        {
            return allTimes.Any(x => x.Id == teamId);
        }

        public bool IsThereAPlayer (long playerId)
        {
            return allPlayers.Any(x => x.Id == playerId);
        }

        public void AddTeam(long id, string name, DateTime createDate, string mainShirtColor, string secondaryShirtColor)
        {
            if (IsThereATeam(id))
                throw new UniqueIdentifierException();

            allTimes.Add(new Teams
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
            if (IsThereAPlayer(id))
                throw new UniqueIdentifierException();

            if (!IsThereATeam(teamId))
                throw new TeamNotFoundException();

            allPlayers.Add(new Players
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

            if (!IsThereAPlayer(playerId))
                throw new PlayerNotFoundException();

            long idteam = allPlayers
                .Where(x => x.Id == playerId)
                .Select(x => x.TeamId)
                .FirstOrDefault();

            Captain capexiste = cap.FirstOrDefault(x => x.teamId == idteam);

            if (capexiste != null)
                cap.Remove(capexiste);

            cap.Add(new Captain
            {
                playerId = playerId,
                teamId = idteam

            });
        }

        public long GetTeamCaptain(long teamId)
        {
            if (!IsThereATeam(teamId))
                throw new TeamNotFoundException();

            Captain capexiste = cap.FirstOrDefault(x => x.teamId == teamId);

            if (capexiste == null)
                throw new CaptainNotFoundException();

            return capexiste.playerId;
        }

        public string GetPlayerName(long playerId)
        {
            if (!IsThereAPlayer(playerId))
                throw new PlayerNotFoundException();

            return allPlayers
                .Where(x => x.Id == playerId)
                .Select(x => x.Name)
                .FirstOrDefault().ToString();
        }

        public string GetTeamName(long teamId)
        {
            if (!IsThereATeam(teamId))
                throw new TeamNotFoundException();

            return allTimes
                .Where(x => x.Id == teamId)
                .Select(x => x.Name)
                .FirstOrDefault().ToString();
        }

        public List<long> GetTeamPlayers(long teamId)
        {
            if (!IsThereATeam(teamId))
                throw new TeamNotFoundException();

            return allPlayers
                .OrderBy(x => x.Id)
                .Where(x => x.TeamId == teamId)
                .Select(x => x.Id)
                .ToList();
        }

        public long GetBestTeamPlayer(long teamId)
        {
            if (!IsThereATeam(teamId))
                throw new TeamNotFoundException();

            return allPlayers
                .OrderByDescending(x => x.SkillLevel)
                .ThenBy(x => x.Id)
                .Where(x => x.TeamId == teamId)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        public long GetOlderTeamPlayer(long teamId)
        {
            if (!IsThereATeam(teamId))
                throw new TeamNotFoundException();

            return allPlayers
                .OrderBy(x => x.BirthDate)
                .ThenBy(x => x.Id)
                .Where(x => x.TeamId == teamId)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        public List<long> GetTeams()
        {
            return allTimes
                .OrderBy(x => x.Id)
                .Select(x => x.Id)
                .ToList();
        }

        public long GetHigherSalaryPlayer(long teamId)
        {
            if (!IsThereATeam(teamId))
                throw new TeamNotFoundException();

            return allPlayers
                .OrderByDescending(x => x.Salary)
                .ThenBy(x => x.Id)
                .Where(x => x.TeamId == teamId)
                .Select(x => x.Id)
                .FirstOrDefault();
        }

        public decimal GetPlayerSalary(long playerId)
        {
            if (!IsThereAPlayer(playerId))
                throw new PlayerNotFoundException();

            return allPlayers
                .Where(x => x.Id == playerId)
                .Select(x => x.Salary)
                .FirstOrDefault();
        }

        public List<long> GetTopPlayers(int top)
        {
            return allPlayers
                .OrderByDescending(x => x.SkillLevel)
                .ThenBy(x => x.Id)
                .Select(x => x.Id)
                .Take(top)
                .ToList();
        }

        public string GetVisitorShirtColor(long teamId, long visitorTeamId)
        {
            if (!IsThereATeam(teamId) || !IsThereATeam(visitorTeamId))
                throw new TeamNotFoundException();

            string home = allTimes.Where(x => x.Id == teamId).Select(x => x.MainUniform).FirstOrDefault();
            string visitant = allTimes.Where(x => x.Id == visitorTeamId).Select(x => x.MainUniform).FirstOrDefault();

            if (home == visitant)
                return allTimes
                    .Where(x => x.Id == visitorTeamId)
                    .Select(x => x.SecondaryUniform)
                    .FirstOrDefault();

            return allTimes
                    .Where(x => x.Id == visitorTeamId)
                    .Select(x => x.MainUniform)
                    .FirstOrDefault();
        }
    }
}
