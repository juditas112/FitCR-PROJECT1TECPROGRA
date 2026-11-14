using System.Globalization;

namespace FitCRModels
{
    /// <summary>
    /// Class to represent a visit of a user to the gym.
    /// </summary>
    public class Visit
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Visit"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="date">The date of the visit.</param>
        public Visit(string id, string userId, string date)
        {
            this.Id = id;
            this.UserId = userId;
            this.Date = DateTime.Parse(date, CultureInfo.InvariantCulture);
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
        /// Gets or sets the date of the visit.
        /// </summary>
        /// <value>
        /// The date of the visit.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Converts to string, using the same order of the columns of the file.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> with the line to save in the file.
        /// </returns>
        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.UserId, this.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) });
        }
    }
}
