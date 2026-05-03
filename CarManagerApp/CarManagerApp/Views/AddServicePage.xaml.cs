using CarManagerApp.ViewModels;

namespace CarManagerApp.Views;

public partial class AddServicePage : ContentPage
{
    public AddServicePage(AddServiceViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
