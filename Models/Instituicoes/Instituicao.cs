using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDoePlus.Models.Instituicoes;

[Table("Instituicao")]
public class Instituicao
{
    [Key]
    public int InstituicaoId { get; set; }
    [Required]
    [StringLength(300)]
    public string? Nome { get; set; }
    [Required]
    [StringLength(100)]
    public string? Email { get; set; }
    [Required]
    [StringLength(16)]
    public string? Senha { get; set; }
    [Required]
    public string? Tipo { get; set; }
    [StringLength(500)]
    public string? Descricao { get; set; }
    public string? Foto { get; set; }
    [StringLength(15)]
    public string? Telefone { get; set; }
    [Required]
    [StringLength(500)]
    public string? Endereco { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    [StringLength(100)]
    public string? ChavePix { get; set; }
    [StringLength(50)]
    public string? Banco { get; set; }
    [StringLength(5)]
    public string? Agencia { get; set; }
    [StringLength(10)]
    public string? Conta { get; set; }
    [StringLength(100)]
    public string? PicPay { get; set; }

    public float? Avaliacao { get; set; }
}
