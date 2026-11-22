namespace FitCRController.Abstractions
{
    /// <summary>Rule for a badge of shame. A new badge of shame only needs a new rule.</summary>
    public interface IShameBadgeRule
    {
        public string BadgeName { get; }

        public IEnumerable<string> GetUserIds(InformationService informationService);
    }
}
