using FitCRModels;
using FitCRModels.Enums;

namespace FitCRController
{
    /// <summary>Controller for the statistics, calculated when the user asks for them. Only the basic user can see them.</summary>
    public class StatisticsController : BaseController
    {
        private const int RankingSize = 10;

        public StatisticsController(InformationService informationService)
            : base(informationService)
        {
        }

        /// <summary>Checks that the person logged in can see the statistics and that the range of dates is valid.</summary>
        public OperationResult Validate(DateTime from, DateTime to)
        {
            if (this.InformationService.CurrentPerson?.Role != Role.BasicUser)
            {
                return OperationResult.Fail("The statistics are only for the basic user.");
            }

            return from.Date > to.Date ? OperationResult.Fail("The first date cannot be after the last date.") : OperationResult.Ok(string.Empty);
        }

        /// <summary>Gets how the weight, body fat and muscle mass of a user changed in a range of dates. The changes are zero if there are less than two measurements.</summary>
        public ProgressSummary GetProgress(string userId, DateTime from, DateTime to)
        {
            var summary = new ProgressSummary
            {
                Visits = this.CountVisits(userId, from, to),
                GoalsAchieved = this.InformationService.Goals.Count(g =>
                    g.UserId == userId && g.IsAchieved && g.Deadline.Date >= from.Date && g.Deadline.Date <= to.Date),
            };

            var measurements = this.InformationService.Measurements
                .Where(m => m.UserId == userId && m.Date.Date >= from.Date && m.Date.Date <= to.Date)
                .OrderBy(m => m.Date)
                .ToList();

            if (measurements.Count >= 2)
            {
                var first = measurements[0];
                var last = measurements[^1];
                summary.HasEnoughData = true;
                summary.WeightChange = Math.Round(last.Weight - first.Weight, 2);
                summary.BodyFatChange = Math.Round(last.BodyFat - first.BodyFat, 2);
                summary.MuscleMassChange = Math.Round(last.MuscleMass - first.MuscleMass, 2);
            }

            return summary;
        }

        /// <summary>Gets the 10 users with the most visits in a range of dates.</summary>
        public List<RankingEntry> GetTopVisitors(DateTime from, DateTime to)
        {
            return this.BuildRanking(from, to)
                .OrderByDescending(r => r.Visits)
                .ThenBy(r => r.User.Id)
                .Take(RankingSize)
                .ToList();
        }

        /// <summary>Gets the 10 users with the fewest visits in a range of dates.</summary>
        public List<RankingEntry> GetTopAbsent(DateTime from, DateTime to)
        {
            return this.BuildRanking(from, to)
                .OrderBy(r => r.Visits)
                .ThenBy(r => r.User.Id)
                .Take(RankingSize)
                .ToList();
        }

        private List<RankingEntry> BuildRanking(DateTime from, DateTime to)
        {
            return this.InformationService.Users
                .Select(u => new RankingEntry(u, this.CountVisits(u.Id, from, to)))
                .ToList();
        }

        private int CountVisits(string userId, DateTime from, DateTime to)
        {
            return this.InformationService.Visits.Count(v => v.UserId == userId && v.Date.Date >= from.Date && v.Date.Date <= to.Date);
        }
    }
}
