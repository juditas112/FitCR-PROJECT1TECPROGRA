using System.Globalization;
using FitCRModels.Enums;

namespace FitCRModels
{
    /// <summary>Class to represent a badge earned by a user, it can be an achievement or a badge of shame.</summary>
    public class Badge
    {
        public Badge(string id, string userId, string badgeType, string name, string date)
        {
            this.Id = id;
            this.UserId = userId;
            this.BadgeType = (BadgeType)int.Parse(badgeType, CultureInfo.InvariantCulture);
            this.Name = name;
            this.Date = DateTime.Parse(date, CultureInfo.InvariantCulture);
        }

        public string Id { get; set; } = string.Empty;

        public string UserId { get; set; } = string.Empty;

        public BadgeType BadgeType { get; set; }

        public string Name { get; set; } = string.Empty;

        public DateTime Date { get; set; }

        public override string ToString()
        {
            return string.Join(",", new object[] { this.Id, this.UserId, (int)this.BadgeType, this.Name, this.Date.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture) });
        }
    }
}
