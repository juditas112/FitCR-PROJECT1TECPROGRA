using System.Globalization;

namespace FitCRModels
{
    /// <summary>Class to represent a routine assigned to a user by a trainer.</summary>
    public class Routine
    {
        public Routine(string id, string userId, string trainerId, string startDate, string endDate)
        {
            this.Id = id;
            this.UserId = userId;
            this.TrainerId = trainerId;
            this.StartDate = DateTime.Parse(startDate, CultureInfo.InvariantCulture);
            this.EndDate = DateTime.Parse(endDate, CultureInfo.InvariantCulture);
        }

        public string Id { get; set; }

        public string UserId { get; set; }

        public string TrainerId { get; set; }

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }

        public bool IsExpired
        {
            get
            {
                return this.EndDate.Date < DateTime.Today;
            }
        }

        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.UserId, this.TrainerId, this.StartDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture), this.EndDate.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) });
        }
    }
}
