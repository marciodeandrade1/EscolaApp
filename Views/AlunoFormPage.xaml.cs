using EscolaApp.ViewModels;

namespace EscolaApp.Views;

public partial class AlunoFormPage : ContentPage
{
    public AlunoFormPage(AlunoFormViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}
