using System.Globalization;
using FitCRModels.Enums;

namespace FitCRModels
{
    /// <summary>Class to represent a class offered by the gym.</summary>
    public class GymClass
    {
        public GymClass(string id, string classType, string day, string startTime, string endTime, string trainerId)
        {
            this.Id = id;
            this.ClassType = (GymClassType)int.Parse(classType, CultureInfo.InvariantCulture);
            this.Day = day;
            this.StartTime = TimeSpan.Parse(startTime, CultureInfo.InvariantCulture);
            this.EndTime = TimeSpan.Parse(endTime, CultureInfo.InvariantCulture);
            this.TrainerId = trainerId;
        }

        public string Id { get; set; }

        public GymClassType ClassType { get; set; }

        public string Day { get; set; }

        public TimeSpan StartTime { get; set; }

        public TimeSpan EndTime { get; set; }

        public string TrainerId { get; set; }

        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, (int)this.ClassType, this.Day, this.StartTime.ToString(@"hh\:mm", CultureInfo.InvariantCulture), this.EndTime.ToString(@"hh\:mm", CultureInfo.InvariantCulture), this.TrainerId });
        }
    }
}
