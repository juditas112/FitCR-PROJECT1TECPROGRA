using System.Globalization;

namespace FitCRModels
{
    /// <summary>
    /// Class to represent a routine assigned to a user by a trainer.
    /// </summary>
    public class Routine
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Routine"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="trainerId">The identifier of the trainer.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        public Routine(string id, string userId, string trainerId, string startDate, string endDate)
        {
            this.Id = id;
            this.UserId = userId;
            this.TrainerId = trainerId;
            this.StartDate = DateTime.Parse(startDate, CultureInfo.InvariantCulture);
            this.EndDate = DateTime.Parse(endDate, CultureInfo.InvariantCulture);
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
        /// Gets or sets the identifier of the trainer.
        /// </summary>
        /// <value>
        /// The identifier of the trainer.
        /// </value>
        public string TrainerId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the start date.
        /// </summary>
        /// <value>
        /// The start date.
        /// </value>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// Gets or sets the end date.
        /// </summary>
        /// <value>
        /// The end date.
        /// </value>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// Gets a value indicating whether this routine has expired.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the end date is before today; otherwise, <c>false</c>.
        /// </value>
        public bool IsExpired
        {
            get
            {
                return this.EndDate.Date < DateTime.Today;
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
            return string.Join(",", new object[] { this.Id, this.UserId, this.TrainerId, this.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), this.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) });
        }
    }
}
