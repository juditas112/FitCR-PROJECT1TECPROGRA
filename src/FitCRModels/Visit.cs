using System.Globalization;

namespace FitCRModels
{
    /// <summary>Class to represent a visit of a user to the gym.</summary>
    public class Visit
    {
        public Visit(string id, string userId, string date)
        {
            this.Id = id;
            this.UserId = userId;
            this.Date = DateTime.Parse(date, CultureInfo.InvariantCulture);
        }

        public string Id { get; set; }

        public string UserId { get; set; }

        public DateTime Date { get; set; }

        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.UserId, this.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) });
        }
    }
}
