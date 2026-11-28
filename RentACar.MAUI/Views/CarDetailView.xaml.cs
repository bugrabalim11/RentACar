using RentACar.MAUI.ViewModels;

namespace RentACar.MAUI.Views;

public partial class CarDetailView : ContentPage
{
    public CarDetailView(CarDetailViewModel carDetailViewModel)
    {
        InitializeComponent();
        BindingContext = carDetailViewModel;
    }
}