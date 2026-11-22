using FitCRModels;

namespace FitCRController
{
    /// <summary>Controller for the login operations.</summary>
    public class LoginController : BaseController
    {
        public LoginController(InformationService informationService)
            : base(informationService)
        {
        }

        /// <summary>Logs in a user or a trainer with the username or the email. Returns true if the credentials are valid.</summary>
        public bool Login(string usernameOrEmail, string password)
        {
            if (string.IsNullOrWhiteSpace(usernameOrEmail) || string.IsNullOrWhiteSpace(password))
            {
                return false;
            }

            var login = usernameOrEmail.Trim();
            var people = new List<Person>();
            people.AddRange(this.InformationService.Users);
            people.AddRange(this.InformationService.Trainers);

            var person = people.FirstOrDefault(p =>
                (p.Username.Equals(login, StringComparison.OrdinalIgnoreCase) || p.Email.Equals(login, StringComparison.OrdinalIgnoreCase)) &&
                p.Password == password);

            this.InformationService.CurrentPerson = person;
            return person != null;
        }

        public void Logout()
        {
            this.InformationService.CurrentPerson = null;
        }
    }
}
