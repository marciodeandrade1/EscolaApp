using EscolaApp.ViewModels;

namespace EscolaApp.Views;

public partial class MatriculasPage : ContentPage
{
    private readonly MatriculasViewModel _viewModel;

    public MatriculasPage(MatriculasViewModel viewModel)
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
