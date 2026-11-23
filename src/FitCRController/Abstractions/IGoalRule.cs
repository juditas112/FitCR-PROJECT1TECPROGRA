using FitCRModels;
using FitCRModels.Enums;

namespace FitCRController.Abstractions
{
    /// <summary>Rule that decides if a goal of one type was reached. A new type of goal only needs a new rule.</summary>
    public interface IGoalRule
    {
        public GoalType GoalType { get; }

        public string DisplayName { get; }

        public string BadgeName { get; }

        public bool IsReached(Goal goal, InformationService informationService);
    }
}
