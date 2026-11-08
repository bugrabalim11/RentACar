namespace RentACar.MAUI
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
        }

# pragma warning disable CA1416
        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new Views.LoginView());
        }
# pragma warning restore CA1416
    }
}