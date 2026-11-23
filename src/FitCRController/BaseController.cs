using FitCRModels.Enums;

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

        /// <summary>Returns a failed result if the person logged in is not a trainer, or null if the operation can continue.</summary>
        protected OperationResult? EnsureTrainer()
        {
            return this.InformationService.CurrentPerson?.Role == Role.Trainer
                ? null
                : OperationResult.Fail("Only a trainer can do this operation.");
        }
    }
}
