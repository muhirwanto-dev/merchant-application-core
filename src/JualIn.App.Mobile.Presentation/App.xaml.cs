using CommunityToolkit.Maui.Alerts;
using HorusStudio.Maui.MaterialDesignControls;

namespace JualIn.App.Mobile.Presentation
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MaterialDesignControls.InitializeComponents();

            AppDomain.CurrentDomain.UnhandledException += AppDomain_UnhandledException;
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new AppShell());
        }

        private void AppDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            if (e.ExceptionObject is Exception exception)
            {
                Toast.Make(exception.Message);
            }
        }
    }
}