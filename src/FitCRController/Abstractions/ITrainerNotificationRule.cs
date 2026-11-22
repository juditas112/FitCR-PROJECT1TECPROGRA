namespace FitCRController.Abstractions
{
    /// <summary>Rule that creates the messages for the timeline of a trainer. A new kind of notice only needs a new rule.</summary>
    public interface ITrainerNotificationRule
    {
        public IEnumerable<string> GetMessages(string trainerId, InformationService informationService);
    }
}
