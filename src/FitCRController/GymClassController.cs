using FitCRModels;
using FitCRModels.Enums;

namespace FitCRController
{
    /// <summary>Controller for the classes offered by the gym.</summary>
    public class GymClassController
    {
        public GymClassController(InformationService informationService)
        {
            this.InformationService = informationService;
        }

        public InformationService InformationService { get; set; }

        /// <summary>Gets all the classes, grouped by type.</summary>
        public List<GymClass> GetClasses()
        {
            return this.InformationService.GymClasses
                .OrderBy(c => c.ClassType)
                .ThenBy(c => c.Schedule)
                .ToList();
        }

        /// <summary>Gets the classes that a trainer teaches.</summary>
        public List<GymClass> GetClassesOfTrainer(string trainerId)
        {
            return this.GetClasses().Where(c => c.TrainerId == trainerId).ToList();
        }

        /// <summary>Gets the name to show for a type of class.</summary>
        public string GetClassName(GymClassType type)
        {
            return type switch
            {
                GymClassType.Yoga => "Yoga",
                GymClassType.OpenGym => "Hora de gimnasio abierto",
                GymClassType.CrossFit => "Crossfit",
                GymClassType.Zumba => "Zumba",
                _ => "Spinning",
            };
        }
    }
}
