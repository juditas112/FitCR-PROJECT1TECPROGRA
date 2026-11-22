using FitCRModels;
using FitCRModels.Enums;
using System.Globalization;

namespace FitCRController
{
    /// <summary>Controller for the registration and the profile of the users, and for the trainers information.</summary>
    public class UserController
    {
        private const int MinimumAge = 12;
        private const int MaximumAge = 100;

        public UserController(InformationService informationService, NotificationController notificationController)
        {
            this.InformationService = informationService;
            this.NotificationController = notificationController;
        }

        public InformationService InformationService { get; set; }

        public NotificationController NotificationController { get; set; }

        /// <summary>Registers a new basic user. The trainer assigned is the first trainer of the gym.</summary>
        public OperationResult Register(string name, string lastName, string email, string number, string username, string password, DateTime birthDate, double weight, double height)
        {
            var error = this.ValidateRegistration(name, lastName, email, number, username, password, birthDate);
            if (error != null)
            {
                return OperationResult.Fail(error);
            }

            error = this.ValidateBody(weight, height);
            if (error != null)
            {
                return OperationResult.Fail(error);
            }

            var id = this.InformationService.GenerateId("U", this.InformationService.Users.Select(u => u.Id), 3);
            var trainer = this.InformationService.Trainers.FirstOrDefault(t => t.TrainerType == TrainerType.Gym);

            var user = new BasicUser(
                id,
                name.Trim(),
                lastName.Trim(),
                email.Trim(),
                number.Trim(),
                username.Trim(),
                password,
                ((int)Role.BasicUser).ToString(),
                birthDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture),
                weight.ToString(CultureInfo.InvariantCulture),
                height.ToString(CultureInfo.InvariantCulture),
                trainer?.Id ?? string.Empty);

            this.InformationService.Users.Add(user);
            this.InformationService.Save(this.InformationService.Users);
            this.NotificationController.Add(id, "Bienvenido a Fit.CR. Ya puedes empezar a registrar tus visitas.");
            return OperationResult.Ok("User registered.", id);
        }

        /// <summary>Updates the weight and the height of a user. The BMI is calculated again by the user itself.</summary>
        public OperationResult UpdateProfile(string userId, double weight, double height)
        {
            var user = this.GetUser(userId);
            if (user == null)
            {
                return OperationResult.Fail("The user does not exist.");
            }

            var error = this.ValidateBody(weight, height);
            if (error != null)
            {
                return OperationResult.Fail(error);
            }

            user.Weight = weight;
            user.Height = height;
            this.InformationService.Save(this.InformationService.Users);
            return OperationResult.Ok("Profile updated.");
        }

        /// <summary>Gets a user.</summary>
        public BasicUser? GetUser(string userId)
        {
            return this.InformationService.Users.FirstOrDefault(u => u.Id == userId);
        }

        /// <summary>Gets a trainer.</summary>
        public Trainer? GetTrainer(string trainerId)
        {
            return this.InformationService.Trainers.FirstOrDefault(t => t.Id == trainerId);
        }

        /// <summary>Gets the users that a trainer takes care of.</summary>
        public List<BasicUser> GetUsersOfTrainer(string trainerId)
        {
            return this.InformationService.Users
                .Where(u => u.TrainerId == trainerId)
                .OrderBy(u => u.Name)
                .ThenBy(u => u.LastName)
                .ToList();
        }

        private string? ValidateRegistration(string name, string lastName, string email, string number, string username, string password, DateTime birthDate)
        {
            var texts = new[] { name, lastName, email, number, username, password };
            if (texts.Any(string.IsNullOrWhiteSpace))
            {
                return "All the fields are required.";
            }

            if (texts.Any(t => t.Contains(',')))
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

            if (!this.IsValidNumber(number))
            {
                return "The phone number is not valid.";
            }

            var age = DateTime.Today.Year - birthDate.Year;
            if (birthDate.Date > DateTime.Today.AddYears(-age))
            {
                age--;
            }

            return age < MinimumAge || age > MaximumAge ? $"The age must be between {MinimumAge} and {MaximumAge} years." : null;
        }

        private string? ValidateBody(double weight, double height)
        {
            if (weight < 20 || weight > 300)
            {
                return "The weight must be between 20 and 300 kg.";
            }

            return height < 1.0 || height > 2.5 ? "The height must be between 1.0 and 2.5 meters." : null;
        }

        private bool UsernameExists(string username)
        {
            return this.InformationService.Users.Any(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase)) ||
                   this.InformationService.Trainers.Any(t => t.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        }

        private bool IsValidNumber(string number)
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
    }
}
