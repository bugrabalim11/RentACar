using RentACar.MAUI.Views;

namespace RentACar.MAUI
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(CarDetailView), typeof(CarDetailView));
        }
    }
}
