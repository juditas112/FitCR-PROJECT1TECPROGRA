using System.Globalization;

namespace FitCRModels
{
    /// <summary>Class to represent a body measurement of a user.</summary>
    public class Measurement
    {
        public Measurement(string id, string userId, string date, string weight, string bodyFat, string muscleMass)
        {
            this.Id = id;
            this.UserId = userId;
            this.Date = DateTime.Parse(date, CultureInfo.InvariantCulture);
            this.Weight = double.Parse(weight, CultureInfo.InvariantCulture);
            this.BodyFat = double.Parse(bodyFat, CultureInfo.InvariantCulture);
            this.MuscleMass = double.Parse(muscleMass, CultureInfo.InvariantCulture);
        }

        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public double Weight { get; set; }

        public double BodyFat { get; set; }

        public double MuscleMass { get; set; }

        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.UserId, this.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), this.Weight.ToString(CultureInfo.InvariantCulture), this.BodyFat.ToString(CultureInfo.InvariantCulture), this.MuscleMass.ToString(CultureInfo.InvariantCulture) });
        }
    }
}
