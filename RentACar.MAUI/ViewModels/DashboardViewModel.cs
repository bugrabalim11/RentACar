using RentACar.MAUI.Models;
using RentACar.MAUI.Services.Abstract;
using RentACar.MAUI.Views;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace RentACar.MAUI.ViewModels
{
    public class DashboardViewModel
    {
        private readonly ICarService _carService;
        private readonly IDialogService _dialogService;
        public ICommand SelectCarCommand { get; }

        public DashboardViewModel(ICarService carService, IDialogService dialogService)
        {
            _carService = carService;
            _dialogService = dialogService;
            SelectCarCommand = new Command<CarResultDto>(async (car) => await GoToCarDetailAsync(car));
        }
        public ObservableCollection<CarResultDto> Cars { get; } = new();

        public async Task LoadCarsAsync()
        {
            try
            {
                var carList = await _carService.GetAllCarsAsync();

                Cars.Clear();
                foreach (var car in carList)
                {
                    Cars.Add(car);
                }
            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Hata", $"Araçlar yüklenirken bir sorun oluştu: {ex.Message}");
            }
        }

        private async Task GoToCarDetailAsync(CarResultDto car)
        {
            if (car == null) return;
            await Shell.Current.GoToAsync($"{nameof(CarDetailView)}?CarId={car.Id}");
        }
    }
}
