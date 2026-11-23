using FitCRModels;

namespace FitCRController
{
    /// <summary>Controller for the body measurements of the users.</summary>
    public class MeasurementController : BaseController
    {
        private readonly GoalController goalController;

        public MeasurementController(InformationService informationService, GoalController goalController)
            : base(informationService)
        {
            this.goalController = goalController;
        }

        /// <summary>Gets the measurements of a user, from the newest to the oldest.</summary>
        public List<Measurement> GetMeasurements(string userId)
        {
            return this.InformationService.Measurements
                .Where(m => m.UserId == userId)
                .OrderByDescending(m => m.Date)
                .ToList();
        }

        /// <summary>Adds a measurement. If it is the newest one the weight of the user is updated, and then the goals are checked.</summary>
        public OperationResult AddMeasurement(string userId, DateTime date, double weight, double bodyFat, double muscleMass)
        {
            var notAllowed = this.EnsureTrainer();
            if (notAllowed != null)
            {
                return notAllowed;
            }

            var user = this.InformationService.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                return OperationResult.Fail("The user does not exist.");
            }

            var error = Validate(date, weight, bodyFat, muscleMass);
            if (error != null)
            {
                return OperationResult.Fail(error);
            }

            var isNewest = !this.InformationService.Measurements.Any(m => m.UserId == userId && m.Date.Date > date.Date);
            var id = IdGenerator.Next("M", this.InformationService.Measurements.Select(m => m.Id), 4);

            this.InformationService.Measurements.Add(new Measurement(id, userId, CsvFormat.Date(date), CsvFormat.Number(weight), CsvFormat.Number(bodyFat), CsvFormat.Number(muscleMass)));
            this.InformationService.Save(this.InformationService.Measurements);

            if (isNewest)
            {
                user.Weight = weight;
                this.InformationService.Save(this.InformationService.Users);
            }

            this.goalController.EvaluateGoals(userId);
            return OperationResult.Ok("Measurement added.", id);
        }

        private static string? Validate(DateTime date, double weight, double bodyFat, double muscleMass)
        {
            if (date.Date > DateTime.Today)
            {
                return "The date of the measurement cannot be in the future.";
            }

            var weightError = Validation.WeightError(weight);
            if (weightError != null)
            {
                return weightError;
            }

            if (bodyFat < 2 || bodyFat > 70)
            {
                return "The body fat must be between 2% and 70%.";
            }

            return muscleMass < 5 || muscleMass > 100 ? "The muscle mass must be between 5 and 100 kg." : null;
        }
    }
}
