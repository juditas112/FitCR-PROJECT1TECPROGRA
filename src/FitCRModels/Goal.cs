using System.Globalization;
using FitCRModels.Enums;

namespace FitCRModels
{
    /// <summary>Class to represent a goal set by a user.</summary>
    public class Goal
    {
        public Goal(string id, string userId, string goalType, string targetValue, string deadline, string isAchieved)
        {
            this.Id = id;
            this.UserId = userId;
            this.GoalType = (GoalType)int.Parse(goalType, CultureInfo.InvariantCulture);
            this.TargetValue = double.Parse(targetValue, CultureInfo.InvariantCulture);
            this.Deadline = DateTime.Parse(deadline, CultureInfo.InvariantCulture);
            this.IsAchieved = bool.Parse(isAchieved);
        }

        public string Id { get; set; }

        public string UserId { get; set; }

        public GoalType GoalType { get; set; }

        public double TargetValue { get; set; }

        public DateTime Deadline { get; set; }

        public bool IsAchieved { get; set; }

        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.UserId, (int)this.GoalType, this.TargetValue.ToString(CultureInfo.InvariantCulture), this.Deadline.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), this.IsAchieved });
        }
    }
}
