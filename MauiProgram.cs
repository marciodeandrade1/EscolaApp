using EscolaApp.Data;
using EscolaApp.ViewModels;
using EscolaApp.Views;
using Microsoft.Extensions.Logging;

namespace EscolaApp;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UseMauiApp<App>()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
            });

        // Banco de dados: uma única instância compartilhada por todo o app
        builder.Services.AddSingleton<DatabaseService>();

        // Alunos
        builder.Services.AddTransient<AlunosViewModel>();
        builder.Services.AddTransient<AlunosPage>();
        builder.Services.AddTransient<AlunoFormViewModel>();
        builder.Services.AddTransient<AlunoFormPage>();

        // Cursos
        builder.Services.AddTransient<CursosViewModel>();
        builder.Services.AddTransient<CursosPage>();
        builder.Services.AddTransient<CursoFormViewModel>();
        builder.Services.AddTransient<CursoFormPage>();

        // Matrículas
        builder.Services.AddTransient<MatriculasViewModel>();
        builder.Services.AddTransient<MatriculasPage>();
        builder.Services.AddTransient<MatriculaFormViewModel>();
        builder.Services.AddTransient<MatriculaFormPage>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

        return builder.Build();
    }
}
