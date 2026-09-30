using System.Globalization;
using GerenciamentoProdutos.Models;
using GerenciamentoProdutos.Repositories;
using GerenciamentoProdutos.Services;
using Microsoft.Extensions.Configuration;

namespace GerenciamentoProdutos;

internal class Program
{
    private static ProdutoRepository _produtoRepository = null!;

    private static void Main()
    {
        try
        {
            IConfiguration configuracao = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json", optional: false).Build();
            string? connectionString = configuracao.GetConnectionString("DefaultConnection");
            if (string.IsNullOrWhiteSpace(connectionString)) { Console.WriteLine("A connection string 'DefaultConnection' não foi encontrada."); return; }
            _produtoRepository = new ProdutoRepository(connectionString);
            ExibirMenu();
        }
        catch (Exception ex) { Console.WriteLine($"Erro ao iniciar a aplicação: {ex.Message}"); }
    }

    private static void ExibirMenu()
    {
        string? opcao;
        do
        {
            Console.Clear();
            Console.WriteLine("===== GERENCIAMENTO DE PRODUTOS =====\n\n1 - Inserir produto\n2 - Listar produtos\n3 - Buscar produto por ID\n4 - Atualizar produto\n5 - Excluir produto\n6 - Sair\n");
            Console.Write("Escolha uma opção: ");
            opcao = Console.ReadLine();
            Console.WriteLine();
            switch (opcao)
            {
                case "1": InserirProduto(); break;
                case "2": ListarProdutos(); break;
                case "3": BuscarProduto(); break;
                case "4": AtualizarProduto(); break;
                case "5": ExcluirProduto(); break;
                case "6": Console.WriteLine("Aplicação encerrada."); break;
                default: Console.WriteLine("Opção inválida."); break;
            }
            if (opcao != "6") Pausar();
        } while (opcao != "6");
    }

    private static void InserirProduto()
    {
        Produto? produto = LerDadosProduto();
        if (produto is null) return;
        try { _produtoRepository.Inserir(produto); Logger.Registrar($"Produto inserido - Nome: {produto.Nome}"); Console.WriteLine("Produto inserido com sucesso."); }
        catch (Exception ex) { Console.WriteLine($"Não foi possível inserir o produto: {ex.Message}"); }
    }

    private static void ListarProdutos()
    {
        try
        {
            List<Produto> produtos = _produtoRepository.Listar();
            if (produtos.Count == 0) { Console.WriteLine("Nenhum produto cadastrado."); return; }
            Console.WriteLine("ID | Nome | Preço | Estoque | Categoria\n" + new string('-', 65));
            foreach (Produto produto in produtos) Console.WriteLine($"{produto.Id} | {produto.Nome} | R$ {produto.Preco:N2} | {produto.Estoque} | {produto.Categoria}");
        }
        catch (Exception ex) { Console.WriteLine($"Não foi possível listar os produtos: {ex.Message}"); }
    }

    private static void BuscarProduto()
    {
        int? id = LerId();
        if (!id.HasValue) return;
        try
        {
            Produto? produto = _produtoRepository.BuscarPorId(id.Value);
            Logger.Registrar($"Produto consultado - ID: {id.Value}");
            if (produto is null) Console.WriteLine("Produto não encontrado."); else ExibirProduto(produto);
        }
        catch (Exception ex) { Console.WriteLine($"Não foi possível buscar o produto: {ex.Message}"); }
    }

    private static void AtualizarProduto()
    {
        int? id = LerId();
        if (!id.HasValue) return;
        try
        {
            if (_produtoRepository.BuscarPorId(id.Value) is null) { Console.WriteLine("Produto não encontrado."); return; }
            Console.WriteLine("Informe os novos dados do produto:");
            Produto? produto = LerDadosProduto();
            if (produto is null) return;
            produto.Id = id.Value;
            if (_produtoRepository.Atualizar(produto)) { Logger.Registrar($"Produto atualizado - ID: {produto.Id} - Nome: {produto.Nome}"); Console.WriteLine("Produto atualizado com sucesso."); }
        }
        catch (Exception ex) { Console.WriteLine($"Não foi possível atualizar o produto: {ex.Message}"); }
    }

    private static void ExcluirProduto()
    {
        int? id = LerId();
        if (!id.HasValue) return;
        try
        {
            if (_produtoRepository.Excluir(id.Value)) { Logger.Registrar($"Produto excluído - ID: {id.Value}"); Console.WriteLine("Produto excluído com sucesso."); }
            else Console.WriteLine("Produto não encontrado.");
        }
        catch (Exception ex) { Console.WriteLine($"Não foi possível excluir o produto: {ex.Message}"); }
    }

    private static Produto? LerDadosProduto()
    {
        Console.Write("Nome: "); string? nome = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(nome)) { Console.WriteLine("O nome não pode ser vazio."); return null; }
        Console.Write("Preço: ");
        if (!TentarLerPreco(Console.ReadLine(), out decimal preco) || preco < 0) { Console.WriteLine("Preço inválido."); return null; }
        Console.Write("Estoque: ");
        if (!int.TryParse(Console.ReadLine(), out int estoque) || estoque < 0) { Console.WriteLine("Estoque inválido."); return null; }
        Console.Write("Categoria: "); string? categoria = Console.ReadLine();
        if (string.IsNullOrWhiteSpace(categoria)) { Console.WriteLine("A categoria não pode ser vazia."); return null; }
        return new Produto { Nome = nome.Trim(), Preco = preco, Estoque = estoque, Categoria = categoria.Trim() };
    }

    private static int? LerId()
    {
        Console.Write("ID: ");
        if (int.TryParse(Console.ReadLine(), out int id) && id > 0) return id;
        Console.WriteLine("ID inválido."); return null;
    }

    private static bool TentarLerPreco(string? texto, out decimal preco)
    {
        if (string.IsNullOrWhiteSpace(texto)) { preco = 0; return false; }
        string valorNormalizado = texto.Trim().Replace(',', '.');
        return decimal.TryParse(valorNormalizado, NumberStyles.Number, CultureInfo.InvariantCulture, out preco);
    }
    private static void ExibirProduto(Produto produto) => Console.WriteLine($"ID: {produto.Id}\nNome: {produto.Nome}\nPreço: R$ {produto.Preco:N2}\nEstoque: {produto.Estoque}\nCategoria: {produto.Categoria}");

    private static void Pausar()
    {
        Console.WriteLine("\nPressione qualquer tecla para continuar...");
        if (Console.IsInputRedirected) Console.ReadLine();
        else Console.ReadKey();
    }
}
