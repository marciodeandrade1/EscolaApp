using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using EscolaApp.Data;
using EscolaApp.Models;

namespace EscolaApp.ViewModels;

[QueryProperty(nameof(MatriculaId), "MatriculaId")]
public partial class MatriculaFormViewModel : ObservableObject
{
    private readonly DatabaseService _db;
    private int _matriculaId;
    private Matricula _matriculaOriginal = new();

    public ObservableCollection<Aluno> Alunos { get; } = new();
    public ObservableCollection<Curso> Cursos { get; } = new();
    public List<StatusMatricula> StatusDisponiveis { get; } =
        Enum.GetValues<StatusMatricula>().ToList();

    [ObservableProperty]
    private string titulo = "Nova Matrícula";

    [ObservableProperty]
    private Aluno? alunoSelecionado;

    [ObservableProperty]
    private Curso? cursoSelecionado;

    [ObservableProperty]
    private DateTime dataMatricula = DateTime.Today;

    [ObservableProperty]
    private StatusMatricula statusSelecionado = StatusMatricula.Ativa;

    [ObservableProperty]
    private string mensagemErro = string.Empty;

    public int MatriculaId
    {
        get => _matriculaId;
        set
        {
            _matriculaId = value;
            _ = CarregarSeEdicaoAsync();
        }
    }

    public MatriculaFormViewModel(DatabaseService db)
    {
        _db = db;
    }

    [RelayCommand]
    private async Task InicializarAsync()
    {
        var alunos = await _db.ListarAlunosAsync();
        Alunos.Clear();
        foreach (var aluno in alunos)
            Alunos.Add(aluno);

        var cursos = await _db.ListarCursosAsync();
        Cursos.Clear();
        foreach (var curso in cursos)
            Cursos.Add(curso);

        // Se já veio um Id de edição antes dos combos carregarem, reaplica a seleção
        if (_matriculaId > 0)
            await CarregarSeEdicaoAsync();
    }

    private async Task CarregarSeEdicaoAsync()
    {
        if (_matriculaId <= 0) return;

        var matricula = await _db.ObterMatriculaAsync(_matriculaId);
        if (matricula is null) return;

        _matriculaOriginal = matricula;
        Titulo = "Editar Matrícula";
        DataMatricula = matricula.DataMatricula;
        StatusSelecionado = matricula.Status;

        if (Alunos.Count > 0)
            AlunoSelecionado = Alunos.FirstOrDefault(a => a.Id == matricula.AlunoId);

        if (Cursos.Count > 0)
            CursoSelecionado = Cursos.FirstOrDefault(c => c.Id == matricula.CursoId);
    }

    [RelayCommand]
    private async Task SalvarAsync()
    {
        MensagemErro = string.Empty;

        if (AlunoSelecionado is null)
        {
            MensagemErro = "Selecione um aluno.";
            return;
        }

        if (CursoSelecionado is null)
        {
            MensagemErro = "Selecione um curso.";
            return;
        }

        _matriculaOriginal.AlunoId = AlunoSelecionado.Id;
        _matriculaOriginal.CursoId = CursoSelecionado.Id;
        _matriculaOriginal.DataMatricula = DataMatricula;
        _matriculaOriginal.Status = StatusSelecionado;

        await _db.SalvarMatriculaAsync(_matriculaOriginal);
        await Shell.Current.GoToAsync("..");
    }

    [RelayCommand]
    private async Task CancelarAsync()
    {
        await Shell.Current.GoToAsync("..");
    }
}
