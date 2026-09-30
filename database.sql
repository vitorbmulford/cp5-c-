-- SQLite: a aplicação executa este mesmo comando automaticamente na primeira execução.
CREATE TABLE Produtos (
    Id INTEGER PRIMARY KEY AUTOINCREMENT,
    Nome TEXT NOT NULL,
    Preco DECIMAL(10,2) NOT NULL,
    Estoque INTEGER NOT NULL,
    Categoria TEXT NOT NULL
);
