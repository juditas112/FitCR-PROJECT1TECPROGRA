using System.Globalization;
using FitCRModels.Enums;

namespace FitCRModels
{
    /// <summary>Class to represent a class offered by the gym.</summary>
    public class GymClass
    {
        public GymClass(string id, string classType, string schedule, string trainerId)
        {
            this.Id = id;
            this.ClassType = (GymClassType)int.Parse(classType, CultureInfo.InvariantCulture);
            this.Schedule = schedule;
            this.TrainerId = trainerId;
        }

        public string Id { get; set; }

        public GymClassType ClassType { get; set; }

        public string Schedule { get; set; }

        public string TrainerId { get; set; }

        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, (int)this.ClassType, this.Schedule, this.TrainerId });
        }
    }
}
