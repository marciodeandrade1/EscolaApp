using SQLite;

namespace EscolaApp.Models;

public class Aluno
{
    [PrimaryKey, AutoIncrement]
    public int Id { get; set; }

    public string Nome { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Telefone { get; set; } = string.Empty;

    public DateTime DataNascimento { get; set; } = DateTime.Today.AddYears(-18);

    // Propriedade auxiliar somente para exibição (não persistida)
    [Ignore]
    public string Iniciais =>
        string.IsNullOrWhiteSpace(Nome)
            ? "?"
            : string.Concat(Nome.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                .Take(2)
                .Select(p => char.ToUpper(p[0])));
}
