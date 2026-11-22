using FitCRModels;
using System.Globalization;

namespace FitCRController
{
    /// <summary>Controller for the visits of the users to the gym.</summary>
    public class VisitController
    {
        private const int VisitsForRest = 5;
        private const string RestMessage = "Descansa esta semana: ya acumulaste 5 visitas";

        public VisitController(InformationService informationService, NotificationController notificationController, GoalController goalController, BadgeController badgeController)
        {
            this.InformationService = informationService;
            this.NotificationController = notificationController;
            this.GoalController = goalController;
            this.BadgeController = badgeController;
        }

        public InformationService InformationService { get; set; }

        public NotificationController NotificationController { get; set; }

        public GoalController GoalController { get; set; }

        public BadgeController BadgeController { get; set; }

        /// <summary>Gets the visits of a user, from the newest to the oldest.</summary>
        public List<Visit> GetVisits(string userId)
        {
            return this.InformationService.Visits
                .Where(v => v.UserId == userId)
                .OrderByDescending(v => v.Date)
                .ToList();
        }

        /// <summary>Registers the visit of a user for today. When the user reaches 5 visits in the same week, a notification tells him to rest.</summary>
        public OperationResult RegisterVisit(string userId)
        {
            if (!this.InformationService.Users.Any(u => u.Id == userId))
            {
                return OperationResult.Fail("The user does not exist.");
            }

            var today = DateTime.Today;
            if (this.InformationService.Visits.Any(v => v.UserId == userId && v.Date.Date == today))
            {
                return OperationResult.Fail("The visit of today was already registered.");
            }

            var id = this.InformationService.GenerateId("V", this.InformationService.Visits.Select(v => v.Id), 4);
            this.InformationService.Visits.Add(new Visit(id, userId, today.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture)));
            this.InformationService.Save(this.InformationService.Visits);

            this.NotifyRestIfNeeded(userId, today);
            this.GoalController.EvaluateGoals(userId);
            this.BadgeController.RefreshShameBadges();
            return OperationResult.Ok("Visit registered.", id);
        }

        private void NotifyRestIfNeeded(string userId, DateTime today)
        {
            var weekStart = today.AddDays(-(((int)today.DayOfWeek + 6) % 7));
            var visitsInWeek = this.InformationService.Visits.Count(v => v.UserId == userId && v.Date.Date >= weekStart);
            var alreadyNotified = this.InformationService.Notifications.Any(n =>
                n.RecipientId == userId && n.Message == RestMessage && n.Date.Date >= weekStart);

            if (visitsInWeek >= VisitsForRest && !alreadyNotified)
            {
                this.NotificationController.Add(userId, RestMessage);
            }
        }
    }
}
