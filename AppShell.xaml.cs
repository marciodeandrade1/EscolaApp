using EscolaApp.Views;

namespace EscolaApp;

public partial class AppShell : Shell
{
    public AppShell()
    {
        InitializeComponent();

        // Rotas de navegação para as páginas de formulário (não aparecem nas abas)
        Routing.RegisterRoute(nameof(AlunoFormPage), typeof(AlunoFormPage));
        Routing.RegisterRoute(nameof(CursoFormPage), typeof(CursoFormPage));
        Routing.RegisterRoute(nameof(MatriculaFormPage), typeof(MatriculaFormPage));
    }
}
