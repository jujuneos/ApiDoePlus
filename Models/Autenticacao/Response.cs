namespace ApiDoePlus.Models.Autenticacao;

public class Response
{
    public bool Success { get; set; } = true;

    public string? Mensagem { get; set; }

    public object? Data { get; set; }
}