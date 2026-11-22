using System.Globalization;

namespace FitCRModels
{
    /// <summary>Class to represent a notification of the timeline of a user or a trainer.</summary>
    public class Notification
    {
        public Notification(string id, string recipientId, string message, string date)
        {
            this.Id = id;
            this.RecipientId = recipientId;
            this.Message = message;
            this.Date = DateTime.Parse(date, CultureInfo.InvariantCulture);
        }

        public string Id { get; set; }

        public string RecipientId { get; set; }

        public string Message { get; set; }

        public DateTime Date { get; set; }

        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.RecipientId, this.Message, this.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) });
        }
    }
}
