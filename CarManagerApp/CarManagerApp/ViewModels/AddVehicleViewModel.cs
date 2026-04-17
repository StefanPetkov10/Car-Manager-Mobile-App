using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarManagerApp.Data;
using CarManagerApp.Models;
using System.Diagnostics;

namespace CarManagerApp.ViewModels
{
    public partial class AddVehicleViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty] private string make = string.Empty;
        [ObservableProperty] private string model = string.Empty;
        [ObservableProperty] private string yearText = string.Empty;
        [ObservableProperty] private string licensePlate = string.Empty;
        [ObservableProperty] private string initialOdometerText = string.Empty;

        public AddVehicleViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Title = "Add Vehicle";
        }

        [RelayCommand]
        private async Task SaveVehicleAsync()
        {
            if (string.IsNullOrWhiteSpace(Make) || string.IsNullOrWhiteSpace(Model))
            {
                await Shell.Current.DisplayAlert("Validation", "Make and Model are required.", "OK");
                return;
            }

            if (!int.TryParse(YearText, out int year) || year < 1900 || year > DateTime.Now.Year + 1)
            {
                await Shell.Current.DisplayAlert("Validation", "Please enter a valid year.", "OK");
                return;
            }

            if (!double.TryParse(InitialOdometerText, out double odometer) || odometer < 0)
            {
                await Shell.Current.DisplayAlert("Validation", "Please enter a valid odometer reading.", "OK");
                return;
            }

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var vehicle = new Vehicle
                {
                    Make = Make.Trim(),
                    Model = Model.Trim(),
                    Year = year,
                    LicensePlate = LicensePlate.Trim(),
                    InitialOdometer = odometer
                };

                await _databaseService.SaveVehicleAsync(vehicle);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddVehicleViewModel] Save failed: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Could not save vehicle. Please try again.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }
}
