# EscolaApp — Modelo .NET MAUI (Alunos, Cursos e Matrículas)

Projeto didático em **.NET MAUI (.NET 8)** demonstrando um CRUD completo com **SQLite** local,
padrão **MVVM** (CommunityToolkit.Mvvm) e um visual moderno (cards, cantos arredondados, sombras,
badges de status, swipe-to-delete, pull-to-refresh).

## Como abrir

1. Instale o workload MAUI (se ainda não tiver): `dotnet workload install maui`
2. Abra a pasta `EscolaApp` no Visual Studio 2022 (17.8+) ou `dotnet build` via CLI.
3. Rode em Windows, Android ou macOS (Mac Catalyst) — os pacotes NuGet (`sqlite-net-pcl`,
   `CommunityToolkit.Mvvm`) são restaurados automaticamente.
4. O banco `escola.db3` é criado automaticamente na primeira execução, dentro da pasta de dados
   do app (`FileSystem.AppDataDirectory`).

## Estrutura

```
EscolaApp/
├─ Models/            → Aluno, Curso, Matricula (entidades SQLite) + MatriculaDetalhada (DTO de exibição)
├─ Data/
│  └─ DatabaseService.cs  → toda a lógica de acesso ao SQLite (CRUD + regras de integridade)
├─ ViewModels/        → um ViewModel de listagem e um de formulário para cada entidade
├─ Views/             → páginas XAML (listagem + formulário) para cada entidade
├─ Converters/        → conversores usados nos bindings (bool invertido, status → cor, string vazia)
├─ Resources/Styles/   → Colors.xaml e Styles.xaml (design system do app)
├─ AppShell.xaml(.cs) → navegação por abas (TabBar) + rotas dos formulários
├─ MauiProgram.cs     → injeção de dependência (DatabaseService, ViewModels, Views)
└─ App.xaml(.cs)
```

## Conceitos demonstrados

- **MVVM com CommunityToolkit.Mvvm**: `[ObservableProperty]` e `[RelayCommand]` eliminam
  boilerplate de `INotifyPropertyChanged` e `ICommand`.
- **Injeção de dependência**: `DatabaseService` é `Singleton` (uma única conexão para o app
  todo); ViewModels e Views são `Transient`, resolvidos automaticamente pelo construtor.
- **Navegação Shell com parâmetros**: `[QueryProperty]` recebe o Id do registro a editar
  (ex.: `AlunoId`), permitindo reaproveitar a mesma página para "Novo" e "Editar".
- **Regra de integridade referencial**: não é possível excluir um Aluno ou Curso que já
  possua Matrículas vinculadas (validado em `DatabaseService`).
- **DTO para listagem**: `MatriculaDetalhada` junta dados de Aluno e Curso em memória para
  exibir nomes amigáveis na tela de Matrículas, sem precisar de JOIN SQL manual.
- **UI moderna**: `Frame` com `CornerRadius`/`HasShadow` simulando cards Material/Fluent,
  `SwipeView` para excluir com gesto, `RefreshView` para atualizar puxando a lista, badges
  coloridos de status via `IValueConverter`.

## Possíveis evoluções

- Trocar `sqlite-net-pcl` por **Entity Framework Core + Sqlite** (mais familiar vindo do
  mundo ASP.NET Core / EF que você já domina).
- Adicionar validação com **FluentValidation** ou `DataAnnotations`.
- Adicionar testes unitários nos ViewModels (mockando `DatabaseService`).
- Sincronizar com uma API ASP.NET Core (o mesmo `DatabaseService` local viraria um cache
  offline-first).
