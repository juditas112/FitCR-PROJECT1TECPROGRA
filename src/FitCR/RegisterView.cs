using FitCRController;

namespace FitCR
{
    /// <summary>View to register a new basic user.</summary>
    public class RegisterView : Form
    {
        private readonly TextBox txtName = new TextBox();
        private readonly TextBox txtLastName = new TextBox();
        private readonly TextBox txtEmail = new TextBox();
        private readonly TextBox txtPhone = new TextBox();
        private readonly TextBox txtUsername = new TextBox();
        private readonly TextBox txtPassword = new TextBox { UseSystemPasswordChar = true };
        private readonly DateTimePicker dtBirthDate = new DateTimePicker { Format = DateTimePickerFormat.Short, Value = new DateTime(2000, 1, 1, 0, 0, 0, DateTimeKind.Local) };
        private readonly NumericUpDown numWeight = UiHelper.CreateNumber(20, 300, 1, 70);
        private readonly NumericUpDown numHeight = UiHelper.CreateNumber(1, 2.5M, 2, 1.7M);

        public RegisterView(ControllerContainer controllers)
        {
            this.Controllers = controllers;
            this.BuildLayout();
        }

        private ControllerContainer Controllers { get; }

        public string RegisteredUsername { get; private set; } = string.Empty;

        private void BuildLayout()
        {
            UiHelper.ConfigureDialog(this, "Fit.CR - Registro de usuario", 420, 470);
            var table = UiHelper.CreateFormTable();
            UiHelper.AddRow(table, "Nombre", this.txtName);
            UiHelper.AddRow(table, "Apellidos", this.txtLastName);
            UiHelper.AddRow(table, "Correo", this.txtEmail);
            UiHelper.AddRow(table, "Teléfono", this.txtPhone);
            UiHelper.AddRow(table, "Usuario", this.txtUsername);
            UiHelper.AddRow(table, "Contraseña", this.txtPassword);
            UiHelper.AddRow(table, "Fecha de nacimiento", this.dtBirthDate);
            UiHelper.AddRow(table, "Peso (kg)", this.numWeight);
            UiHelper.AddRow(table, "Estatura (m)", this.numHeight);

            var save = UiHelper.CreateButton("Registrar", this.BtnRegister_Click);
            var cancel = UiHelper.CreateButton("Cancelar", (sender, e) => this.Close());
            this.AcceptButton = save;
            this.CancelButton = cancel;
            this.Controls.Add(UiHelper.CreateButtonPanel(DockStyle.Bottom, save, cancel));
            this.Controls.Add(table);
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            var result = this.Controllers.Users.Register(
                this.txtName.Text,
                this.txtLastName.Text,
                this.txtEmail.Text,
                this.txtPhone.Text,
                this.txtUsername.Text,
                this.txtPassword.Text,
                this.dtBirthDate.Value,
                (double)this.numWeight.Value,
                (double)this.numHeight.Value);

            UiHelper.ShowResult(result);
            if (result.Success)
            {
                this.RegisteredUsername = this.txtUsername.Text.Trim();
                this.DialogResult = DialogResult.OK;
            }
        }
    }
}
