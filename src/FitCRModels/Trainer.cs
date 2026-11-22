using System.Globalization;
using FitCRModels.Enums;

namespace FitCRModels
{
    /// <summary>Class to represent the trainers, who can be personal or from the gym.</summary>
    public class Trainer : Person
    {
        public Trainer(string id, string name, string lastName, string email, string number, string username, string password, string role, string trainerType)
            : base(id, name, lastName, email, number, username, password, role)
        {
            this.TrainerType = (TrainerType)int.Parse(trainerType, CultureInfo.InvariantCulture);
        }

        public TrainerType TrainerType { get; set; }

        public override string ToString()
        {
            return $"{this.PersonToString()},{string.Join(",", new object[] { (int)this.TrainerType })}";
        }
    }
}
