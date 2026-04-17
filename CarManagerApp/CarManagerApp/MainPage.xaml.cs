using CarManagerApp.ViewModels;

namespace CarManagerApp
{
    public partial class MainPage : ContentPage
    {
        private readonly VehiclesViewModel _viewModel;

        public MainPage(VehiclesViewModel viewModel)
        {
            InitializeComponent();
            _viewModel = viewModel;
            BindingContext = _viewModel;
        }

        protected override async void OnAppearing()
        {
            base.OnAppearing();
            await _viewModel.LoadCarsCommand.ExecuteAsync(null);
        }
    }
}
