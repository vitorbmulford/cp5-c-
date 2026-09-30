namespace GerenciamentoProdutos.Services;

public static class Logger
{
    private static readonly string CaminhoLog = Path.Combine(AppContext.BaseDirectory, "logs", "operacoes.txt");
    public static void Registrar(string mensagem)
    {
        try
        {
            string? diretorio = Path.GetDirectoryName(CaminhoLog);
            if (diretorio is not null) Directory.CreateDirectory(diretorio);
            File.AppendAllText(CaminhoLog, $"[{DateTime.Now:dd/MM/yyyy HH:mm:ss}] {mensagem}{Environment.NewLine}");
        }
        catch { /* Log não deve interromper a operação principal. */ }
    }
}
