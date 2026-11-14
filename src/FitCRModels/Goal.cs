using System.Globalization;
using FitCRModels.Enums;

namespace FitCRModels
{
    /// <summary>
    /// Class to represent a goal set by a user.
    /// </summary>
    public class Goal
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Goal"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="goalType">The type of goal.</param>
        /// <param name="targetValue">The target value.</param>
        /// <param name="deadline">The deadline.</param>
        /// <param name="isAchieved">The state of the goal (true if it was achieved).</param>
        public Goal(string id, string userId, string goalType, string targetValue, string deadline, string isAchieved)
        {
            this.Id = id;
            this.UserId = userId;
            this.GoalType = (GoalType)int.Parse(goalType, CultureInfo.InvariantCulture);
            this.TargetValue = double.Parse(targetValue, CultureInfo.InvariantCulture);
            this.Deadline = DateTime.Parse(deadline, CultureInfo.InvariantCulture);
            this.IsAchieved = bool.Parse(isAchieved);
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the user.
        /// </summary>
        /// <value>
        /// The identifier of the user.
        /// </value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of goal.
        /// </summary>
        /// <value>
        /// The type of goal.
        /// </value>
        public GoalType GoalType { get; set; }

        /// <summary>
        /// Gets or sets the target value.
        /// </summary>
        /// <value>
        /// The target value.
        /// </value>
        public double TargetValue { get; set; }

        /// <summary>
        /// Gets or sets the deadline.
        /// </summary>
        /// <value>
        /// The deadline.
        /// </value>
        public DateTime Deadline { get; set; }

        /// <summary>
        /// Gets or sets the state of the goal (true if it was achieved).
        /// </summary>
        /// <value>
        /// The state of the goal (true if it was achieved).
        /// </value>
        public bool IsAchieved { get; set; }

        /// <summary>
        /// Converts to string, using the same order of the columns of the file.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> with the line to save in the file.
        /// </returns>
        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.UserId, (int)this.GoalType, this.TargetValue.ToString(CultureInfo.InvariantCulture), this.Deadline.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), this.IsAchieved });
        }
    }
}
