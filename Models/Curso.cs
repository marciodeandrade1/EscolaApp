using SQLite;

namespace EscolaApp.Models;

public class Curso
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Descricao { get; set; } = string.Empty;

    public int CargaHoraria { get; set; } // em horas

    [Ignore]
    public string CargaHorariaLabel => $"{CargaHoraria}h";
}
