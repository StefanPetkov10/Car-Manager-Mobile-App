using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarManagerApp.Data;
using CarManagerApp.Models;
using System.Diagnostics;

namespace CarManagerApp.ViewModels
{
    [QueryProperty(nameof(VehicleId), "VehicleId")]
    public partial class AddFuelViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty] private int vehicleId;
        [ObservableProperty] private DateTime date = DateTime.Today;
        [ObservableProperty] private string litersText = string.Empty;
        [ObservableProperty] private string pricePerLiterText = string.Empty;
        [ObservableProperty] private string odometerText = string.Empty;
        [ObservableProperty] private bool isFullTank;
        [ObservableProperty] private bool missedPreviousRecord;
        [ObservableProperty] private string stationName = string.Empty;

        public AddFuelViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Title = "Add Fuel Record";
        }

        [RelayCommand]
        private async Task SaveFuelRecordAsync()
        {
            if (!double.TryParse(LitersText, out double liters) || liters <= 0)
            {
                await Shell.Current.DisplayAlert("Validation", "Please enter a valid amount of liters.", "OK");
                return;
            }

            if (!double.TryParse(PricePerLiterText, out double price) || price <= 0)
            {
                await Shell.Current.DisplayAlert("Validation", "Please enter a valid price per liter.", "OK");
                return;
            }

            if (!double.TryParse(OdometerText, out double odometer) || odometer <= 0)
            {
                await Shell.Current.DisplayAlert("Validation", "Please enter a valid odometer reading.", "OK");
                return;
            }

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var record = new FuelRecord
                {
                    VehicleId = VehicleId,
                    Date = Date,
                    Liters = liters,
                    PricePerLiter = price,
                    TotalCost = Math.Round(liters * price, 2),
                    Odometer = odometer,
                    IsFullTank = IsFullTank,
                    MissedPreviousRecord = MissedPreviousRecord,
                    StationName = StationName.Trim()
                };

                await _databaseService.SaveFuelRecordAsync(record);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddFuelViewModel] Save failed: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Could not save fuel record. Please try again.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
