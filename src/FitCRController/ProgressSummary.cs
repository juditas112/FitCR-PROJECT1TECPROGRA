namespace FitCRController
{
    /// <summary>Summary of the progress of a user in a range of dates.</summary>
    public class ProgressSummary
    {
        public bool HasEnoughData { get; set; }

        public double WeightChange { get; set; }

        public double BodyFatChange { get; set; }

        public double MuscleMassChange { get; set; }

        public int Visits { get; set; }

        public int GoalsAchieved { get; set; }
    }
}
