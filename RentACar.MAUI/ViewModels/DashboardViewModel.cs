using RentACar.MAUI.Models;
using RentACar.MAUI.Services.Abstract;
using System.Collections.ObjectModel;

namespace RentACar.MAUI.ViewModels
{
    public class DashboardViewModel
    {
        private readonly ICarService _carService;
        private readonly IDialogService _dialogService;

        // Araç eklenip veya silindiğinde ekranı anında yeniletir
        public ObservableCollection<CarResultDto> Cars { get; } = new();

        public DashboardViewModel(ICarService carService, IDialogService dialogService)
        {
            _carService = carService;
            _dialogService = dialogService;
        }

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
    }
}
