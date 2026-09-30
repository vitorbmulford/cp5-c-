using GerenciamentoProdutos.Models;
using Microsoft.Data.Sqlite;

namespace GerenciamentoProdutos.Repositories;

public class ProdutoRepository
{
    private readonly string _connectionString;

    public ProdutoRepository(string connectionString)
    {
        _connectionString = connectionString;
        CriarTabelaSeNaoExistir();
    }

    public void Inserir(Produto produto)
    {
        const string sql = "INSERT INTO Produtos (Nome, Preco, Estoque, Categoria) VALUES (@Nome, @Preco, @Estoque, @Categoria)";
        using SqliteConnection connection = new(_connectionString);
        using SqliteCommand command = new(sql, connection);
        AdicionarParametrosProduto(command, produto);
        connection.Open();
        command.ExecuteNonQuery();
    }

    public List<Produto> Listar()
    {
        const string sql = "SELECT Id, Nome, Preco, Estoque, Categoria FROM Produtos ORDER BY Id";
        List<Produto> produtos = [];
        using SqliteConnection connection = new(_connectionString);
        using SqliteCommand command = new(sql, connection);
        connection.Open();
        using SqliteDataReader reader = command.ExecuteReader();
        while (reader.Read()) produtos.Add(MapearProduto(reader));
        return produtos;
    }

    public Produto? BuscarPorId(int id)
    {
        const string sql = "SELECT Id, Nome, Preco, Estoque, Categoria FROM Produtos WHERE Id = @Id";
        using SqliteConnection connection = new(_connectionString);
        using SqliteCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", id);
        connection.Open();
        using SqliteDataReader reader = command.ExecuteReader();
        return reader.Read() ? MapearProduto(reader) : null;
    }

    public bool Atualizar(Produto produto)
    {
        const string sql = "UPDATE Produtos SET Nome = @Nome, Preco = @Preco, Estoque = @Estoque, Categoria = @Categoria WHERE Id = @Id";
        using SqliteConnection connection = new(_connectionString);
        using SqliteCommand command = new(sql, connection);
        AdicionarParametrosProduto(command, produto);
        command.Parameters.AddWithValue("@Id", produto.Id);
        connection.Open();
        return command.ExecuteNonQuery() > 0;
    }

    public bool Excluir(int id)
    {
        const string sql = "DELETE FROM Produtos WHERE Id = @Id";
        using SqliteConnection connection = new(_connectionString);
        using SqliteCommand command = new(sql, connection);
        command.Parameters.AddWithValue("@Id", id);
        connection.Open();
        return command.ExecuteNonQuery() > 0;
    }

    private void CriarTabelaSeNaoExistir()
    {
        const string sql = "CREATE TABLE IF NOT EXISTS Produtos (Id INTEGER PRIMARY KEY AUTOINCREMENT, Nome TEXT NOT NULL, Preco DECIMAL(10,2) NOT NULL, Estoque INTEGER NOT NULL, Categoria TEXT NOT NULL);";
        using SqliteConnection connection = new(_connectionString);
        using SqliteCommand command = new(sql, connection);
        connection.Open();
        command.ExecuteNonQuery();
    }

    private static void AdicionarParametrosProduto(SqliteCommand command, Produto produto)
    {
        command.Parameters.AddWithValue("@Nome", produto.Nome);
        command.Parameters.AddWithValue("@Preco", produto.Preco);
        command.Parameters.AddWithValue("@Estoque", produto.Estoque);
        command.Parameters.AddWithValue("@Categoria", produto.Categoria);
    }

    private static Produto MapearProduto(SqliteDataReader reader) => new()
    {
        Id = reader.GetInt32(reader.GetOrdinal("Id")),
        Nome = reader.GetString(reader.GetOrdinal("Nome")),
        Preco = reader.GetDecimal(reader.GetOrdinal("Preco")),
        Estoque = reader.GetInt32(reader.GetOrdinal("Estoque")),
        Categoria = reader.GetString(reader.GetOrdinal("Categoria"))
    };
}
