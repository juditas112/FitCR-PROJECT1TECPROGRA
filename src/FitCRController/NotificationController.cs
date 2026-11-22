using FitCRModels;

namespace FitCRController
{
    /// <summary>Controller for the notifications timeline of the users and the trainers.</summary>
    public class NotificationController
    {
        private const int ExpirationWarningDays = 7;
        private const int MeasurementIntervalDays = 30;
        private const int MeasurementWarningDays = 5;

        public NotificationController(InformationService informationService)
        {
            this.InformationService = informationService;
        }

        public InformationService InformationService { get; set; }

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

        /// <summary>Creates the notifications of a trainer: routines that are about to expire and measurements that are close. A notification is not created again if the same message already exists.</summary>
        public int GenerateTrainerNotifications(string trainerId)
        {
            var today = DateTime.Today;
            var created = 0;

            var expiring = this.InformationService.Routines
                .Where(r => r.TrainerId == trainerId && r.EndDate.Date >= today && r.EndDate.Date <= today.AddDays(ExpirationWarningDays));

            foreach (var routine in expiring)
            {
                var user = this.InformationService.Users.FirstOrDefault(u => u.Id == routine.UserId);
                if (user != null)
                {
                    var message = $"La rutina de {user.Name} {user.LastName} vence el {routine.EndDate:yyyy-MM-dd}. Hay que cargar la nueva.";
                    created += this.AddIfMissing(trainerId, message);
                }
            }

            foreach (var user in this.InformationService.Users.Where(u => u.TrainerId == trainerId))
            {
                var dueDate = this.GetNextMeasurementDate(user.Id);
                if (dueDate <= today.AddDays(MeasurementWarningDays))
                {
                    var message = $"La medicion de {user.Name} {user.LastName} esta cerca. Le toca el {dueDate:yyyy-MM-dd}.";
                    created += this.AddIfMissing(trainerId, message);
                }
            }

            if (created > 0)
            {
                this.InformationService.Save(this.InformationService.Notifications);
            }

            return created;
        }

        private DateTime GetNextMeasurementDate(string userId)
        {
            var last = this.InformationService.Measurements
                .Where(m => m.UserId == userId)
                .Select(m => (DateTime?)m.Date)
                .Max();

            return last.HasValue ? last.Value.Date.AddDays(MeasurementIntervalDays) : DateTime.Today;
        }

        private int AddIfMissing(string recipientId, string message)
        {
            message = CleanMessage(message);
            var exists = this.InformationService.Notifications.Any(n => n.RecipientId == recipientId && n.Message == message);
            if (exists)
            {
                return 0;
            }

            this.AddWithoutSaving(recipientId, message);
            return 1;
        }

        private void AddWithoutSaving(string recipientId, string message)
        {
            var id = this.InformationService.GenerateId("N", this.InformationService.Notifications.Select(n => n.Id), 4);
            var date = DateTime.Today.ToString("yyyy-MM-dd");
            this.InformationService.Notifications.Add(new Notification(id, recipientId, CleanMessage(message), date));
        }

        private static string CleanMessage(string message)
        {
            return message.Replace(',', ';');
        }
    }
}
