using FitCRController;
using FitCRModels.Enums;

namespace FitCR
{
    /// <summary>Login view. After the login it opens the menu that matches the role of the person.</summary>
    public class LoginView : Form
    {
        private readonly TextBox txtUser = new TextBox();
        private readonly TextBox txtPassword = new TextBox { UseSystemPasswordChar = true };

        public LoginView(ControllerContainer controllers)
        {
            this.Controllers = controllers;
            this.BuildLayout();
            this.Shown += this.LoginView_Shown;
        }

        private ControllerContainer Controllers { get; }

        private void BuildLayout()
        {
            this.Text = "Fit.CR - Iniciar sesión";
            this.ClientSize = new Size(400, 270);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;

            var title = new Label { Text = "Fit.CR", Font = new Font("Segoe UI", 22F, FontStyle.Bold), Dock = DockStyle.Top, Height = 55, Padding = new Padding(12, 10, 0, 0) };
            var table = UiHelper.CreateFormTable();
            UiHelper.AddRow(table, "Usuario o correo", this.txtUser);
            UiHelper.AddRow(table, "Contraseña", this.txtPassword);

            var login = UiHelper.CreateButton("Ingresar", this.BtnLogin_Click);
            var register = UiHelper.CreateButton("Registrarme", this.BtnRegister_Click);
            var buttons = UiHelper.CreateButtonPanel(DockStyle.Top, login, register);
            buttons.Padding = new Padding(12, 8, 0, 0);

            this.AcceptButton = login;
            this.Controls.Add(buttons);
            this.Controls.Add(table);
            this.Controls.Add(title);
        }

        private void LoginView_Shown(object? sender, EventArgs e)
        {
            var errors = this.Controllers.InformationService.LoadErrors;
            if (errors.Count > 0)
            {
                MessageBox.Show(string.Join(Environment.NewLine, errors.Take(5)), "Problemas al cargar los archivos", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
        }

        private void BtnLogin_Click(object? sender, EventArgs e)
        {
            if (!this.Controllers.Login.Login(this.txtUser.Text, this.txtPassword.Text))
            {
                MessageBox.Show("Invalid username or password.", "Login Failed", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            this.Hide();
            using (var menu = this.CreateMenu())
            {
                menu.ShowDialog();
            }

            this.Controllers.Login.Logout();
            this.txtPassword.Clear();
            this.Show();
        }

        private void BtnRegister_Click(object? sender, EventArgs e)
        {
            using var registerView = new RegisterView(this.Controllers);
            if (registerView.ShowDialog(this) == DialogResult.OK)
            {
                this.txtUser.Text = registerView.RegisteredUsername;
                this.txtPassword.Focus();
            }
        }

        private Form CreateMenu()
        {
            return this.Controllers.InformationService.CurrentPerson?.Role == Role.Trainer
                ? new TrainerMenuView(this.Controllers)
                : new UserMenuView(this.Controllers);
        }
    }
}
