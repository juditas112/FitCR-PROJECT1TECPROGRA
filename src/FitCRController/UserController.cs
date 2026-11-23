using FitCRModels;
using FitCRModels.Enums;

namespace FitCRController
{
    /// <summary>Controller for the registration and the profile of the users, and for the information of the trainers.</summary>
    public class UserController : BaseController
    {
        private const int MinimumAge = 12;
        private const int MaximumAge = 100;

        private readonly NotificationController notificationController;

        public UserController(InformationService informationService, NotificationController notificationController)
            : base(informationService)
        {
            this.notificationController = notificationController;
        }

        /// <summary>Registers a new basic user. The trainer assigned is the first trainer of the gym. If it succeded, the new identifier is in the result.</summary>
        public OperationResult Register(string name, string lastName, string email, string number, string username, string password, DateTime birthDate, double weight, double height)
        {
            var error = this.ValidateRegistration(name, lastName, email, number, username, password, birthDate) ?? ValidateBody(weight, height);
            if (error != null)
            {
                return OperationResult.Fail(error);
            }

            var id = IdGenerator.Next("U", this.InformationService.Users.Select(u => u.Id), 3);
            var trainer = this.InformationService.Trainers.FirstOrDefault(t => t.TrainerType == TrainerType.Gym);

            this.InformationService.Users.Add(new BasicUser(
                id,
                name.Trim(),
                lastName.Trim(),
                email.Trim(),
                number.Trim(),
                username.Trim(),
                password,
                CsvFormat.Code(Role.BasicUser),
                CsvFormat.Date(birthDate),
                CsvFormat.Number(weight),
                CsvFormat.Number(height),
                trainer?.Id ?? string.Empty));

            this.InformationService.Save(this.InformationService.Users);
            this.notificationController.Add(id, "Bienvenido a Fit.CR. Ya puedes empezar a registrar tus visitas.");
            return OperationResult.Ok("User registered.", id);
        }

        public BasicUser? GetUser(string userId)
        {
            return this.InformationService.Users.FirstOrDefault(u => u.Id == userId);
        }

        public Trainer? GetTrainer(string trainerId)
        {
            return this.InformationService.Trainers.FirstOrDefault(t => t.Id == trainerId);
        }

        /// <summary>Gets the users that a trainer takes care of, sorted by name.</summary>
        public List<BasicUser> GetUsersOfTrainer(string trainerId)
        {
            return this.InformationService.Users
                .Where(u => u.TrainerId == trainerId)
                .OrderBy(u => u.Name)
                .ThenBy(u => u.LastName)
                .ToList();
        }

        private static string? ValidateBody(double weight, double height)
        {
            return Validation.WeightError(weight) ?? Validation.HeightError(height);
        }

        private static bool IsValidNumber(string number)
        {
            try
            {
                _ = new PhoneNumber(number);
                return true;
            }
            catch (Exception ex) when (ex is FormatException || ex is OverflowException || ex is IndexOutOfRangeException)
            {
                return false;
            }
        }

        private static string? ValidateAge(DateTime birthDate)
        {
            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            return age < MinimumAge || age > MaximumAge ? $"The age must be between {MinimumAge} and {MaximumAge} years." : null;
        }

        private string? ValidateRegistration(string name, string lastName, string email, string number, string username, string password, DateTime birthDate)
        {
            var texts = new[] { name, lastName, email, number, username, password };
            if (texts.Any(string.IsNullOrWhiteSpace))
            {
                return "All the fields are required.";
            }

            if (Validation.HasComma(texts))
            {
                return "The fields cannot contain commas.";
            }

            if (!email.Contains('@') || !email.Contains('.'))
            {
                return "The email is not valid.";
            }

            if (username.Trim().Length < 3 || username.Contains(' '))
            {
                return "The username needs at least 3 characters and no spaces.";
            }

            if (password.Length < 4)
            {
                return "The password needs at least 4 characters.";
            }

            if (this.UsernameExists(username.Trim()))
            {
                return "The username is already taken.";
            }

            return IsValidNumber(number) ? ValidateAge(birthDate) : "The phone number is not valid.";
        }

        private bool UsernameExists(string username)
        {
            return this.InformationService.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)) ||
                   this.InformationService.Trainers.Any(t => t.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }
    }
}
