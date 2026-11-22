using FitCRController.Abstractions;

namespace FitCRController.Rules
{
    /// <summary>Badge of shame for the users who have no measurement in the last 45 days.</summary>
    public class NoProgressRule : IShameBadgeRule
    {
        private const int WindowDays = 45;

        public string BadgeName => "Sin agregar progreso";

        public IEnumerable<string> GetUserIds(InformationService informationService)
        {
            var since = DateTime.Today.AddDays(-WindowDays);
            return informationService.Users
                .Where(u => !informationService.Measurements.Any(m => m.UserId == u.Id && m.Date.Date >= since))
                .Select(u => u.Id)
                .ToList();
        }
    }
}
