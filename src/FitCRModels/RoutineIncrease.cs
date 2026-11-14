using System.Globalization;

namespace FitCRModels
{
    /// <summary>
    /// Class to represent an increase made to an exercise of a routine.
    /// </summary>
    public class RoutineIncrease
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RoutineIncrease"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="routineId">The identifier of the routine.</param>
        /// <param name="exerciseId">The identifier of the exercise.</param>
        /// <param name="date">The date.</param>
        /// <param name="previousWeight">The previous weight.</param>
        /// <param name="newWeight">The new weight.</param>
        public RoutineIncrease(string id, string routineId, string exerciseId, string date, string previousWeight, string newWeight)
        {
            this.Id = id;
            this.RoutineId = routineId;
            this.ExerciseId = exerciseId;
            this.Date = DateTime.Parse(date, CultureInfo.InvariantCulture);
            this.PreviousWeight = double.Parse(previousWeight, CultureInfo.InvariantCulture);
            this.NewWeight = double.Parse(newWeight, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the routine.
        /// </summary>
        /// <value>
        /// The identifier of the routine.
        /// </value>
        public string RoutineId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the exercise.
        /// </summary>
        /// <value>
        /// The identifier of the exercise.
        /// </value>
        public string ExerciseId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the previous weight.
        /// </summary>
        /// <value>
        /// The previous weight.
        /// </value>
        public double PreviousWeight { get; set; }

        /// <summary>
        /// Gets or sets the new weight.
        /// </summary>
        /// <value>
        /// The new weight.
        /// </value>
        public double NewWeight { get; set; }

        /// <summary>
        /// Gets the increase.
        /// </summary>
        /// <value>
        /// The difference between the new weight and the previous one.
        /// </value>
        public double Increase
        {
            get
            {
                return this.NewWeight - this.PreviousWeight;
            }
        }

        /// <summary>
        /// Converts to string, using the same order of the columns of the file.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> with the line to save in the file.
        /// </returns>
        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.RoutineId, this.ExerciseId, this.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), this.PreviousWeight.ToString(CultureInfo.InvariantCulture), this.NewWeight.ToString(CultureInfo.InvariantCulture) });
        }
    }
}
