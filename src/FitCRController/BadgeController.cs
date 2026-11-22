using FitCRController.Abstractions;
using FitCRModels;
using FitCRModels.Enums;

namespace FitCRController
{
    /// <summary>Controller for the badges of the users, the ones for achievements and the badges of shame.</summary>
    public class BadgeController : BaseController
    {
        private readonly NotificationController notificationController;
        private readonly List<IShameBadgeRule> rules;

        public BadgeController(InformationService informationService, NotificationController notificationController, IEnumerable<IShameBadgeRule> rules)
            : base(informationService)
        {
            this.notificationController = notificationController;
            this.rules = rules.ToList();
        }

        /// <summary>Gets the badges of a user, from the newest to the oldest.</summary>
        public List<Badge> GetBadges(string userId)
        {
            return this.InformationService.Badges
                .Where(b => b.UserId == userId)
                .OrderByDescending(b => b.Date)
                .ThenByDescending(b => b.Id)
                .ToList();
        }

        /// <summary>Gives a badge of achievement to a user. A user gets each of these badges only once.</summary>
        public void AwardAchievementBadge(string userId, string badgeName)
        {
            if (this.HasBadge(userId, badgeName))
            {
                return;
            }

            this.AddBadge(userId, BadgeType.Achievement, badgeName);
            this.InformationService.Save(this.InformationService.Badges);
            this.notificationController.Add(userId, $"Has ganado la insignia: {badgeName}");
        }

        /// <summary>Recalculates the badges of shame using the rules. The new ones are given and the ones that are not valid anymore are removed.</summary>
        public void RefreshShameBadges()
        {
            var shouldHave = this.rules
                .SelectMany(rule => rule.GetUserIds(this.InformationService).Select(userId => (UserId: userId, Name: rule.BadgeName)))
                .ToHashSet();
            var shameNames = this.rules.Select(rule => rule.BadgeName).ToHashSet();

            var removed = this.InformationService.Badges.RemoveAll(b =>
                b.BadgeType == BadgeType.Shame && shameNames.Contains(b.Name) && !shouldHave.Contains((b.UserId, b.Name)));

            var missing = shouldHave.Where(pair => !this.HasBadge(pair.UserId, pair.Name)).ToList();
            foreach (var pair in missing)
            {
                this.AddBadge(pair.UserId, BadgeType.Shame, pair.Name);
                this.notificationController.Add(pair.UserId, $"Insignia de la verguenza: {pair.Name}");
            }

            if (removed > 0 || missing.Count > 0)
            {
                this.InformationService.Save(this.InformationService.Badges);
            }
        }

        private bool HasBadge(string userId, string badgeName)
        {
            return this.InformationService.Badges.Any(b => b.UserId == userId && b.Name == badgeName);
        }

        private void AddBadge(string userId, BadgeType type, string name)
        {
            var id = IdGenerator.Next("B", this.InformationService.Badges.Select(b => b.Id), 3);
            this.InformationService.Badges.Add(new Badge(id, userId, CsvFormat.Code(type), name, CsvFormat.Date(DateTime.Today)));
        }
    }
}
