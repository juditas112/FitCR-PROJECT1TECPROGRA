using FitCRController;
using FitCRModels.Enums;

namespace FitCR
{
    /// <summary>View for the user to set a new goal. The types of goal come from the rules, so a new type shows up here by itself.</summary>
    public class GoalView : Form
    {
        private readonly string userId;
        private readonly ComboBox cmbType = new ComboBox { DropDownStyle = ComboBoxStyle.DropDownList };
        private readonly NumericUpDown numTarget = UiHelper.CreateNumber(0.1M, 10000, 1, 10);
        private readonly DateTimePicker dtDeadline = new DateTimePicker { Format = DateTimePickerFormat.Short, MinDate = DateTime.Today.AddDays(1), Value = DateTime.Today.AddDays(30) };

        public GoalView(ControllerContainer controllers, string userId)
        {
            this.Controllers = controllers;
            this.userId = userId;
            this.BuildLayout();
        }

        private ControllerContainer Controllers { get; }

        private void BuildLayout()
        {
            UiHelper.ConfigureDialog(this, "Fit.CR - Nueva meta", 380, 200);
            this.cmbType.DisplayMember = "Value";
            this.cmbType.ValueMember = "Key";
            this.cmbType.DataSource = this.Controllers.Goals.GetGoalTypes();

            var table = UiHelper.CreateFormTable();
            UiHelper.AddRow(table, "Tipo de meta", this.cmbType);
            UiHelper.AddRow(table, "Valor a lograr", this.numTarget);
            UiHelper.AddRow(table, "Fecha límite", this.dtDeadline);

            var save = UiHelper.CreateButton("Guardar", this.BtnSave_Click);
            var cancel = UiHelper.CreateButton("Cancelar", (sender, e) => this.Close());
            this.CancelButton = cancel;
            this.Controls.Add(UiHelper.CreateButtonPanel(DockStyle.Bottom, save, cancel));
            this.Controls.Add(table);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            var result = this.Controllers.Goals.AddGoal(this.userId, (GoalType)this.cmbType.SelectedValue!, (double)this.numTarget.Value, this.dtDeadline.Value);
            UiHelper.ShowResult(result);
            if (result.Success)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
