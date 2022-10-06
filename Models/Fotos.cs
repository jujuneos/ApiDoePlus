using ApiDoePlus.Models.Autenticacao;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDoePlus.Models;

public class Fotos
{
    public int Id { get; set; }
    public byte[]? Bytes { get; set; }
    public string? Descricao { get; set; }
    public string? FileExtension { get; set; }
    public decimal SizePhoto { get; set; }
    public string? InstituicaoId { get; set; }
}