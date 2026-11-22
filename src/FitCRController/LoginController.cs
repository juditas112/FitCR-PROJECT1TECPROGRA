using FitCRModels;

namespace FitCRController
{
    /// <summary>Controller for the login operations.</summary>
    public class LoginController
    {
        public LoginController(InformationService informationService)
        {
            this.InformationService = informationService;
        }

        public InformationService InformationService { get; set; }

        /// <summary>Logs in a user or a trainer.</summary>
        public bool Login(string usernameOrEmail, string password)
        {
            if (string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var people = new List<Person>();
            people.AddRange(this.InformationService.Users);
            people.AddRange(this.InformationService.Trainers);

            var person = people.FirstOrDefault(p =>
                (p.Username.Equals(usernameOrEmail.Trim(), StringComparison.OrdinalIgnoreCase) ||
                 p.Email.Equals(usernameOrEmail.Trim(), StringComparison.OrdinalIgnoreCase)) &&
                p.Password == password);

            this.InformationService.CurrentPerson = person;
            return person != null;
        }

        /// <summary>Logs out the current person.</summary>
        public void Logout()
        {
            this.InformationService.CurrentPerson = null;
        }
    }
}
