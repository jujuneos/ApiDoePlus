using ApiDoePlus.Models.Autenticacao;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDoePlus.Models;

public class Foto
{
    public int Id { get; set; }
    public byte[] Bytes { get; set; }
    public string Descricao { get; set; }
    public string FileExtension { get; set; }
    public decimal Size { get; set; }
    public string InstituicaoId { get; set; }

    [ForeignKey("InstituicaoId")]
    public ApplicationUser Instituicao { get; set; }
}