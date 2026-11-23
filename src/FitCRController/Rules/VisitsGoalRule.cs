using FitCRController.Abstractions;
using FitCRModels;
using FitCRModels.Enums;

namespace FitCRController.Rules
{
    /// <summary>Goal of visits: it is reached when the user has the target amount of visits before the deadline.</summary>
    public class VisitsGoalRule : IGoalRule
    {
        public GoalType GoalType => GoalType.Visits;

        public string DisplayName => "Visitas al gimnasio";

        public string BadgeName => "Meta de visitas cumplida";

        public bool IsReached(Goal goal, InformationService informationService)
        {
            var visits = informationService.Visits.Count(v => v.UserId == goal.UserId && v.Date.Date <= goal.Deadline.Date);
            return visits >= goal.TargetValue;
        }
    }
}
