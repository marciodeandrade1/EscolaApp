using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EscolaApp.Data;
using EscolaApp.Models;

namespace EscolaApp.ViewModels;

[QueryProperty(nameof(CursoId), "CursoId")]
public partial class CursoFormViewModel : ObservableObject
{
    private readonly DatabaseService _db;
    private int _cursoId;
    private Curso _cursoOriginal = new();

    [ObservableProperty]
    private string titulo = "Novo Curso";

    [ObservableProperty]
    private string nome = string.Empty;

    [ObservableProperty]
    private string descricao = string.Empty;

    [ObservableProperty]
    private int cargaHoraria = 40;

    [ObservableProperty]
    private string mensagemErro = string.Empty;

    public int CursoId
    {
        get => _cursoId;
        set
        {
            _cursoId = value;
            _ = CarregarSeEdicaoAsync();
        }
    }

    public CursoFormViewModel(DatabaseService db)
    {
        _db = db;
    }

    private async Task CarregarSeEdicaoAsync()
    {
        if (_cursoId <= 0) return;

        var curso = await _db.ObterCursoAsync(_cursoId);
        if (curso is null) return;

        _cursoOriginal = curso;
        Titulo = "Editar Curso";
        Nome = curso.Nome;
        Descricao = curso.Descricao;
        CargaHoraria = curso.CargaHoraria;
    }

    [RelayCommand]
    private async Task SalvarAsync()
    {
        MensagemErro = string.Empty;

        if (string.IsNullOrWhiteSpace(Nome))
        {
            MensagemErro = "Informe o nome do curso.";
            return;
        }

        if (CargaHoraria <= 0)
        {
            MensagemErro = "A carga horária deve ser maior que zero.";
            return;
        }

        _cursoOriginal.Nome = Nome.Trim();
        _cursoOriginal.Descricao = Descricao.Trim();
        _cursoOriginal.CargaHoraria = CargaHoraria;

        await _db.SalvarCursoAsync(_cursoOriginal);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelarAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
