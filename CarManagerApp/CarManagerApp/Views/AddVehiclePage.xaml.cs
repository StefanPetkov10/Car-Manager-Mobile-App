using CarManagerApp.ViewModels;

namespace CarManagerApp.Views
{
    public partial class AddVehiclePage : ContentPage
    {
        public AddVehiclePage(AddVehicleViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
