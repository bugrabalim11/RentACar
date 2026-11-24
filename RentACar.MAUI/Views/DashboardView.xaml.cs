using RentACar.MAUI.ViewModels;

namespace RentACar.MAUI.Views;

public partial class DashboardView : ContentPage
{
    private readonly DashboardViewModel _dashboardViewModel;
    public DashboardView(DashboardViewModel dashboardViewModel)
    {
        InitializeComponent();
        _dashboardViewModel = dashboardViewModel;
        BindingContext = dashboardViewModel;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();
        await _dashboardViewModel.LoadCarsAsync();
    }
}