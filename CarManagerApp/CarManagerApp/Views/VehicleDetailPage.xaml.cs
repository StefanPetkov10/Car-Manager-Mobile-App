using CarManagerApp.ViewModels;

namespace CarManagerApp.Views
{
    public partial class VehicleDetailPage : ContentPage
    {
        private readonly VehicleDetailViewModel _viewModel;

        public VehicleDetailPage(VehicleDetailViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadDataCommand.ExecuteAsync(null);
        }

        private async void OnAddFuelClicked(object sender, EventArgs e)
        {
            if (_viewModel.Vehicle is null)
                return;

            await Shell.Current.GoToAsync(
                nameof(AddFuelPage),
                new Dictionary<string, object>
                {
                    { "VehicleId", _viewModel.Vehicle.Id }
                });
        }
    }
}
