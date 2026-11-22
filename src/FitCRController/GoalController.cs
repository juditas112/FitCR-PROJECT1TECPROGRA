using FitCRController.Abstractions;
using FitCRModels;
using FitCRModels.Enums;

namespace FitCRController
{
    /// <summary>Controller for the goals of the users. How each type of goal is reached is defined by the rules.</summary>
    public class GoalController : BaseController
    {
        private readonly BadgeController badgeController;
        private readonly List<IGoalRule> rules;

        public GoalController(InformationService informationService, BadgeController badgeController, IEnumerable<IGoalRule> rules)
            : base(informationService)
        {
            this.badgeController = badgeController;
            this.rules = rules.ToList();
        }

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

            if (this.GetRule(type) == null)
            {
                return OperationResult.Fail("This type of goal is not available.");
            }

            if (targetValue <= 0)
            {
                return OperationResult.Fail("The goal value must be greater than zero.");
            }

            if (deadline.Date <= DateTime.Today)
            {
                return OperationResult.Fail("The deadline must be a future date.");
            }

            var id = IdGenerator.Next("O", this.InformationService.Goals.Select(g => g.Id), 3);
            this.InformationService.Goals.Add(new Goal(id, userId, CsvFormat.Code(type), CsvFormat.Number(targetValue), CsvFormat.Date(deadline), bool.FalseString));
            this.InformationService.Save(this.InformationService.Goals);
            this.EvaluateGoals(userId);
            return OperationResult.Ok("Goal added.", id);
        }

        /// <summary>Checks the open goals of a user. The ones that were reached are marked and their badge is given. Returns the goals reached in this check.</summary>
        public List<Goal> EvaluateGoals(string userId)
        {
            var achieved = this.InformationService.Goals
                .Where(g => g.UserId == userId && !g.IsAchieved && g.Deadline.Date >= DateTime.Today)
                .Where(this.IsReached)
                .ToList();

            if (achieved.Count > 0)
            {
                achieved.ForEach(goal => goal.IsAchieved = true);
                this.InformationService.Save(this.InformationService.Goals);
                achieved.ForEach(goal => this.badgeController.AwardAchievementBadge(goal.UserId, this.GetRule(goal.GoalType)!.BadgeName));
            }

            return achieved;
        }

        private IGoalRule? GetRule(GoalType type)
        {
            return this.rules.FirstOrDefault(rule => rule.GoalType == type);
        }

        private bool IsReached(Goal goal)
        {
            return this.GetRule(goal.GoalType)?.IsReached(goal, this.InformationService) ?? false;
        }
    }
}
