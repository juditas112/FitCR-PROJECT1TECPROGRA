using FitCRController;
using FitCRModels;
using FitCRModels.Enums;

namespace FitCR
{
    /// <summary>Menu of the basic user: his information in tabs and the buttons to register a visit, set goals and see the statistics.</summary>
    public class UserMenuView : Form
    {
        private static readonly System.Globalization.CultureInfo SpanishCulture = new System.Globalization.CultureInfo("es-CR");

        private readonly BasicUser user;
        private readonly Label lblWelcome = new Label { AutoSize = true, Font = new Font("Segoe UI", 14F, FontStyle.Bold) };
        private readonly DataGridView gridNotifications = UiHelper.CreateGrid();
        private readonly DataGridView gridVisits = UiHelper.CreateGrid();
        private readonly DataGridView gridRoutines = UiHelper.CreateGrid();
        private readonly DataGridView gridExercises = UiHelper.CreateGrid();
        private readonly DataGridView gridMeasurements = UiHelper.CreateGrid();
        private readonly DataGridView gridIncreases = UiHelper.CreateGrid();
        private readonly DataGridView gridBadges = UiHelper.CreateGrid();
        private readonly DataGridView gridGoals = UiHelper.CreateGrid();
        private readonly DataGridView gridClasses = UiHelper.CreateGrid();

        public UserMenuView(ControllerContainer controllers)
        {
            this.Controllers = controllers;
            this.user = (BasicUser)controllers.InformationService.CurrentPerson!;
            this.BuildLayout();
            this.Controllers.Badges.RefreshShameBadges();
            this.LoadData();
        }

        private ControllerContainer Controllers { get; }

        private static TabPage CreateTab(string title, Control content)
        {
            var page = new TabPage(title);
            content.Dock = DockStyle.Fill;
            page.Controls.Add(content);
            return page;
        }

        private void BuildLayout()
        {
            this.Text = "Fit.CR - Menú del usuario";
            this.ClientSize = new Size(900, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            var routines = new SplitContainer { Orientation = Orientation.Horizontal };
            routines.Panel1.Controls.Add(this.gridRoutines);
            routines.Panel2.Controls.Add(this.gridExercises);
            this.gridRoutines.SelectionChanged += this.GridRoutines_SelectionChanged;

            var tabs = new TabControl { Dock = DockStyle.Fill };
            tabs.TabPages.Add(CreateTab("Notificaciones", this.gridNotifications));
            tabs.TabPages.Add(CreateTab("Visitas", this.gridVisits));
            tabs.TabPages.Add(CreateTab("Últimas rutinas", routines));
            tabs.TabPages.Add(CreateTab("Mediciones", this.gridMeasurements));
            tabs.TabPages.Add(CreateTab("Aumentos", this.gridIncreases));
            tabs.TabPages.Add(CreateTab("Insignias", this.gridBadges));
            tabs.TabPages.Add(CreateTab("Metas", this.gridGoals));
            tabs.TabPages.Add(CreateTab("Clases", this.gridClasses));

            var buttons = UiHelper.CreateButtonPanel(
                DockStyle.Top,
                UiHelper.CreateButton("Registrar visita", this.BtnVisit_Click),
                UiHelper.CreateButton("Nueva meta", this.BtnGoal_Click),
                UiHelper.CreateButton("Mi perfil", this.BtnProfile_Click),
                UiHelper.CreateButton("Estadísticas", this.BtnStatistics_Click),
                UiHelper.CreateButton("Cerrar sesión", (sender, e) => this.Close()));

            var header = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.TopDown, Padding = new Padding(10, 10, 10, 0) };
            header.Controls.Add(this.lblWelcome);

            this.Controls.Add(tabs);
            this.Controls.Add(buttons);
            this.Controls.Add(header);
        }

        private void LoadData()
        {
            var id = this.user.Id;
            this.lblWelcome.Text = $"{this.user.Name} {this.user.LastName}";

            UiHelper.Fill(
                this.gridNotifications,
                this.Controllers.Notifications.GetNotifications(id),
                new GridColumn<Notification>("Fecha", n => UiHelper.FormatDate(n.Date)),
                new GridColumn<Notification>("Mensaje", n => n.Message));

            UiHelper.Fill(
                this.gridVisits,
                this.Controllers.Visits.GetVisits(id),
                new GridColumn<Visit>("Fecha y hora", v => UiHelper.FormatDateTime(v.Date)),
                new GridColumn<Visit>("Día", v => v.Date.ToString("dddd", SpanishCulture)));

            UiHelper.Fill(
                this.gridRoutines,
                this.Controllers.Routines.GetLastRoutines(id),
                new GridColumn<Routine>("Rutina", r => r.Id),
                new GridColumn<Routine>("Inicio", r => UiHelper.FormatDate(r.StartDate)),
                new GridColumn<Routine>("Vencimiento", r => UiHelper.FormatDate(r.EndDate)),
                new GridColumn<Routine>("Estado", r => r.IsExpired ? "Vencida" : "Vigente"));

            UiHelper.Fill(
                this.gridMeasurements,
                this.Controllers.Measurements.GetMeasurements(id),
                new GridColumn<Measurement>("Fecha", m => UiHelper.FormatDate(m.Date)),
                new GridColumn<Measurement>("Peso (kg)", m => m.Weight),
                new GridColumn<Measurement>("Grasa (%)", m => m.BodyFat),
                new GridColumn<Measurement>("Musculatura (kg)", m => m.MuscleMass));

            UiHelper.Fill(
                this.gridIncreases,
                this.Controllers.Routines.GetIncreases(id),
                new GridColumn<RoutineIncrease>("Fecha", i => UiHelper.FormatDate(i.Date)),
                new GridColumn<RoutineIncrease>("Ejercicio", i => this.Controllers.Routines.GetExerciseName(i.ExerciseId)),
                new GridColumn<RoutineIncrease>("Peso anterior", i => i.PreviousWeight),
                new GridColumn<RoutineIncrease>("Peso nuevo", i => i.NewWeight),
                new GridColumn<RoutineIncrease>("Aumento", i => i.Increase));

            UiHelper.Fill(
                this.gridBadges,
                this.Controllers.Badges.GetBadges(id),
                new GridColumn<Badge>("Fecha", b => UiHelper.FormatDate(b.Date)),
                new GridColumn<Badge>("Tipo", b => b.BadgeType == BadgeType.Shame ? "Vergüenza" : "Logro"),
                new GridColumn<Badge>("Insignia", b => b.Name));

            UiHelper.Fill(
                this.gridGoals,
                this.Controllers.Goals.GetGoals(id),
                new GridColumn<Goal>("Meta", g => this.Controllers.Goals.GetGoalTypeName(g.GoalType)),
                new GridColumn<Goal>("Valor", g => g.TargetValue),
                new GridColumn<Goal>("Fecha límite", g => UiHelper.FormatDate(g.Deadline)),
                new GridColumn<Goal>("Estado", g => g.IsAchieved ? "Cumplida" : "Pendiente"));

            UiHelper.Fill(
                this.gridClasses,
                this.Controllers.GymClasses.GetClasses(),
                new GridColumn<GymClass>("Clase", c => GymClassController.GetClassName(c.ClassType)),
                new GridColumn<GymClass>("Día", c => c.Day),
                new GridColumn<GymClass>("Inicio", c => UiHelper.FormatTime(c.StartTime)),
                new GridColumn<GymClass>("Fin", c => UiHelper.FormatTime(c.EndTime)),
                new GridColumn<GymClass>("Entrenador", c => this.GetTrainerName(c.TrainerId)));
        }

        private string GetTrainerName(string trainerId)
        {
            var trainer = this.Controllers.Users.GetTrainer(trainerId);
            return trainer == null ? string.Empty : $"{trainer.Name} {trainer.LastName}";
        }

        private void GridRoutines_SelectionChanged(object? sender, EventArgs e)
        {
            var routine = UiHelper.GetSelected<Routine>(this.gridRoutines);
            var exercises = routine == null ? new List<Exercise>() : this.Controllers.Routines.GetExercises(routine.Id);
            UiHelper.Fill(
                this.gridExercises,
                exercises,
                new GridColumn<Exercise>("Ejercicio", x => x.Name),
                new GridColumn<Exercise>("Series", x => x.Sets),
                new GridColumn<Exercise>("Repeticiones", x => x.Repetitions),
                new GridColumn<Exercise>("Peso (kg)", x => x.Weight));
        }

        private void BtnVisit_Click(object? sender, EventArgs e)
        {
            UiHelper.ShowResult(this.Controllers.Visits.RegisterVisit(this.user.Id));
            this.LoadData();
        }

        private void BtnGoal_Click(object? sender, EventArgs e)
        {
            using var goalView = new GoalView(this.Controllers, this.user.Id);
            goalView.ShowDialog(this);
            this.LoadData();
        }

        private void BtnProfile_Click(object? sender, EventArgs e)
        {
            using var profileView = new ProfileView(this.Controllers, this.user);
            profileView.ShowDialog(this);
            this.LoadData();
        }

        private void BtnStatistics_Click(object? sender, EventArgs e)
        {
            using var statisticsView = new StatisticsView(this.Controllers, this.user.Id);
            statisticsView.ShowDialog(this);
        }
    }
}
