using FitCRController.Abstractions;

namespace FitCRController.Rules
{
    /// <summary>Notice for the trainer when a routine expires in the next 7 days.</summary>
    public class ExpiringRoutineRule : ITrainerNotificationRule
    {
        private const int WarningDays = 7;

        public IEnumerable<string> GetMessages(string trainerId, InformationService informationService)
        {
            var today = DateTime.Today;
            return informationService.Routines
                .Where(r => r.TrainerId == trainerId && r.EndDate.Date >= today && r.EndDate.Date <= today.AddDays(WarningDays))
                .Join(
                    informationService.Users,
                    routine => routine.UserId,
                    user => user.Id,
                    (routine, user) => $"La rutina de {user.Name} {user.LastName} vence el {routine.EndDate:yyyy-MM-dd}. Hay que cargar la nueva.")
                .ToList();
        }
    }
}
