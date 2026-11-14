using FitCRModels.Enums;
using System.Globalization;

namespace FitCRModels
{
    /// <summary>
    /// Abstraction of a person, used as a base class for the users and the trainers of the gym.
    /// </summary>
    public abstract class Person
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Person"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="name">The name.</param>
        /// <param name="lastName">The last name.</param>
        /// <param name="email">The email.</param>
        /// <param name="number">The phone number.</param>
        /// <param name="username">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="role">The role.</param>
        protected Person(string id, string name, string lastName, string email, string number, string username, string password, string role)
        {
            this.Id = id;
            this.Name = name;
            this.LastName = lastName;
            this.Email = email;
            this.Number = new PhoneNumber(number);
            this.Username = username;
            this.Password = password;
            this.Role = (Role)int.Parse(role, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the last name.
        /// </summary>
        /// <value>
        /// The last name.
        /// </value>
        public string LastName { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the email.
        /// </summary>
        /// <value>
        /// The email.
        /// </value>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the phone number.
        /// </summary>
        /// <value>
        /// The phone number.
        /// </value>
        public PhoneNumber Number { get; set; }

        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        public string Username { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        public string Password { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the role.
        /// </summary>
        /// <value>
        /// The role.
        /// </value>
        public Role Role { get; set; }

        /// <summary>
        /// Builds the part of the file line that is shared by all the people.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> with the common columns separated by commas.
        /// </returns>
        protected string PersonToString()
        {
            return string.Join(",", this.Id, this.Name, this.LastName, this.Email, this.Number, this.Username, this.Password, (int)this.Role);
        }
    }
}
