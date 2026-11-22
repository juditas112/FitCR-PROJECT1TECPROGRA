using System.Globalization;

namespace FitCRModels
{
    /// <summary>Class to represent the user who uses the application to follow his routines.</summary>
    public class BasicUser : Person
    {
        public BasicUser(string id, string name, string lastName, string email, string number, string username, string password, string role, string birthDate, string weight, string height, string trainerId)
            : base(id, name, lastName, email, number, username, password, role)
        {
            this.BirthDate = DateTime.Parse(birthDate, CultureInfo.InvariantCulture);
            this.Weight = double.Parse(weight, CultureInfo.InvariantCulture);
            this.Height = double.Parse(height, CultureInfo.InvariantCulture);
            this.TrainerId = trainerId;
        }

        public DateTime BirthDate { get; set; }

        public double Weight { get; set; }

        public double Height { get; set; }

        public string TrainerId { get; set; }

        public int Age
        {
            get
            {
                var today = DateTime.Today;
                var age = today.Year - this.BirthDate.Year;

                if (this.BirthDate.Date > today.AddYears(-age))
                {
                    age--;
                }

                return age;
            }
        }

        public double Bmi
        {
            get
            {
                if (this.Height <= 0)
                {
                    return 0;
                }

                return Math.Round(this.Weight / (this.Height * this.Height), 2);
            }
        }

        public override string ToString()
        {
            return $"{this.PersonToString()},{string.Join(",", new object[] { this.BirthDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), this.Weight.ToString(CultureInfo.InvariantCulture), this.Height.ToString(CultureInfo.InvariantCulture), this.TrainerId })}";
        }
    }
}
