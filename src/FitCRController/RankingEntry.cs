using FitCRModels;

namespace FitCRController
{
    /// <summary>A row of a ranking, used for the top 10 lists.</summary>
    public class RankingEntry
    {
        public RankingEntry(BasicUser user, int visits)
        {
            this.User = user;
            this.Visits = visits;
        }

        public BasicUser User { get; }

        public int Visits { get; }
    }
}
