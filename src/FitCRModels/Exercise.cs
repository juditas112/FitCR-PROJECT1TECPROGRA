using System.Globalization;

namespace FitCRModels
{
    /// <summary>
    /// Class to represent an exercise that belongs to a routine.
    /// </summary>
    public class Exercise
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Exercise"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="routineId">The identifier of the routine.</param>
        /// <param name="name">The name.</param>
        /// <param name="sets">The number of sets.</param>
        /// <param name="repetitions">The number of repetitions.</param>
        /// <param name="weight">The weight in kilograms.</param>
        public Exercise(string id, string routineId, string name, string sets, string repetitions, string weight)
        {
            this.Id = id;
            this.RoutineId = routineId;
            this.Name = name;
            this.Sets = int.Parse(sets, CultureInfo.InvariantCulture);
            this.Repetitions = int.Parse(repetitions, CultureInfo.InvariantCulture);
            this.Weight = double.Parse(weight, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the routine.
        /// </summary>
        /// <value>
        /// The identifier of the routine.
        /// </value>
        public string RoutineId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the number of sets.
        /// </summary>
        /// <value>
        /// The number of sets.
        /// </value>
        public int Sets { get; set; }

        /// <summary>
        /// Gets or sets the number of repetitions.
        /// </summary>
        /// <value>
        /// The number of repetitions.
        /// </value>
        public int Repetitions { get; set; }

        /// <summary>
        /// Gets or sets the weight in kilograms.
        /// </summary>
        /// <value>
        /// The weight in kilograms.
        /// </value>
        public double Weight { get; set; }

        /// <summary>
        /// Converts to string, using the same order of the columns of the file.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> with the line to save in the file.
        /// </returns>
        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.RoutineId, this.Name, this.Sets, this.Repetitions, this.Weight.ToString(CultureInfo.InvariantCulture) });
        }
    }
}
