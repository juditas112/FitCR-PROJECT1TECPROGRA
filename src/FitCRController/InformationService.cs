using FitCRController.Abstractions;
using FitCRModels;

namespace FitCRController
{
    /// <summary>Service for handling the information of the application. It loads the files once and shares the lists with the controllers.</summary>
    public class InformationService
    {
        public InformationService(IDataHandler dataHandler)
        {
            this.DataHandler = dataHandler;
            this.Users = this.Load<BasicUser>();
            this.Trainers = this.Load<Trainer>();
            this.GymClasses = this.Load<GymClass>();
            this.Routines = this.Load<Routine>();
            this.Exercises = this.Load<Exercise>();
            this.RoutineIncreases = this.Load<RoutineIncrease>();
            this.Visits = this.Load<Visit>();
            this.Measurements = this.Load<Measurement>();
            this.Goals = this.Load<Goal>();
            this.Badges = this.Load<Badge>();
            this.Notifications = this.Load<Notification>();
        }

        public Person? CurrentPerson { get; set; }

        public IDataHandler DataHandler { get; set; }

        /// <summary>Gets the errors found while the files were loaded.</summary>
        public List<string> LoadErrors { get; } = new List<string>();

        public List<BasicUser> Users { get; set; }

        public List<Trainer> Trainers { get; set; }

        public List<GymClass> GymClasses { get; set; }

        public List<Routine> Routines { get; set; }

        public List<Exercise> Exercises { get; set; }

        public List<RoutineIncrease> RoutineIncreases { get; set; }

        public List<Visit> Visits { get; set; }

        public List<Measurement> Measurements { get; set; }

        public List<Goal> Goals { get; set; }

        public List<Badge> Badges { get; set; }

        public List<Notification> Notifications { get; set; }

        /// <summary>Saves a list in its file.</summary>
        public bool Save<T>(List<T> data)
        {
            return this.DataHandler.SaveData(Configuration.GetFilePath<T>(), data);
        }

        /// <summary>Generates the next identifier for a type of element, for example U046 after U045.</summary>
        public string GenerateId(string prefix, IEnumerable<string> existingIds, int digits)
        {
            var max = 0;
            foreach (var id in existingIds)
            {
                if (id.StartsWith(prefix) && int.TryParse(id.Substring(prefix.Length), out var number) && number > max)
                {
                    max = number;
                }
            }

            return prefix + (max + 1).ToString("D" + digits);
        }

        private List<T> Load<T>()
        {
            var data = this.DataHandler.LoadData<T>(Configuration.GetFilePath<T>());
            this.LoadErrors.AddRange(this.DataHandler.Errors);
            return data;
        }
    }
}
