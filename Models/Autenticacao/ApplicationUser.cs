using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ApiDoePlus.Models.Autenticacao;

public class ApplicationUser : IdentityUser
{
    public string? Tipo { get; set; }
    [StringLength(500)]
    public string? Descricao { get; set; }
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

    public string? Site { get; set; }

    public double Avaliacao { get; set; }

    public double AvaliacaoTotal { get; set; }

    public int QtdAvaliacoes { get; set; }

    [JsonIgnore]
    public virtual ICollection<ApplicationUser>? InstituicoesFavoritas { get; set; }
}