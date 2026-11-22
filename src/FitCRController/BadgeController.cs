using FitCRModels;
using FitCRModels.Enums;

namespace FitCRController
{
    /// <summary>Controller for the badges of the users, the ones for achievements and the badges of shame.</summary>
    public class BadgeController
    {
        public const string FewVisitsBadge = "Top 10 con menos visitas";

        public const string NoProgressBadge = "Sin agregar progreso";

        public const string MissedGoalBadge = "Objetivo no cumplido";

        private const int VisitsWindowDays = 30;
        private const int ProgressWindowDays = 45;
        private const int RankingSize = 10;

        public BadgeController(InformationService informationService, NotificationController notificationController)
        {
            this.InformationService = informationService;
            this.NotificationController = notificationController;
        }

        public InformationService InformationService { get; set; }

        public NotificationController NotificationController { get; set; }

        /// <summary>Gets the badges of a user, from the newest to the oldest.</summary>
        public List<Badge> GetBadges(string userId)
        {
            return this.InformationService.Badges
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.Date)
                .ThenByDescending(b => b.Id)
                .ToList();
        }

        /// <summary>Gives the badge of a goal that was achieved. A user gets each of these badges only once.</summary>
        public void AwardGoalBadge(Goal goal)
        {
            var name = this.GetGoalBadgeName(goal.GoalType);
            var alreadyHas = this.InformationService.Badges.Any(b => b.UserId == goal.UserId && b.Name == name);
            if (alreadyHas)
            {
                return;
            }

            this.AddBadge(goal.UserId, BadgeType.Achievement, name);
            this.InformationService.Save(this.InformationService.Badges);
            this.NotificationController.Add(goal.UserId, $"Has ganado la insignia: {name}");
        }

        /// <summary>Recalculates the badges of shame. The new ones are given and the ones that are not valid anymore are removed.</summary>
        public void RefreshShameBadges()
        {
            var shouldHave = new HashSet<(string UserId, string Name)>();

            foreach (var user in this.GetUsersWithFewestVisits())
            {
                shouldHave.Add((user.Id, FewVisitsBadge));
            }

            foreach (var user in this.InformationService.Users)
            {
                if (!this.HasRecentMeasurement(user.Id))
                {
                    shouldHave.Add((user.Id, NoProgressBadge));
                }

                if (this.HasMissedGoal(user.Id))
                {
                    shouldHave.Add((user.Id, MissedGoalBadge));
                }
            }

            var shameNames = new[] { FewVisitsBadge, NoProgressBadge, MissedGoalBadge };
            var removed = this.InformationService.Badges.RemoveAll(b =>
                b.BadgeType == BadgeType.Shame && shameNames.Contains(b.Name) && !shouldHave.Contains((b.UserId, b.Name)));

            var added = 0;
            foreach (var (userId, name) in shouldHave)
            {
                var alreadyHas = this.InformationService.Badges.Any(b => b.UserId == userId && b.Name == name);
                if (!alreadyHas)
                {
                    this.AddBadge(userId, BadgeType.Shame, name);
                    this.NotificationController.Add(userId, $"Insignia de la verguenza: {name}");
                    added++;
                }
            }

            if (removed > 0 || added > 0)
            {
                this.InformationService.Save(this.InformationService.Badges);
            }
        }

        private string GetGoalBadgeName(GoalType type)
        {
            return type switch
            {
                GoalType.Muscle => "Meta de musculatura cumplida",
                GoalType.Fat => "Meta de grasa cumplida",
                _ => "Meta de visitas cumplida",
            };
        }

        private void AddBadge(string userId, BadgeType type, string name)
        {
            var id = this.InformationService.GenerateId("B", this.InformationService.Badges.Select(b => b.Id), 3);
            var date = DateTime.Today.ToString("yyyy-MM-dd");
            this.InformationService.Badges.Add(new Badge(id, userId, ((int)type).ToString(), name, date));
        }

        private List<BasicUser> GetUsersWithFewestVisits()
        {
            var since = DateTime.Today.AddDays(-VisitsWindowDays);
            return this.InformationService.Users
                .OrderBy(u => this.InformationService.Visits.Count(v => v.UserId == u.Id && v.Date.Date >= since))
                .ThenBy(u => u.Id)
                .Take(RankingSize)
                .ToList();
        }

        private bool HasRecentMeasurement(string userId)
        {
            var since = DateTime.Today.AddDays(-ProgressWindowDays);
            return this.InformationService.Measurements.Any(m => m.UserId == userId && m.Date.Date >= since);
        }

        private bool HasMissedGoal(string userId)
        {
            return this.InformationService.Goals.Any(g => g.UserId == userId && !g.IsAchieved && g.Deadline.Date < DateTime.Today);
        }
    }
}
