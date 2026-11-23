using FitCRController.Abstractions;
using FitCRController.Rules;

namespace FitCRController
{
    /// <summary>Creates all the controllers with their rules, sharing the same information service, so the views receive only one object.</summary>
    public class ControllerContainer
    {
        public ControllerContainer(IDataHandler dataHandler)
        {
            this.InformationService = new InformationService(dataHandler);
            this.Notifications = new NotificationController(this.InformationService, new ITrainerNotificationRule[] { new ExpiringRoutineRule(), new MeasurementDueRule() });
            this.Badges = new BadgeController(this.InformationService, this.Notifications, new IShameBadgeRule[] { new FewVisitsRule(), new NoProgressRule(), new MissedGoalRule() });
            this.Goals = new GoalController(this.InformationService, this.Badges, new IGoalRule[] { new MuscleGoalRule(), new FatGoalRule(), new VisitsGoalRule() });
            this.Login = new LoginController(this.InformationService);
            this.Users = new UserController(this.InformationService, this.Notifications);
            this.Routines = new RoutineController(this.InformationService, this.Notifications);
            this.Visits = new VisitController(this.InformationService, this.Notifications, this.Goals, this.Badges);
            this.Measurements = new MeasurementController(this.InformationService, this.Goals);
            this.Statistics = new StatisticsController(this.InformationService);
            this.GymClasses = new GymClassController(this.InformationService);
        }

        public InformationService InformationService { get; }

        public LoginController Login { get; }

        public UserController Users { get; }

        public RoutineController Routines { get; }

        public VisitController Visits { get; }

        public MeasurementController Measurements { get; }

        public GoalController Goals { get; }

        public BadgeController Badges { get; }

        public NotificationController Notifications { get; }

        public StatisticsController Statistics { get; }

        public GymClassController GymClasses { get; }
    }
}
