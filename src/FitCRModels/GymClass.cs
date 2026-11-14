using System.Globalization;
using FitCRModels.Enums;

namespace FitCRModels
{
    /// <summary>
    /// Class to represent a class offered by the gym.
    /// </summary>
    public class GymClass
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GymClass"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="classType">The type of class.</param>
        /// <param name="schedule">The schedule.</param>
        /// <param name="trainerId">The identifier of the trainer.</param>
        public GymClass(string id, string classType, string schedule, string trainerId)
        {
            this.Id = id;
            this.ClassType = (GymClassType)int.Parse(classType, CultureInfo.InvariantCulture);
            this.Schedule = schedule;
            this.TrainerId = trainerId;
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of class.
        /// </summary>
        /// <value>
        /// The type of class.
        /// </value>
        public GymClassType ClassType { get; set; }

        /// <summary>
        /// Gets or sets the schedule.
        /// </summary>
        /// <value>
        /// The schedule.
        /// </value>
        public string Schedule { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the trainer.
        /// </summary>
        /// <value>
        /// The identifier of the trainer.
        /// </value>
        public string TrainerId { get; set; } = string.Empty;

        /// <summary>
        /// Converts to string, using the same order of the columns of the file.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> with the line to save in the file.
        /// </returns>
        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, (int)this.ClassType, this.Schedule, this.TrainerId });
        }
    }
}
