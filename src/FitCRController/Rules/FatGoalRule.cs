using FitCRController.Abstractions;
using FitCRModels;
using FitCRModels.Enums;

namespace FitCRController.Rules
{
    /// <summary>Goal of body fat: it is reached when the last measurement has the target or less.</summary>
    public class FatGoalRule : IGoalRule
    {
        public GoalType GoalType => GoalType.Fat;

        public string BadgeName => "Meta de grasa cumplida";

        public bool IsReached(Goal goal, InformationService informationService)
        {
            var last = informationService.GetLastMeasurement(goal.UserId);
            return last != null && last.BodyFat <= goal.TargetValue;
        }
    }
}
