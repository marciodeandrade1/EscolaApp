using EscolaApp.Models;
using SQLite;

namespace EscolaApp.Data;

/// <summary>
/// Encapsula toda a comunicação com o banco SQLite local.
/// Registrado como Singleton no MauiProgram para reaproveitar a mesma conexão.
/// </summary>
public class DatabaseService
{
    private SQLiteAsyncConnection? _conexao;

    private async Task<SQLiteAsyncConnection> ObterConexaoAsync()
    {
        if (_conexao is not null)
            return _conexao;

        var caminhoBanco = Path.Combine(FileSystem.AppDataDirectory, "escola.db3");
        _conexao = new SQLiteAsyncConnection(caminhoBanco);

        await _conexao.CreateTableAsync<Aluno>();
        await _conexao.CreateTableAsync<Curso>();
        await _conexao.CreateTableAsync<Matricula>();

        return _conexao;
    }

    // ---------------- ALUNOS ----------------

    public async Task<List<Aluno>> ListarAlunosAsync()
    {
        var conexao = await ObterConexaoAsync();
        return await conexao.Table<Aluno>().OrderBy(a => a.Nome).ToListAsync();
    }

    public async Task<Aluno?> ObterAlunoAsync(int id)
    {
        var conexao = await ObterConexaoAsync();
        return await conexao.Table<Aluno>().FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<int> SalvarAlunoAsync(Aluno aluno)
    {
        var conexao = await ObterConexaoAsync();
        if (aluno.Id != 0)
            return await conexao.UpdateAsync(aluno);

        return await conexao.InsertAsync(aluno);
    }

    public async Task<int> ExcluirAlunoAsync(Aluno aluno)
    {
        var conexao = await ObterConexaoAsync();

        // Regra de integridade: não permite excluir aluno com matrícula ativa
        var possuiMatricula = await conexao.Table<Matricula>()
            .Where(m => m.AlunoId == aluno.Id)
            .CountAsync();

        if (possuiMatricula > 0)
            throw new InvalidOperationException("Este aluno possui matrículas vinculadas e não pode ser excluído.");

        return await conexao.DeleteAsync(aluno);
    }

    // ---------------- CURSOS ----------------

    public async Task<List<Curso>> ListarCursosAsync()
    {
        var conexao = await ObterConexaoAsync();
        return await conexao.Table<Curso>().OrderBy(c => c.Nome).ToListAsync();
    }

    public async Task<Curso?> ObterCursoAsync(int id)
    {
        var conexao = await ObterConexaoAsync();
        return await conexao.Table<Curso>().FirstOrDefaultAsync(c => c.Id == id);
    }

    public async Task<int> SalvarCursoAsync(Curso curso)
    {
        var conexao = await ObterConexaoAsync();
        if (curso.Id != 0)
            return await conexao.UpdateAsync(curso);

        return await conexao.InsertAsync(curso);
    }

    public async Task<int> ExcluirCursoAsync(Curso curso)
    {
        var conexao = await ObterConexaoAsync();

        var possuiMatricula = await conexao.Table<Matricula>()
            .Where(m => m.CursoId == curso.Id)
            .CountAsync();

        if (possuiMatricula > 0)
            throw new InvalidOperationException("Este curso possui matrículas vinculadas e não pode ser excluído.");

        return await conexao.DeleteAsync(curso);
    }

    // ---------------- MATRICULAS ----------------

    public async Task<List<MatriculaDetalhada>> ListarMatriculasDetalhadasAsync()
    {
        var conexao = await ObterConexaoAsync();

        var matriculas = await conexao.Table<Matricula>().ToListAsync();
        var alunos = await conexao.Table<Aluno>().ToListAsync();
        var cursos = await conexao.Table<Curso>().ToListAsync();

        var resultado = matriculas
            .Select(m => new MatriculaDetalhada
            {
                Id = m.Id,
                AlunoId = m.AlunoId,
                CursoId = m.CursoId,
                NomeAluno = alunos.FirstOrDefault(a => a.Id == m.AlunoId)?.Nome ?? "(aluno removido)",
                NomeCurso = cursos.FirstOrDefault(c => c.Id == m.CursoId)?.Nome ?? "(curso removido)",
                DataMatricula = m.DataMatricula,
                Status = m.Status
            })
            .OrderByDescending(m => m.DataMatricula)
            .ToList();

        return resultado;
    }

    public async Task<Matricula?> ObterMatriculaAsync(int id)
    {
        var conexao = await ObterConexaoAsync();
        return await conexao.Table<Matricula>().FirstOrDefaultAsync(m => m.Id == id);
    }

    public async Task<int> SalvarMatriculaAsync(Matricula matricula)
    {
        var conexao = await ObterConexaoAsync();
        if (matricula.Id != 0)
            return await conexao.UpdateAsync(matricula);

        return await conexao.InsertAsync(matricula);
    }

    public async Task<int> ExcluirMatriculaAsync(Matricula matricula)
    {
        var conexao = await ObterConexaoAsync();
        return await conexao.DeleteAsync(matricula);
    }
}
