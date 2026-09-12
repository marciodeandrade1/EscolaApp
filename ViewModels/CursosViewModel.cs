using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EscolaApp.Data;
using EscolaApp.Models;

namespace EscolaApp.ViewModels;

public partial class CursosViewModel : ObservableObject
{
    private readonly DatabaseService _db;

    public ObservableCollection<Curso> Cursos { get; } = new();

    [ObservableProperty]
    private bool estaCarregando;

    [ObservableProperty]
    private bool listaVazia;

    public CursosViewModel(DatabaseService db)
    {
        _db = db;
    }

    [RelayCommand]
    private async Task CarregarAsync()
    {
        if (EstaCarregando) return;

        try
        {
            EstaCarregando = true;
            var lista = await _db.ListarCursosAsync();

            Cursos.Clear();
            foreach (var curso in lista)
                Cursos.Add(curso);

            ListaVazia = Cursos.Count == 0;
        }
        finally
        {
            EstaCarregando = false;
        }
    }

    [RelayCommand]
    private async Task NovoAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.CursoFormPage));
    }

    [RelayCommand]
    private async Task EditarAsync(Curso curso)
    {
        if (curso is null) return;

        var parametros = new Dictionary<string, object> { { "CursoId", curso.Id } };
        await Shell.Current.GoToAsync(nameof(Views.CursoFormPage), parametros);
    }

    [RelayCommand]
    private async Task ExcluirAsync(Curso curso)
    {
        if (curso is null) return;

        var confirmar = await Shell.Current.DisplayAlertAsync(
            "Excluir curso",
            $"Deseja realmente excluir \"{curso.Nome}\"?",
            "Excluir", "Cancelar");

        if (!confirmar) return;

        try
        {
            await _db.ExcluirCursoAsync(curso);
            Cursos.Remove(curso);
            ListaVazia = Cursos.Count == 0;
        }
        catch (InvalidOperationException ex)
        {
            await Shell.Current.DisplayAlertAsync("Não é possível excluir", ex.Message, "OK");
        }
    }
}
