using ApiDoePlus.Models;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDoePlus.ViewModels;

public class CadastroInstituicaoViewModel
{
    [Required(ErrorMessage = "Nome é obrigatório.")]
    public string? Nome { get; set; }

    [Required(ErrorMessage = "Nome de usuário é obrigatório.")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória.")]
    public string? Senha { get; set; }
    public string? Tipo { get; set; }
    [StringLength(500)]
    public string? Descricao { get; set; }
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

    public string? Site { get; set; }

    public double Avaliacao { get; set; }
    public double AvaliacaoTotal { get; set; }
    public int QtdAvaliacoes { get; set; }

    [FromForm(Name = "fotos")]
    public List<IFormFile> Files { get; set; }
}

