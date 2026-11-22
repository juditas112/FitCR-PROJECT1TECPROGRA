using FitCRController.Abstractions;

namespace FitCRController.Rules
{
    /// <summary>Badge of shame for the 10 users with the fewest visits in the last 30 days.</summary>
    public class FewVisitsRule : IShameBadgeRule
    {
        private const int WindowDays = 30;
        private const int RankingSize = 10;

        public string BadgeName => "Top 10 con menos visitas";

        public IEnumerable<string> GetUserIds(InformationService informationService)
        {
            var since = DateTime.Today.AddDays(-WindowDays);
            return informationService.Users
                .OrderBy(u => informationService.Visits.Count(v => v.UserId == u.Id && v.Date.Date >= since))
                .ThenBy(u => u.Id)
                .Take(RankingSize)
                .Select(u => u.Id)
                .ToList();
        }
    }
}
