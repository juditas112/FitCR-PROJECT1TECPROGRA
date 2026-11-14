using System.Globalization;
using FitCRModels.Enums;

namespace FitCRModels
{
    /// <summary>
    /// Class to represent a badge earned by a user, it can be an achievement or a badge of shame.
    /// </summary>
    public class Badge
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Badge"/> class.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="userId">The identifier of the user.</param>
        /// <param name="badgeType">The type of badge.</param>
        /// <param name="name">The name.</param>
        /// <param name="date">The date when it was earned.</param>
        public Badge(string id, string userId, string badgeType, string name, string date)
        {
            this.Id = id;
            this.UserId = userId;
            this.BadgeType = (BadgeType)int.Parse(badgeType, CultureInfo.InvariantCulture);
            this.Name = name;
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
        /// Gets or sets the identifier of the user.
        /// </summary>
        /// <value>
        /// The identifier of the user.
        /// </value>
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the type of badge.
        /// </summary>
        /// <value>
        /// The type of badge.
        /// </value>
        public BadgeType BadgeType { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the date when it was earned.
        /// </summary>
        /// <value>
        /// The date when it was earned.
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
            return string.Join(",", new object[] { this.Id, this.UserId, (int)this.BadgeType, this.Name, this.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) });
        }
    }
}
