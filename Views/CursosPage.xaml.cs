using EscolaApp.ViewModels;

namespace EscolaApp.Views;

public partial class CursosPage : ContentPage
{
    private readonly CursosViewModel _viewModel;

    public CursosPage(CursosViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void ContentPage_NavigatedTo(object? sender, NavigatedToEventArgs e)
    {
        await _viewModel.CarregarCommand.ExecuteAsync(null);
    }
}
