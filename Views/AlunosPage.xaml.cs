using EscolaApp.ViewModels;

namespace EscolaApp.Views;

public partial class AlunosPage : ContentPage
{
    private readonly AlunosViewModel _viewModel;

    public AlunosPage(AlunosViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void ContentPage_NavigatedTo(object? sender, NavigatedToEventArgs e)
    {
        // Recarrega a lista sempre que a página volta ao foco (ex: após salvar/excluir)
        await _viewModel.CarregarCommand.ExecuteAsync(null);
    }
}
