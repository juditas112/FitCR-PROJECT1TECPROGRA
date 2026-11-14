using System.Globalization;

namespace FitCRModels
{
    /// <summary>
    /// Class to represent the user who uses the application to follow his routines.
    /// </summary>
    public class BasicUser : Person
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BasicUser"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="number">The number.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="role">The role.</param>
        /// <param name="birthDate">The birth date.</param>
        /// <param name="weight">The weight in kilograms.</param>
        /// <param name="height">The height in meters.</param>
        /// <param name="trainerId">The identifier of the trainer.</param>
        public BasicUser(string id, string name, string lastName, string email, string number, string username, string password, string role, string birthDate, string weight, string height, string trainerId)
            : base(id, name, lastName, email, number, username, password, role)
        {
            this.BirthDate = DateTime.Parse(birthDate, CultureInfo.InvariantCulture);
            this.Weight = double.Parse(weight, CultureInfo.InvariantCulture);
            this.Height = double.Parse(height, CultureInfo.InvariantCulture);
            this.TrainerId = trainerId;
        }

        /// <summary>
        /// Gets or sets the birth date.
        /// </summary>
        /// <value>
        /// The birth date.
        /// </value>
        public DateTime BirthDate { get; set; }

        /// <summary>
        /// Gets or sets the weight in kilograms.
        /// </summary>
        /// <value>
        /// The weight in kilograms.
        /// </value>
        public double Weight { get; set; }

        /// <summary>
        /// Gets or sets the height in meters.
        /// </summary>
        /// <value>
        /// The height in meters.
        /// </value>
        public double Height { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the trainer.
        /// </summary>
        /// <value>
        /// The identifier of the trainer.
        /// </value>
        public string TrainerId { get; set; } = string.Empty;

        /// <summary>
        /// Gets the age.
        /// </summary>
        /// <value>
        /// The age, calculated from the birth date.
        /// </value>
        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - this.BirthDate.Year;

                // Si todavia no ha cumplido anos este anio, se le resta uno.
                if (this.BirthDate.Date > today.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
        }

        /// <summary>
        /// Gets the body mass index (BMI).
        /// </summary>
        /// <value>
        /// The BMI, calculated as weight divided by height squared. Returns 0 if the height is not valid.
        /// </value>
        public double Bmi
        {
            get
            {
                if (this.Height <= 0)
                {
                    return 0;
                }

                return Math.Round(this.Weight / (this.Height * this.Height), 2);
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
            return $"{this.PersonToString()},{string.Join(",", new object[] { this.BirthDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), this.Weight.ToString(CultureInfo.InvariantCulture), this.Height.ToString(CultureInfo.InvariantCulture), this.TrainerId })}";
        }
    }
}
