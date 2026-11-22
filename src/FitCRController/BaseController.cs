namespace FitCRController
{
    /// <summary>Base class for the controllers, so none of them repeats how it receives the information service.</summary>
    public abstract class BaseController
    {
        protected BaseController(InformationService informationService)
        {
            this.InformationService = informationService;
        }

        public InformationService InformationService { get; set; }
    }
}
