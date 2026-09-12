using EscolaApp.ViewModels;

namespace EscolaApp.Views;

public partial class CursoFormPage : ContentPage
{
    public CursoFormPage(CursoFormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
