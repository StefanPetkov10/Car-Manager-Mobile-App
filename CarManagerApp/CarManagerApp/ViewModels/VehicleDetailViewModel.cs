using System.Collections.ObjectModel;
using System.Diagnostics;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CarManagerApp.Data;
using CarManagerApp.Models;

namespace CarManagerApp.ViewModels
{
    [QueryProperty(nameof(Vehicle), "Vehicle")]
    public partial class VehicleDetailViewModel : BaseViewModel
    {
        private readonly DatabaseService _databaseService;

        [ObservableProperty]
        [NotifyPropertyChangedFor(nameof(HasFuelData))]
        private Vehicle? vehicle;

        [ObservableProperty]
        private double averageFuelConsumption;

        public ObservableCollection<FuelRecord> FuelRecords { get; } = [];

        public bool HasFuelData => FuelRecords.Count > 0;

        public VehicleDetailViewModel(DatabaseService databaseService)
        {
            _databaseService = databaseService;
            Title = "Vehicle Details";
        }

        partial void OnVehicleChanged(Vehicle? value)
        {
            if (value is not null)
                Title = $"{value.Make} {value.Model}";
        }

        [RelayCommand]
        private async Task LoadDataAsync()
        {
            if (IsBusy || Vehicle is null)
                return;

            try
            {
                IsBusy = true;

                var records = await _databaseService.GetFuelRecordsForVehicleAsync(Vehicle.Id);

                if (FuelRecords.Count > 0)
                    FuelRecords.Clear();

                foreach (var record in records)
                    FuelRecords.Add(record);

                OnPropertyChanged(nameof(HasFuelData));

                AverageFuelConsumption = CalculateAverageConsumption();
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"[VehicleDetailViewModel] Failed to load data: {ex.Message}");
                await Shell.Current.DisplayAlert(
                    "Error",
                    "Could not load vehicle data. Please try again.",
                    "OK");
            }
            finally
            {
                IsBusy = false;
            }
        }

        /// <summary>
        /// Calculates average fuel consumption in L/100km.
        /// Requires at least 2 fuel records. Skips records where
        /// MissedPreviousRecord is true (broken odometer chain).
        /// </summary>
        private double CalculateAverageConsumption()
        {
            // Need at least 2 records to compute a distance delta
            if (FuelRecords.Count < 2)
                return 0;

            // Records are ordered descending by date; reverse for chronological order
            var chronological = FuelRecords.OrderBy(r => r.Date).ToList();

            double totalLiters = 0;
            double totalKilometers = 0;

            for (int i = 1; i < chronological.Count; i++)
            {
                var current = chronological[i];
                var previous = chronological[i - 1];

                // If the chain is broken, skip this interval — data is unreliable
                if (current.MissedPreviousRecord)
                    continue;

                double kmDelta = current.Odometer - previous.Odometer;

                // Guard against zero or negative odometer delta (bad data entry)
                if (kmDelta <= 0)
                    continue;

                totalLiters += current.Liters;
                totalKilometers += kmDelta;
            }

            if (totalKilometers <= 0)
                return 0;

            return Math.Round((totalLiters / totalKilometers) * 100, 2);
        }
    }
}
