using System.Globalization;

namespace FitCRModels
{
    /// <summary>
    /// Class to represent a body measurement of a user.
    /// </summary>
    public class Measurement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Measurement"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="date">The date of the measurement.</param>
        /// <param name="weight">The weight in kilograms.</param>
        /// <param name="bodyFat">The body fat percentage.</param>
        /// <param name="muscleMass">The muscle mass in kilograms.</param>
        public Measurement(string id, string userId, string date, string weight, string bodyFat, string muscleMass)
        {
            this.Id = id;
            this.UserId = userId;
            this.Date = DateTime.Parse(date, CultureInfo.InvariantCulture);
            this.Weight = double.Parse(weight, CultureInfo.InvariantCulture);
            this.BodyFat = double.Parse(bodyFat, CultureInfo.InvariantCulture);
            this.MuscleMass = double.Parse(muscleMass, CultureInfo.InvariantCulture);
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
        /// Gets or sets the date of the measurement.
        /// </summary>
        /// <value>
        /// The date of the measurement.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Gets or sets the weight in kilograms.
        /// </summary>
        /// <value>
        /// The weight in kilograms.
        /// </value>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the body fat percentage.
        /// </summary>
        /// <value>
        /// The body fat percentage.
        /// </value>
        public double BodyFat { get; set; }

        /// <summary>
        /// Gets or sets the muscle mass in kilograms.
        /// </summary>
        /// <value>
        /// The muscle mass in kilograms.
        /// </value>
        public double MuscleMass { get; set; }

        /// <summary>
        /// Converts to string, using the same order of the columns of the file.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> with the line to save in the file.
        /// </returns>
        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.UserId, this.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), this.Weight.ToString(CultureInfo.InvariantCulture), this.BodyFat.ToString(CultureInfo.InvariantCulture), this.MuscleMass.ToString(CultureInfo.InvariantCulture) });
        }
    }
}
