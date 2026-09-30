# Gerenciamento de Produtos com C# e ADO.NET

Projeto acadêmico de console que demonstra CRUD de produtos com C#, SQLite e ADO.NET, sem ORM.

## Tecnologias utilizadas

- C# e .NET 10
- SQLite (banco local em arquivo, sem servidor)
- ADO.NET com `Microsoft.Data.Sqlite`
- `Microsoft.Extensions.Configuration.Json` para ler o `appsettings.json`

## Estrutura do projeto

```text
GerenciamentoProdutos/
├── Models/Produto.cs                 # Modelo da tabela
├── Repositories/ProdutoRepository.cs # CRUD ADO.NET
├── Services/Logger.cs                # Registro das operações
├── Program.cs                         # Menu de console
├── appsettings.json                   # Connection string
├── database.sql                       # Tabela Produtos
└── GerenciamentoProdutos.csproj       # Projeto e pacotes
```

## Requisitos

- .NET SDK 10 (ou ajuste o TargetFramework conforme o SDK instalado)
- Nenhum servidor de banco de dados adicional

## Configuração do banco de dados

1. Não é necessário instalar ou iniciar um servidor de banco de dados.
2. Na primeira execução, a aplicação cria automaticamente o arquivo `produtos.db` e a tabela `Produtos`.
3. O arquivo `database.sql` contém o script equivalente para consulta ou execução manual.
4. Confira a connection string no `appsettings.json`:

```json
"DefaultConnection": "Data Source=produtos.db"
```

O arquivo `produtos.db` será criado na pasta em que `dotnet run` for executado.

## Como executar

Na pasta do projeto, execute:

```bash
dotnet restore
dotnet run
```

## Funcionalidades

- Inserir produto
- Listar produtos
- Buscar produto por ID
- Atualizar produto
- Excluir produto
- Registrar operações em `bin/.../logs/operacoes.txt`

Escritas usam `ExecuteNonQuery`; leituras usam `ExecuteReader`; e o `SqlDataReader` é mapeado manualmente para `Produto`. Todas as entradas SQL usam parâmetros para evitar SQL Injection.

## Demonstração

### Inserção de produto

[Adicionar print]

### Listagem de produtos

[Adicionar print]

### Atualização ou exclusão

[Adicionar print]

## Roteiro de apresentação (3 a 5 minutos)

1. Mostre a estrutura e explique: `Program` controla o menu, `Produto` representa dados, repositório acessa o banco.
2. Abra `appsettings.json` e mostre a connection string fora do código.
3. Mostre `Produto.cs` e suas propriedades equivalentes às colunas.
4. Mostre o `INSERT` parametrizado e `ExecuteNonQuery` no repositório.
5. Mostre o `SELECT`, `ExecuteReader` e `MapearProduto`.
6. Execute o programa: insira um produto, liste/busque, atualize e exclua.
7. Abra `logs/operacoes.txt` na pasta de execução.
