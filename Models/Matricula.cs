using SQLite;

namespace EscolaApp.Models;

public enum StatusMatricula
{
    Ativa,
    Concluida,
    Cancelada
}

public class Matricula
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    [Indexed]
    public int AlunoId { get; set; }

    [Indexed]
    public int CursoId { get; set; }

    public DateTime DataMatricula { get; set; } = DateTime.Today;

    public StatusMatricula Status { get; set; } = StatusMatricula.Ativa;
}

// DTO usado nas listagens: junta dados de Aluno e Curso para exibição amigável
public class MatriculaDetalhada
{
    public int Id { get; set; }
    public int AlunoId { get; set; }
    public int CursoId { get; set; }
    public string NomeAluno { get; set; } = string.Empty;
    public string NomeCurso { get; set; } = string.Empty;
    public DateTime DataMatricula { get; set; }
    public StatusMatricula Status { get; set; }

    public string StatusLabel => Status switch
    {
        StatusMatricula.Ativa => "Ativa",
        StatusMatricula.Concluida => "Concluída",
        StatusMatricula.Cancelada => "Cancelada",
        _ => Status.ToString()
    };

    public string DataMatriculaLabel => DataMatricula.ToString("dd/MM/yyyy");
}
