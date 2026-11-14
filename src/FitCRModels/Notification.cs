using System.Globalization;

namespace FitCRModels
{
    /// <summary>
    /// Class to represent a notification of the timeline of a user or a trainer.
    /// </summary>
    public class Notification
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Notification"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="recipientId">The identifier of the person who receives it.</param>
        /// <param name="message">The message.</param>
        /// <param name="date">The date.</param>
        public Notification(string id, string recipientId, string message, string date)
        {
            this.Id = id;
            this.RecipientId = recipientId;
            this.Message = message;
            this.Date = DateTime.Parse(date, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the identifier of the person who receives it.
        /// </summary>
        /// <value>
        /// The identifier of the person who receives it.
        /// </value>
        public string RecipientId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date.
        /// </summary>
        /// <value>
        /// The date.
        /// </value>
        public DateTime Date { get; set; }

        /// <summary>
        /// Converts to string, using the same order of the columns of the file.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> with the line to save in the file.
        /// </returns>
        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.RecipientId, this.Message, this.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) });
        }
    }
}
