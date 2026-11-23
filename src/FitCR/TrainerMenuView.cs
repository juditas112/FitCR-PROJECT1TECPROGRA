using FitCRController;
using FitCRModels;
using FitCRModels.Enums;

namespace FitCR
{
    /// <summary>Menu of the trainer: his users, and for the selected user the routines and the measurements. It also shows his notices and classes.</summary>
    public class TrainerMenuView : Form
    {
        private readonly Trainer trainer;
        private readonly Label lblWelcome = new Label { AutoSize = true, Font = new Font("Segoe UI", 14F, FontStyle.Bold) };
        private readonly Label lblInfo = new Label { AutoSize = true, Font = new Font("Segoe UI", 9F), ForeColor = Color.DimGray };
        private readonly ListBox lstUsers = new ListBox { Dock = DockStyle.Fill, FormattingEnabled = true };
        private readonly DataGridView gridRoutines = UiHelper.CreateGrid();
        private readonly DataGridView gridMeasurements = UiHelper.CreateGrid();
        private readonly DataGridView gridNotices = UiHelper.CreateGrid();
        private readonly DataGridView gridClasses = UiHelper.CreateGrid();

        public TrainerMenuView(ControllerContainer controllers)
        {
            this.Controllers = controllers;
            this.trainer = (Trainer)controllers.InformationService.CurrentPerson!;
            this.BuildLayout();
            this.Controllers.Notifications.GenerateTrainerNotifications(this.trainer.Id);
            this.LoadUsers();
            this.LoadTrainerData();
        }

        private ControllerContainer Controllers { get; }

        private static TabPage CreateTab(string title, Control content, Control? buttons = null)
        {
            var page = new TabPage(title);
            content.Dock = DockStyle.Fill;
            page.Controls.Add(content);
            if (buttons != null)
            {
                page.Controls.Add(buttons);
            }

            return page;
        }

        private void BuildLayout()
        {
            this.Text = "Fit.CR - Menú del entrenador";
            this.ClientSize = new Size(960, 600);
            this.StartPosition = FormStartPosition.CenterScreen;

            var routineButtons = UiHelper.CreateButtonPanel(
                DockStyle.Bottom,
                UiHelper.CreateButton("Agregar rutina", this.BtnAddRoutine_Click),
                UiHelper.CreateButton("Editar rutina", this.BtnEditRoutine_Click),
                UiHelper.CreateButton("Eliminar rutina", this.BtnDeleteRoutine_Click));
            var measurementButtons = UiHelper.CreateButtonPanel(DockStyle.Bottom, UiHelper.CreateButton("Agregar medición", this.BtnAddMeasurement_Click));

            var tabs = new TabControl { Dock = DockStyle.Fill };
            tabs.TabPages.Add(CreateTab("Rutinas", this.gridRoutines, routineButtons));
            tabs.TabPages.Add(CreateTab("Mediciones", this.gridMeasurements, measurementButtons));
            tabs.TabPages.Add(CreateTab("Avisos", this.gridNotices));
            tabs.TabPages.Add(CreateTab("Mis clases", this.gridClasses));

            this.lstUsers.Format += this.LstUsers_Format;
            this.lstUsers.SelectedIndexChanged += this.LstUsers_SelectedIndexChanged;
            var usersGroup = new GroupBox { Text = "Mis usuarios", Dock = DockStyle.Fill, Padding = new Padding(8) };
            usersGroup.Controls.Add(this.lstUsers);

            var split = new SplitContainer { Dock = DockStyle.Fill };
            this.Load += (sender, e) => split.SplitterDistance = 230;
            split.Panel1.Controls.Add(usersGroup);
            split.Panel2.Controls.Add(tabs);

            var header = new FlowLayoutPanel { Dock = DockStyle.Top, AutoSize = true, FlowDirection = FlowDirection.TopDown, Padding = new Padding(10) };
            header.Controls.Add(this.lblWelcome);
            header.Controls.Add(this.lblInfo);
            header.Controls.Add(UiHelper.CreateButton("Cerrar sesión", (sender, e) => this.Close()));

            this.Controls.Add(split);
            this.Controls.Add(header);
        }

        private void LoadUsers()
        {
            this.lstUsers.DataSource = this.Controllers.Users.GetUsersOfTrainer(this.trainer.Id);
        }

        private void LoadTrainerData()
        {
            var type = this.trainer.TrainerType == TrainerType.Personal ? "personal" : "del gimnasio";
            this.lblWelcome.Text = $"{this.trainer.Name} {this.trainer.LastName}";
            this.lblInfo.Text = $"{this.trainer.Id} | Entrenador {type} | {this.lstUsers.Items.Count} usuarios";

            UiHelper.Fill(
                this.gridNotices,
                this.Controllers.Notifications.GetNotifications(this.trainer.Id),
                new GridColumn<Notification>("Fecha", n => UiHelper.FormatDate(n.Date)),
                new GridColumn<Notification>("Aviso", n => n.Message));

            UiHelper.Fill(
                this.gridClasses,
                this.Controllers.GymClasses.GetClassesOfTrainer(this.trainer.Id),
                new GridColumn<GymClass>("Clase", c => GymClassController.GetClassName(c.ClassType)),
                new GridColumn<GymClass>("Día", c => c.Day),
                new GridColumn<GymClass>("Inicio", c => UiHelper.FormatTime(c.StartTime)),
                new GridColumn<GymClass>("Fin", c => UiHelper.FormatTime(c.EndTime)));
        }

        private void LoadUserData()
        {
            var user = this.lstUsers.SelectedItem as BasicUser;
            var routines = user == null ? new List<Routine>() : this.Controllers.Routines.GetRoutines(user.Id);
            var measurements = user == null ? new List<Measurement>() : this.Controllers.Measurements.GetMeasurements(user.Id);

            UiHelper.Fill(
                this.gridRoutines,
                routines,
                new GridColumn<Routine>("Rutina", r => r.Id),
                new GridColumn<Routine>("Inicio", r => UiHelper.FormatDate(r.StartDate)),
                new GridColumn<Routine>("Vencimiento", r => UiHelper.FormatDate(r.EndDate)),
                new GridColumn<Routine>("Estado", r => r.IsExpired ? "Vencida" : "Vigente"),
                new GridColumn<Routine>("Ejercicios", r => this.Controllers.Routines.GetExercises(r.Id).Count));

            UiHelper.Fill(
                this.gridMeasurements,
                measurements,
                new GridColumn<Measurement>("Fecha", m => UiHelper.FormatDate(m.Date)),
                new GridColumn<Measurement>("Peso (kg)", m => m.Weight),
                new GridColumn<Measurement>("Grasa (%)", m => m.BodyFat),
                new GridColumn<Measurement>("Musculatura (kg)", m => m.MuscleMass));
        }

        private BasicUser? RequireUser()
        {
            var user = this.lstUsers.SelectedItem as BasicUser;
            if (user == null)
            {
                MessageBox.Show("Selecciona un usuario de la lista.", "Fit.CR");
            }

            return user;
        }

        private void LstUsers_Format(object? sender, ListControlConvertEventArgs e)
        {
            if (e.ListItem is BasicUser user)
            {
                e.Value = $"{user.Name} {user.LastName}";
            }
        }

        private void LstUsers_SelectedIndexChanged(object? sender, EventArgs e)
        {
            this.LoadUserData();
        }

        private void BtnAddRoutine_Click(object? sender, EventArgs e)
        {
            var user = this.RequireUser();
            if (user != null)
            {
                this.OpenRoutineEditor(user, null);
            }
        }

        private void BtnEditRoutine_Click(object? sender, EventArgs e)
        {
            var user = this.RequireUser();
            var routine = UiHelper.GetSelected<Routine>(this.gridRoutines);
            if (user != null && routine != null)
            {
                this.OpenRoutineEditor(user, routine.Id);
            }
        }

        private void BtnDeleteRoutine_Click(object? sender, EventArgs e)
        {
            var routine = UiHelper.GetSelected<Routine>(this.gridRoutines);
            if (routine == null)
            {
                MessageBox.Show("Selecciona una rutina de la lista.", "Fit.CR");
                return;
            }

            var answer = MessageBox.Show($"¿Eliminar la rutina {routine.Id} con sus ejercicios?", "Fit.CR", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (answer == DialogResult.Yes)
            {
                UiHelper.ShowResult(this.Controllers.Routines.DeleteRoutine(routine.Id));
                this.LoadUserData();
            }
        }

        private void BtnAddMeasurement_Click(object? sender, EventArgs e)
        {
            var user = this.RequireUser();
            if (user != null)
            {
                using var measurementView = new MeasurementView(this.Controllers, user);
                measurementView.ShowDialog(this);
                this.LoadUserData();
            }
        }

        private void OpenRoutineEditor(BasicUser user, string? routineId)
        {
            using var routineView = new RoutineEditView(this.Controllers, this.trainer.Id, user.Id, routineId);
            routineView.ShowDialog(this);
            this.LoadUserData();
            this.LoadTrainerData();
        }
    }
}
