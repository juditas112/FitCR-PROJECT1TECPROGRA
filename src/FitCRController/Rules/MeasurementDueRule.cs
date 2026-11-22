using FitCRController.Abstractions;

namespace FitCRController.Rules
{
    /// <summary>Notice for the trainer when the next measurement of a user is close. It is due 30 days after the last one.</summary>
    public class MeasurementDueRule : ITrainerNotificationRule
    {
        private const int IntervalDays = 30;
        private const int WarningDays = 5;

        public IEnumerable<string> GetMessages(string trainerId, InformationService informationService)
        {
            var limit = DateTime.Today.AddDays(WarningDays);
            return informationService.Users
                .Where(u => u.TrainerId == trainerId)
                .Select(u => (User: u, Due: GetDueDate(u.Id, informationService)))
                .Where(item => item.Due <= limit)
                .Select(item => $"La medicion de {item.User.Name} {item.User.LastName} esta cerca. Le toca el {item.Due:yyyy-MM-dd}.")
                .ToList();
        }

        private static DateTime GetDueDate(string userId, InformationService informationService)
        {
            var last = informationService.GetLastMeasurement(userId);
            return last == null ? DateTime.Today : last.Date.Date.AddDays(IntervalDays);
        }
    }
}
