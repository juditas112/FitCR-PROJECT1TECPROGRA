using FitCRController;
using FitCRModels;

namespace FitCR
{
    /// <summary>View for the trainer to create or change a routine and its exercises. The exercises are enabled when the routine exists.</summary>
    public class RoutineEditView : Form
    {
        private readonly string trainerId;
        private readonly string userId;
        private readonly DateTimePicker dtStart = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today };
        private readonly DateTimePicker dtEnd = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = DateTime.Today.AddDays(27) };
        private readonly DataGridView gridExercises = UiHelper.CreateGrid();
        private readonly TextBox txtName = new TextBox();
        private readonly NumericUpDown numSets = UiHelper.CreateNumber(1, 20, 0, 3);
        private readonly NumericUpDown numRepetitions = UiHelper.CreateNumber(1, 100, 0, 10);
        private readonly NumericUpDown numWeight = UiHelper.CreateNumber(0, 500, 1, 20);
        private readonly GroupBox grpExercises = new GroupBox { Text = "Ejercicios", Dock = DockStyle.Fill, Padding = new Padding(8) };
        private string? routineId;

        public RoutineEditView(ControllerContainer controllers, string trainerId, string userId, string? routineId)
        {
            this.Controllers = controllers;
            this.trainerId = trainerId;
            this.userId = userId;
            this.routineId = routineId;
            this.BuildLayout();
            this.LoadRoutine();
        }

        private ControllerContainer Controllers { get; }

        private void BuildLayout()
        {
            UiHelper.ConfigureDialog(this, "Fit.CR - Rutina", 640, 560);

            var dates = UiHelper.CreateFormTable();
            UiHelper.AddRow(dates, "Fecha de inicio", this.dtStart);
            UiHelper.AddRow(dates, "Fecha de vencimiento", this.dtEnd);
            var saveRoutine = UiHelper.CreateButton("Guardar rutina", this.BtnSaveRoutine_Click);
            var close = UiHelper.CreateButton("Cerrar", (sender, e) => this.Close());
            this.CancelButton = close;

            var inputs = UiHelper.CreateFormTable();
            inputs.Dock = DockStyle.Bottom;
            UiHelper.AddRow(inputs, "Ejercicio", this.txtName);
            UiHelper.AddRow(inputs, "Series", this.numSets);
            UiHelper.AddRow(inputs, "Repeticiones", this.numRepetitions);
            UiHelper.AddRow(inputs, "Peso (kg)", this.numWeight);
            var exerciseButtons = UiHelper.CreateButtonPanel(
                DockStyle.Bottom,
                UiHelper.CreateButton("Agregar ejercicio", this.BtnAddExercise_Click),
                UiHelper.CreateButton("Actualizar ejercicio", this.BtnUpdateExercise_Click),
                UiHelper.CreateButton("Eliminar ejercicio", this.BtnDeleteExercise_Click));

            this.gridExercises.SelectionChanged += this.GridExercises_SelectionChanged;
            this.grpExercises.Controls.Add(this.gridExercises);
            this.grpExercises.Controls.Add(exerciseButtons);
            this.grpExercises.Controls.Add(inputs);

            this.Controls.Add(this.grpExercises);
            this.Controls.Add(UiHelper.CreateButtonPanel(DockStyle.Top, saveRoutine, close));
            this.Controls.Add(dates);
        }

        private void LoadRoutine()
        {
            if (this.routineId != null)
            {
                var routine = this.Controllers.Routines.GetRoutines(this.userId).FirstOrDefault(r => r.Id == this.routineId);
                if (routine != null)
                {
                    this.dtStart.Value = routine.StartDate;
                    this.dtEnd.Value = routine.EndDate;
                }
            }

            this.grpExercises.Enabled = this.routineId != null;
            this.LoadExercises();
        }

        private void LoadExercises()
        {
            var exercises = this.routineId == null ? new List<Exercise>() : this.Controllers.Routines.GetExercises(this.routineId);
            UiHelper.Fill(
                this.gridExercises,
                exercises,
                new GridColumn<Exercise>("Ejercicio", x => x.Name),
                new GridColumn<Exercise>("Series", x => x.Sets),
                new GridColumn<Exercise>("Repeticiones", x => x.Repetitions),
                new GridColumn<Exercise>("Peso (kg)", x => x.Weight));
        }

        private void GridExercises_SelectionChanged(object? sender, EventArgs e)
        {
            var exercise = UiHelper.GetSelected<Exercise>(this.gridExercises);
            if (exercise != null)
            {
                this.txtName.Text = exercise.Name;
                this.numSets.Value = Math.Clamp(exercise.Sets, (int)this.numSets.Minimum, (int)this.numSets.Maximum);
                this.numRepetitions.Value = Math.Clamp(exercise.Repetitions, (int)this.numRepetitions.Minimum, (int)this.numRepetitions.Maximum);
                this.numWeight.Value = Math.Clamp((decimal)exercise.Weight, this.numWeight.Minimum, this.numWeight.Maximum);
            }
        }

        private void BtnSaveRoutine_Click(object? sender, EventArgs e)
        {
            var result = this.routineId == null
                ? this.Controllers.Routines.AddRoutine(this.trainerId, this.userId, this.dtStart.Value, this.dtEnd.Value)
                : this.Controllers.Routines.UpdateRoutine(this.routineId, this.dtStart.Value, this.dtEnd.Value);

            UiHelper.ShowResult(result);
            if (result.Success && this.routineId == null)
            {
                this.routineId = result.Id;
                this.grpExercises.Enabled = true;
            }
        }

        private void BtnAddExercise_Click(object? sender, EventArgs e)
        {
            var result = this.Controllers.Routines.AddExercise(this.routineId!, this.txtName.Text, (int)this.numSets.Value, (int)this.numRepetitions.Value, (double)this.numWeight.Value);
            if (result.Success)
            {
                this.txtName.Clear();
                this.LoadExercises();
            }
            else
            {
                UiHelper.ShowResult(result);
            }
        }

        private void BtnUpdateExercise_Click(object? sender, EventArgs e)
        {
            var exercise = UiHelper.GetSelected<Exercise>(this.gridExercises);
            if (exercise == null)
            {
                MessageBox.Show("Selecciona un ejercicio de la lista.", "Fit.CR");
                return;
            }

            var result = this.Controllers.Routines.UpdateExercise(exercise.Id, (int)this.numSets.Value, (int)this.numRepetitions.Value, (double)this.numWeight.Value);
            UiHelper.ShowResult(result);
            this.LoadExercises();
        }

        private void BtnDeleteExercise_Click(object? sender, EventArgs e)
        {
            var exercise = UiHelper.GetSelected<Exercise>(this.gridExercises);
            if (exercise == null)
            {
                MessageBox.Show("Selecciona un ejercicio de la lista.", "Fit.CR");
                return;
            }

            UiHelper.ShowResult(this.Controllers.Routines.DeleteExercise(exercise.Id));
            this.LoadExercises();
        }
    }
}
