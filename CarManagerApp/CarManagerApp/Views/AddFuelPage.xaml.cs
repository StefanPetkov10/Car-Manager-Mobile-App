using CarManagerApp.ViewModels;

namespace CarManagerApp.Views
{
    public partial class AddFuelPage : ContentPage
    {
        public AddFuelPage(AddFuelViewModel viewModel)
        {
            InitializeComponent();
            BindingContext = viewModel;
        }
    }
}
