using EscolaApp.ViewModels;

namespace EscolaApp.Views;

public partial class MatriculaFormPage : ContentPage
{
    private readonly MatriculaFormViewModel _viewModel;

    public MatriculaFormPage(MatriculaFormViewModel viewModel)
    {
        InitializeComponent();
        _viewModel = viewModel;
        BindingContext = _viewModel;
    }

    private async void ContentPage_NavigatedTo(object? sender, NavigatedToEventArgs e)
    {
        // Carrega as listas de Alunos e Cursos para os Pickers antes de aplicar a seleção de edição
        await _viewModel.InicializarCommand.ExecuteAsync(null);
    }
}
