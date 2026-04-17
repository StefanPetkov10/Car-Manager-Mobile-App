using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarManagerApp.Data;
using CarManagerApp.Models;
using CarManagerApp.Views;

namespace CarManagerApp.ViewModels
{
    public partial class VehiclesViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        public ObservableCollection<Vehicle> Cars { get; } = [];

        public VehiclesViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Title = "My Vehicles";
        }

        [RelayCommand]
        private async Task LoadCarsAsync()
        {
            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                var vehicles = await _databaseService.GetVehiclesAsync();

                if (Cars.Count > 0)
                    Cars.Clear();

                foreach (var vehicle in vehicles)
                    Cars.Add(vehicle);
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[VehiclesViewModel] Failed to load vehicles: {ex.Message}");
                await Shell.Current.DisplayAlert(
                    "Error",
                    "Could not load vehicles. Please try again.",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        [RelayCommand]
        private async Task AddCarAsync()
        {
            await Shell.Current.GoToAsync(nameof(AddVehiclePage));
        }

        [RelayCommand]
        private async Task GoToDetailsAsync(Vehicle vehicle)
        {
            if (vehicle is null)
                return;

            await Shell.Current.GoToAsync(
                nameof(VehicleDetailPage),
                new Dictionary<string, object>
                {
                    { "Vehicle", vehicle }
                });
        }
    }
}
