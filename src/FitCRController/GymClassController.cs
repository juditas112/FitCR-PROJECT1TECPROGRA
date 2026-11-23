using FitCRModels;
using FitCRModels.Enums;

namespace FitCRController
{
    /// <summary>Controller for the classes offered by the gym.</summary>
    public class GymClassController : BaseController
    {
        private static readonly string[] DaysOfWeek = { "Lunes", "Martes", "Miercoles", "Jueves", "Viernes", "Sabado", "Domingo" };

        private static readonly Dictionary<GymClassType, string> ClassNames = new Dictionary<GymClassType, string>
        {
            { GymClassType.Yoga, "Yoga" },
            { GymClassType.OpenGym, "Hora de gimnasio abierto" },
            { GymClassType.CrossFit, "Crossfit" },
            { GymClassType.Zumba, "Zumba" },
            { GymClassType.Spinning, "Spinning" },
        };

        public GymClassController(InformationService informationService)
            : base(informationService)
        {
        }

        /// <summary>Gets the name to show for a type of class.</summary>
        public static string GetClassName(GymClassType type)
        {
            return ClassNames.TryGetValue(type, out var name) ? name : type.ToString();
        }

        /// <summary>Gets all the classes, grouped by type.</summary>
        public List<GymClass> GetClasses()
        {
            return this.InformationService.GymClasses
                .OrderBy(c => c.ClassType)
                .ThenBy(c => Array.IndexOf(DaysOfWeek, c.Day))
                .ThenBy(c => c.StartTime)
                .ToList();
        }

        public List<GymClass> GetClassesOfTrainer(string trainerId)
        {
            return this.GetClasses().Where(c => c.TrainerId == trainerId).ToList();
        }
    }
}
