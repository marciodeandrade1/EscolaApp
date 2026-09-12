using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EscolaApp.Data;
using EscolaApp.Models;

namespace EscolaApp.ViewModels;

public partial class MatriculasViewModel : ObservableObject
{
    private readonly DatabaseService _db;

    public ObservableCollection<MatriculaDetalhada> Matriculas { get; } = new();

    [ObservableProperty]
    private bool estaCarregando;

    [ObservableProperty]
    private bool listaVazia;

    public MatriculasViewModel(DatabaseService db)
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
            var lista = await _db.ListarMatriculasDetalhadasAsync();

            Matriculas.Clear();
            foreach (var matricula in lista)
                Matriculas.Add(matricula);

            ListaVazia = Matriculas.Count == 0;
        }
        finally
        {
            EstaCarregando = false;
        }
    }

    [RelayCommand]
    private async Task NovaAsync()
    {
        await Shell.Current.GoToAsync(nameof(Views.MatriculaFormPage));
    }

    [RelayCommand]
    private async Task EditarAsync(MatriculaDetalhada matricula)
    {
        if (matricula is null) return;

        var parametros = new Dictionary<string, object> { { "MatriculaId", matricula.Id } };
        await Shell.Current.GoToAsync(nameof(Views.MatriculaFormPage), parametros);
    }

    [RelayCommand]
    private async Task ExcluirAsync(MatriculaDetalhada matricula)
    {
        if (matricula is null) return;

        var confirmar = await Shell.Current.DisplayAlertAsync(
            "Excluir matrícula",
            $"Deseja realmente excluir a matrícula de \"{matricula.NomeAluno}\" em \"{matricula.NomeCurso}\"?",
            "Excluir", "Cancelar");

        if (!confirmar) return;

        var entidade = await _db.ObterMatriculaAsync(matricula.Id);
        if (entidade is not null)
            await _db.ExcluirMatriculaAsync(entidade);

        Matriculas.Remove(matricula);
        ListaVazia = Matriculas.Count == 0;
    }
}
