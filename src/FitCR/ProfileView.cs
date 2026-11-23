using FitCRController;
using FitCRModels;

namespace FitCR
{
    /// <summary>Profile of the user. It is read only: the weight and the height are updated by the trainer.</summary>
    public class ProfileView : Form
    {
        private readonly BasicUser user;

        public ProfileView(ControllerContainer controllers, BasicUser user)
        {
            this.Controllers = controllers;
            this.user = user;
            this.BuildLayout();
        }

        private ControllerContainer Controllers { get; }

        private static void AddValue(TableLayoutPanel table, string caption, string value)
        {
            UiHelper.AddRow(table, caption, new Label { Text = value, AutoSize = true, Margin = new Padding(3, 8, 3, 3) });
        }

        private void BuildLayout()
        {
            UiHelper.ConfigureDialog(this, "Fit.CR - Mi perfil", 420, 470);
            var trainer = this.Controllers.Users.GetTrainer(this.user.TrainerId);

            var table = UiHelper.CreateFormTable();
            AddValue(table, "ID", this.user.Id);
            AddValue(table, "Nombre", this.user.Name);
            AddValue(table, "Apellidos", this.user.LastName);
            AddValue(table, "Correo", this.user.Email);
            AddValue(table, "Teléfono", this.user.Number.ToString());
            AddValue(table, "Fecha de nacimiento", UiHelper.FormatDate(this.user.BirthDate));
            AddValue(table, "Edad", $"{this.user.Age} años");
            AddValue(table, "Peso", $"{this.user.Weight} kg");
            AddValue(table, "Estatura", $"{this.user.Height} m");
            AddValue(table, "IMC", this.user.Bmi.ToString());
            AddValue(table, "Entrenador", trainer == null ? "Sin entrenador" : $"{trainer.Name} {trainer.LastName}");

            var note = new Label { Text = "Tu entrenador actualiza tu peso en cada medición.", AutoSize = true, Dock = DockStyle.Bottom, Padding = new Padding(12, 0, 0, 8) };
            var close = UiHelper.CreateButton("Cerrar", (sender, e) => this.Close());
            this.CancelButton = close;
            this.Controls.Add(UiHelper.CreateButtonPanel(DockStyle.Bottom, close));
            this.Controls.Add(note);
            this.Controls.Add(table);
        }
    }
}
