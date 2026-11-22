using FitCRController.Abstractions;
using FitCRModels;

namespace FitCRController
{
    /// <summary>Controller for the notifications timeline of the users and the trainers.</summary>
    public class NotificationController : BaseController
    {
        private readonly List<ITrainerNotificationRule> rules;

        public NotificationController(InformationService informationService, IEnumerable<ITrainerNotificationRule> rules)
            : base(informationService)
        {
            this.rules = rules.ToList();
        }

        /// <summary>Gets the timeline of a person, from the newest notification to the oldest.</summary>
        public List<Notification> GetNotifications(string recipientId)
        {
            return this.InformationService.Notifications
                .Where(n => n.RecipientId == recipientId)
                .OrderByDescending(n => n.Date)
                .ThenByDescending(n => n.Id)
                .ToList();
        }

        /// <summary>Adds a notification to the timeline of a person and saves it.</summary>
        public void Add(string recipientId, string message)
        {
            this.AddWithoutSaving(recipientId, message);
            this.InformationService.Save(this.InformationService.Notifications);
        }

        /// <summary>Creates the notices of a trainer using the rules. A notice is not created again if it already exists. Returns how many were created.</summary>
        public int GenerateTrainerNotifications(string trainerId)
        {
            var created = 0;
            foreach (var message in this.rules.SelectMany(rule => rule.GetMessages(trainerId, this.InformationService)))
            {
                created += this.AddIfMissing(trainerId, message) ? 1 : 0;
            }

            if (created > 0)
            {
                this.InformationService.Save(this.InformationService.Notifications);
            }

            return created;
        }

        private static string CleanMessage(string message)
        {
            return message.Replace(',', ';');
        }

        private bool AddIfMissing(string recipientId, string message)
        {
            var clean = CleanMessage(message);
            if (this.InformationService.Notifications.Any(n => n.RecipientId == recipientId && n.Message == clean))
            {
                return false;
            }

            this.AddWithoutSaving(recipientId, clean);
            return true;
        }

        private void AddWithoutSaving(string recipientId, string message)
        {
            var id = IdGenerator.Next("N", this.InformationService.Notifications.Select(n => n.Id), 4);
            this.InformationService.Notifications.Add(new Notification(id, recipientId, CleanMessage(message), CsvFormat.Date(DateTime.Today)));
        }
    }
}
