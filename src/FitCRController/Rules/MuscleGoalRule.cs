using FitCRController.Abstractions;
using FitCRModels;
using FitCRModels.Enums;

namespace FitCRController.Rules
{
    /// <summary>Goal of muscle mass: it is reached when the last measurement has at least the target.</summary>
    public class MuscleGoalRule : IGoalRule
    {
        public GoalType GoalType => GoalType.Muscle;

        public string BadgeName => "Meta de musculatura cumplida";

        public bool IsReached(Goal goal, InformationService informationService)
        {
            var last = informationService.GetLastMeasurement(goal.UserId);
            return last != null && last.MuscleMass >= goal.TargetValue;
        }
    }
}
