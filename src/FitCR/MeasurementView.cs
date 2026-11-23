using FitCRController;
using FitCRModels;

namespace FitCR
{
    /// <summary>View for the trainer to register a measurement of a user.</summary>
    public class MeasurementView : Form
    {
        private readonly BasicUser user;
        private readonly DateTimePicker dtDate = new DateTimePicker { Format = DateTimePickerFormat.Short, MaxDate = DateTime.Today, Value = DateTime.Today };
        private readonly NumericUpDown numWeight = UiHelper.CreateNumber(20, 300, 1, 70);
        private readonly NumericUpDown numBodyFat = UiHelper.CreateNumber(2, 70, 1, 20);
        private readonly NumericUpDown numMuscleMass = UiHelper.CreateNumber(5, 100, 1, 30);

        public MeasurementView(ControllerContainer controllers, BasicUser user)
        {
            this.Controllers = controllers;
            this.user = user;
            this.BuildLayout();
        }

        private ControllerContainer Controllers { get; }

        private void BuildLayout()
        {
            UiHelper.ConfigureDialog(this, "Fit.CR - Nueva medición", 380, 270);
            var table = UiHelper.CreateFormTable();
            UiHelper.AddRow(table, "Usuario", new Label { Text = $"{this.user.Name} {this.user.LastName}", AutoSize = true });
            UiHelper.AddRow(table, "Fecha", this.dtDate);
            UiHelper.AddRow(table, "Peso (kg)", this.numWeight);
            UiHelper.AddRow(table, "Grasa (%)", this.numBodyFat);
            UiHelper.AddRow(table, "Musculatura (kg)", this.numMuscleMass);

            var save = UiHelper.CreateButton("Guardar", this.BtnSave_Click);
            var cancel = UiHelper.CreateButton("Cancelar", (sender, e) => this.Close());
            this.CancelButton = cancel;
            this.Controls.Add(UiHelper.CreateButtonPanel(DockStyle.Bottom, save, cancel));
            this.Controls.Add(table);
        }

        private void BtnSave_Click(object? sender, EventArgs e)
        {
            var result = this.Controllers.Measurements.AddMeasurement(
                this.user.Id,
                this.dtDate.Value,
                (double)this.numWeight.Value,
                (double)this.numBodyFat.Value,
                (double)this.numMuscleMass.Value);

            UiHelper.ShowResult(result);
            if (result.Success)
            {
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
