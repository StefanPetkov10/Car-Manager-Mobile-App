using CarManagerApp.Views;

namespace CarManagerApp
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            InitializeComponent();

            Routing.RegisterRoute(nameof(AddVehiclePage),   typeof(AddVehiclePage));
            Routing.RegisterRoute(nameof(VehicleDetailPage), typeof(VehicleDetailPage));
            Routing.RegisterRoute(nameof(AddFuelPage),       typeof(AddFuelPage));
        }
    }
}
