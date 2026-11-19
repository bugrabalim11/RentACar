using RentACar.MAUI.ViewModels;

namespace RentACar.MAUI.Views;

public partial class LoginView : ContentPage
{
    public LoginView(LoginViewModel loıginViewModel)
    {
        InitializeComponent();
        BindingContext = loıginViewModel;
    }
}