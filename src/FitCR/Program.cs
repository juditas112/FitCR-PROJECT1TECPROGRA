using FitCRController;

namespace FitCR
{
    internal static class Program
    {
        [STAThread]
        private static void Main()
        {
            ApplicationConfiguration.Initialize();
            var controllers = new ControllerContainer(new FileHandler());
            Application.Run(new LoginView(controllers));
        }
    }
}
