using System.Globalization;
using FitCRModels.Enums;

namespace FitCRModels
{
    
    /// Class to represent the trainers, who can be personal or from the gym.
    
    public class Trainer : Person
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Trainer"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="number">The number.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="role">The role.</param>
        /// <param name="trainerType">The type of trainer.</param>
        public Trainer(string id, string name, string lastName, string email, string number, string username, string password, string role, string trainerType)
            : base(id, name, lastName, email, number, username, password, role)
        {
            this.TrainerType = (TrainerType)int.Parse(trainerType, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the type of trainer.
        /// </summary>
        /// <value>
        /// The type of trainer.
        /// </value>
        public TrainerType TrainerType { get; set; }

        /// <summary>
        /// Converts to string, using the same order of the columns of the file.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> with the line to save in the file.
        /// </returns>
        public override string ToString()
        {
            return $"{this.PersonToString()},{string.Join(",", new object[] { (int)this.TrainerType })}";
        }
    }
}
