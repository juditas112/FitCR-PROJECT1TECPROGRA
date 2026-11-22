using FitCRModels.Enums;
using System.Globalization;

namespace FitCRModels
{
    /// <summary>Abstraction of a person, used as a base class for the users and the trainers of the gym.</summary>
    public abstract class Person
    {
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

        public string Id { get; set; }

        public string Name { get; set; }

        public string LastName { get; set; }

        public string Email { get; set; }

        public PhoneNumber Number { get; set; }

        public string Username { get; set; }

        public string Password { get; set; }

        public Role Role { get; set; }

        /// <summary>Builds the part of the file line that is shared by all the people.</summary>
        protected string PersonToString()
        {
            return string.Join(",", this.Id, this.Name, this.LastName, this.Email, this.Number, this.Username, this.Password, (int)this.Role);
        }
    }
}
