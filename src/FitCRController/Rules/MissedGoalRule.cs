using FitCRController.Abstractions;

namespace FitCRController.Rules
{
    /// <summary>Badge of shame for the users who did not reach a goal before its deadline.</summary>
    public class MissedGoalRule : IShameBadgeRule
    {
        public string BadgeName => "Objetivo no cumplido";

        public IEnumerable<string> GetUserIds(InformationService informationService)
        {
            return informationService.Goals
                .Where(g => !g.IsAchieved && g.Deadline.Date < DateTime.Today)
                .Select(g => g.UserId)
                .Distinct()
                .ToList();
        }
    }
}
