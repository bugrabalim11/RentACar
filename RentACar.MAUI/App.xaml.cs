namespace RentACar.MAUI
{
    public partial class App : Application
    {
        private readonly Views.LoginView _loginView;
        public App(Views.LoginView loginView)
        {
            InitializeComponent();
            _loginView = loginView;
        }

# pragma warning disable CA1416
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(_loginView);
        }
# pragma warning restore CA1416
    }
}