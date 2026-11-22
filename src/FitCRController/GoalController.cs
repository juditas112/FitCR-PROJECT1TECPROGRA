using FitCRModels;
using FitCRModels.Enums;
using System.Globalization;

namespace FitCRController
{
    /// <summary>Controller for the goals of the users.</summary>
    public class GoalController
    {
        public GoalController(InformationService informationService, BadgeController badgeController)
        {
            this.InformationService = informationService;
            this.BadgeController = badgeController;
        }

        public InformationService InformationService { get; set; }

        public BadgeController BadgeController { get; set; }

        /// <summary>Gets the goals of a user, the ones with the closest deadline first.</summary>
        public List<Goal> GetGoals(string userId)
        {
            return this.InformationService.Goals
                .Where(g => g.UserId == userId)
                .OrderBy(g => g.Deadline)
                .ToList();
        }

        /// <summary>Adds a goal for a user.</summary>
        public OperationResult AddGoal(string userId, GoalType type, double targetValue, DateTime deadline)
        {
            if (!this.InformationService.Users.Any(u => u.Id == userId))
            {
                return OperationResult.Fail("The user does not exist.");
            }

            if (targetValue <= 0)
            {
                return OperationResult.Fail("The goal value must be greater than zero.");
            }

            if (deadline.Date <= DateTime.Today)
            {
                return OperationResult.Fail("The deadline must be a future date.");
            }

            var id = this.InformationService.GenerateId("O", this.InformationService.Goals.Select(g => g.Id), 3);
            var goal = new Goal(
                id,
                userId,
                ((int)type).ToString(),
                targetValue.ToString(CultureInfo.InvariantCulture),
                deadline.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                bool.FalseString);

            this.InformationService.Goals.Add(goal);
            this.InformationService.Save(this.InformationService.Goals);
            this.EvaluateGoals(userId);
            return OperationResult.Ok("Goal added.", id);
        }

        /// <summary>Checks the goals of a user that are still open. The ones that were reached are marked and their badge is given.</summary>
        public List<Goal> EvaluateGoals(string userId)
        {
            var achieved = new List<Goal>();
            var openGoals = this.InformationService.Goals
                .Where(g => g.UserId == userId && !g.IsAchieved && g.Deadline.Date >= DateTime.Today);

            foreach (var goal in openGoals)
            {
                if (this.IsReached(goal))
                {
                    goal.IsAchieved = true;
                    achieved.Add(goal);
                }
            }

            if (achieved.Count > 0)
            {
                this.InformationService.Save(this.InformationService.Goals);
                achieved.ForEach(goal => this.BadgeController.AwardGoalBadge(goal));
            }

            return achieved;
        }

        private bool IsReached(Goal goal)
        {
            var last = this.InformationService.Measurements
                .Where(m => m.UserId == goal.UserId)
                .OrderByDescending(m => m.Date)
                .FirstOrDefault();

            switch (goal.GoalType)
            {
                case GoalType.Muscle:
                    return last != null && last.MuscleMass >= goal.TargetValue;
                case GoalType.Fat:
                    return last != null && last.BodyFat <= goal.TargetValue;
                default:
                    var visits = this.InformationService.Visits.Count(v => v.UserId == goal.UserId && v.Date.Date <= goal.Deadline.Date);
                    return visits >= goal.TargetValue;
            }
        }
    }
}
