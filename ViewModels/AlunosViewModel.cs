using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EscolaApp.Data;
using EscolaApp.Models;

namespace EscolaApp.ViewModels;

public partial class AlunosViewModel : ObservableObject
{
    private readonly DatabaseService _db;

    public ObservableCollection<Aluno> Alunos { get; } = new();

    [ObservableProperty]
    private bool estaCarregando;

    [ObservableProperty]
    private bool listaVazia;

    public AlunosViewModel(DatabaseService db)
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
            var lista = await _db.ListarAlunosAsync();

            Alunos.Clear();
            foreach (var aluno in lista)
                Alunos.Add(aluno);

            ListaVazia = Alunos.Count == 0;
        }
        finally
        {
            EstaCarregando = false;
        }
    }

    [RelayCommand]
    private async Task NovoAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.AlunoFormPage));
    }

    [RelayCommand]
    private async Task EditarAsync(Aluno aluno)
    {
        if (aluno is null) return;

        var parametros = new Dictionary<string, object> { { "AlunoId", aluno.Id } };
        await Shell.Current.GoToAsync(nameof(Views.AlunoFormPage), parametros);
    }

    [RelayCommand]
    private async Task ExcluirAsync(Aluno aluno)
    {
        if (aluno is null) return;

        var confirmar = await Shell.Current.DisplayAlertAsync(
            "Excluir aluno",
            $"Deseja realmente excluir \"{aluno.Nome}\"?",
            "Excluir", "Cancelar");

        if (!confirmar) return;

        try
        {
            await _db.ExcluirAlunoAsync(aluno);
            Alunos.Remove(aluno);
            ListaVazia = Alunos.Count == 0;
        }
        catch (InvalidOperationException ex)
        {
            await Shell.Current.DisplayAlertAsync("Não é possível excluir", ex.Message, "OK");
        }
    }
}
