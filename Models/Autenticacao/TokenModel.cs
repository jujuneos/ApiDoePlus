namespace ApiDoePlus.Models.Autenticacao;

public class TokenModel
{
    public string? Token { get; set; }

    public DateTime Expiration { get; set; }

    public bool? Authenticated { get; set; }

    public String? Message { get; set; }
}