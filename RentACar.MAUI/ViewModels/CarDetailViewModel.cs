using RentACar.MAUI.Models;
using RentACar.MAUI.Services.Abstract;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace RentACar.MAUI.ViewModels
{
    public class CarDetailViewModel : INotifyPropertyChanged
    {
        private readonly ICarService _carService;
        private readonly IDialogService _dialogService;
        private CarDetailDto? _carDetail;
        public CarDetailDto? CarDetail { get { return _carDetail; } set { _carDetail = value; OnPropertyChanged(); } }
        public ObservableCollection<string> CarImages { get; } = new();

        public CarDetailViewModel(ICarService carService, IDialogService dialogService)
        {
            _carService = carService;
            _dialogService = dialogService;
        }

        public event PropertyChangedEventHandler? PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string? propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public async Task LoadCarDetailsAsync(int carId)
        {
            try
            {
                var carDetails = await _carService.GetCarDetailAsync(carId);
                if (!carDetails.Success || carDetails.Data == null)
                {
                    await _dialogService.ShowAlertAsync("Hata", carDetails.Message ?? "Araç detayı getirilemedi.");
                    return;
                }
                CarDetail = carDetails.Data;
                CarImages.Clear();
                if (carDetails.Data.CoverImagesUrl != null)
                {
                    foreach (var url in carDetails.Data.CoverImagesUrl)
                    {
                        if (!string.IsNullOrWhiteSpace(url))
                        {
                            var fixedUrl = url.Replace("https://localhost:7085", "http://10.0.2.2:5210");
                            CarImages.Add(fixedUrl);
                        }
                    }
                }

            }
            catch (Exception ex)
            {
                await _dialogService.ShowAlertAsync("Hata", $"Araç detayları yüklenirken hata oluştu: {ex.Message}");
            }
        }
    }
}
