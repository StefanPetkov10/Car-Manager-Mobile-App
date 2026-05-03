using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarManagerApp.Data;
using CarManagerApp.Models;
using System.Diagnostics;

namespace CarManagerApp.ViewModels
{
    [QueryProperty(nameof(VehicleId), "VehicleId")]
    [QueryProperty(nameof(ServiceId), "ServiceId")]
    public partial class AddServiceViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty] private int vehicleId;
        [ObservableProperty] private int serviceId; // For editing

        [ObservableProperty] private DateTime date = DateTime.Today;
        [ObservableProperty] private string serviceType = string.Empty;
        [ObservableProperty] private string costText = string.Empty;
        [ObservableProperty] private string odometerText = string.Empty;
        [ObservableProperty] private string nextOdometerText = string.Empty;
        [ObservableProperty] private string description = string.Empty;
        [ObservableProperty] private string garageName = string.Empty;

        public List<string> AvailableServiceTypes { get; } = new()
        {
            "Oil Change",
            "Tires",
            "Brakes",
            "Battery",
            "Filter Replacement",
            "General Maintenance",
            "Repair",
            "Other"
        };

        public AddServiceViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Title = "Add Service Record";
            ServiceType = AvailableServiceTypes[0];
        }

        partial void OnServiceIdChanged(int value)
        {
            if (value > 0)
            {
                Title = "Edit Service Record";
                LoadServiceRecordAsync(value).FireAndForgetSafeAsync();
            }
        }

        private async Task LoadServiceRecordAsync(int id)
        {
            var service = await _databaseService.GetMaintenanceServiceAsync(id);
            if (service != null)
            {
                VehicleId = service.VehicleId;
                Date = service.Date;
                ServiceType = service.ServiceType;
                CostText = service.Cost.ToString("F2");
                OdometerText = service.Odometer.ToString("F0");
                if (service.NextServiceOdometer.HasValue)
                    NextOdometerText = service.NextServiceOdometer.Value.ToString("F0");
                Description = service.Description;
                GarageName = service.GarageName;
            }
        }

        [RelayCommand]
        private async Task SaveServiceRecordAsync()
        {
            if (string.IsNullOrWhiteSpace(ServiceType))
            {
                await Shell.Current.DisplayAlert("Validation", "Please select a service type.", "OK");
                return;
            }

            if (!double.TryParse(CostText, out double cost) || cost < 0)
            {
                await Shell.Current.DisplayAlert("Validation", "Please enter a valid cost.", "OK");
                return;
            }

            if (!double.TryParse(OdometerText, out double odometer) || odometer < 0)
            {
                await Shell.Current.DisplayAlert("Validation", "Please enter a valid odometer reading.", "OK");
                return;
            }

            if (IsBusy)
                return;

            try
            {
                IsBusy = true;

                double? nextOdometer = null;
                if (!string.IsNullOrWhiteSpace(NextOdometerText) && double.TryParse(NextOdometerText, out double parsedNextOdometer))
                {
                    nextOdometer = parsedNextOdometer;
                }

                var record = new MaintenanceService
                {
                    Id = ServiceId, // Will be 0 for new records
                    VehicleId = VehicleId,
                    Date = Date,
                    ServiceType = ServiceType,
                    Cost = cost,
                    Odometer = odometer,
                    NextServiceOdometer = nextOdometer,
                    Description = Description?.Trim() ?? string.Empty,
                    GarageName = GarageName?.Trim() ?? string.Empty
                };

                await _databaseService.SaveMaintenanceServiceAsync(record);
                await Shell.Current.GoToAsync("..");
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[AddServiceViewModel] Save failed: {ex.Message}");
                await Shell.Current.DisplayAlert("Error", "Could not save service record. Please try again.", "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }
    }

    public static class TaskExtensions
    {
        public static async void FireAndForgetSafeAsync(this Task task)
        {
            try
            {
                await task;
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[FireAndForget] Exception: {ex}");
            }
        }
    }
}
