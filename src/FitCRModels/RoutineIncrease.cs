using System.Globalization;

namespace FitCRModels
{
    /// <summary>Class to represent an increase made to an exercise of a routine.</summary>
    public class RoutineIncrease
    {
        public RoutineIncrease(string id, string routineId, string exerciseId, string date, string previousWeight, string newWeight)
        {
            this.Id = id;
            this.RoutineId = routineId;
            this.ExerciseId = exerciseId;
            this.Date = DateTime.Parse(date, CultureInfo.InvariantCulture);
            this.PreviousWeight = double.Parse(previousWeight, CultureInfo.InvariantCulture);
            this.NewWeight = double.Parse(newWeight, CultureInfo.InvariantCulture);
        }

        public string Id { get; set; } = string.Empty;

        public string RoutineId { get; set; } = string.Empty;

        public string ExerciseId { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public double PreviousWeight { get; set; }

        public double NewWeight { get; set; }

        public double Increase
        {
            get
            {
                return this.NewWeight - this.PreviousWeight;
            }
        }

        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.RoutineId, this.ExerciseId, this.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), this.PreviousWeight.ToString(CultureInfo.InvariantCulture), this.NewWeight.ToString(CultureInfo.InvariantCulture) });
        }
    }
}
