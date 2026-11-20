using RentACar.MAUI.ViewModels;

namespace RentACar.MAUI.Views;

public partial class DashboardView : ContentPage
{
    public DashboardView(DashboardViewModel dashboardViewModel)
    {
        InitializeComponent();
        BindingContext = dashboardViewModel;
    }
}