using System.Globalization;

namespace FitCRModels
{
    /// <summary>Class to represent an exercise that belongs to a routine.</summary>
    public class Exercise
    {
        public Exercise(string id, string routineId, string name, string sets, string repetitions, string weight)
        {
            this.Id = id;
            this.RoutineId = routineId;
            this.Name = name;
            this.Sets = int.Parse(sets, CultureInfo.InvariantCulture);
            this.Repetitions = int.Parse(repetitions, CultureInfo.InvariantCulture);
            this.Weight = double.Parse(weight, CultureInfo.InvariantCulture);
        }

        public string Id { get; set; }

        public string RoutineId { get; set; }

        public string Name { get; set; }

        public int Sets { get; set; }

        public int Repetitions { get; set; }

        public double Weight { get; set; }

        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.RoutineId, this.Name, this.Sets, this.Repetitions, this.Weight.ToString(CultureInfo.InvariantCulture) });
        }
    }
}
