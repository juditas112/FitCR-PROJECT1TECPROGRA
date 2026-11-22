using FitCRModels;

namespace FitCRController
{
    /// <summary>Controller for the visits of the users to the gym.</summary>
    public class VisitController : BaseController
    {
        private const int VisitsForRest = 5;
        private const string RestMessage = "Descansa esta semana: ya acumulaste 5 visitas";

        private readonly NotificationController notificationController;
        private readonly GoalController goalController;
        private readonly BadgeController badgeController;

        public VisitController(InformationService informationService, NotificationController notificationController, GoalController goalController, BadgeController badgeController)
            : base(informationService)
        {
            this.notificationController = notificationController;
            this.goalController = goalController;
            this.badgeController = badgeController;
        }

        /// <summary>Gets the visits of a user, from the newest to the oldest.</summary>
        public List<Visit> GetVisits(string userId)
        {
            return this.InformationService.Visits
                .Where(v => v.UserId == userId)
                .OrderByDescending(v => v.Date)
                .ToList();
        }

        /// <summary>Registers the visit of a user for today. At 5 visits in the same week, a notification tells the user to rest.</summary>
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

            var id = IdGenerator.Next("V", this.InformationService.Visits.Select(v => v.Id), 4);
            this.InformationService.Visits.Add(new Visit(id, userId, CsvFormat.Date(today)));
            this.InformationService.Save(this.InformationService.Visits);

            this.NotifyRestIfNeeded(userId, today);
            this.goalController.EvaluateGoals(userId);
            this.badgeController.RefreshShameBadges();
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
                this.notificationController.Add(userId, RestMessage);
            }
        }
    }
}
