using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EscolaApp.Data;
using EscolaApp.Models;

namespace EscolaApp.ViewModels;

[QueryProperty(nameof(AlunoId), "AlunoId")]
public partial class AlunoFormViewModel : ObservableObject
{
    private readonly DatabaseService _db;
    private int _alunoId;
    private Aluno _alunoOriginal = new();

    [ObservableProperty]
    private string titulo = "Novo Aluno";

    [ObservableProperty]
    private string nome = string.Empty;

    [ObservableProperty]
    private string email = string.Empty;

    [ObservableProperty]
    private string telefone = string.Empty;

    [ObservableProperty]
    private DateTime dataNascimento = DateTime.Today.AddYears(-18);

    [ObservableProperty]
    private string mensagemErro = string.Empty;

    public int AlunoId
    {
        get => _alunoId;
        set
        {
            _alunoId = value;
            _ = CarregarSeEdicaoAsync();
        }
    }

    public AlunoFormViewModel(DatabaseService db)
    {
        _db = db;
    }

    private async Task CarregarSeEdicaoAsync()
    {
        if (_alunoId <= 0) return;

        var aluno = await _db.ObterAlunoAsync(_alunoId);
        if (aluno is null) return;

        _alunoOriginal = aluno;
        Titulo = "Editar Aluno";
        Nome = aluno.Nome;
        Email = aluno.Email;
        Telefone = aluno.Telefone;
        DataNascimento = aluno.DataNascimento;
    }

    [RelayCommand]
    private async Task SalvarAsync()
    {
        MensagemErro = string.Empty;

        if (string.IsNullOrWhiteSpace(Nome))
        {
            MensagemErro = "Informe o nome do aluno.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Email) || !Email.Contains('@'))
        {
            MensagemErro = "Informe um e-mail válido.";
            return;
        }

        _alunoOriginal.Nome = Nome.Trim();
        _alunoOriginal.Email = Email.Trim();
        _alunoOriginal.Telefone = Telefone.Trim();
        _alunoOriginal.DataNascimento = DataNascimento;

        await _db.SalvarAlunoAsync(_alunoOriginal);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelarAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
