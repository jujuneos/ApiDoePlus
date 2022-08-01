using ApiDoePlus.Models.Instituicoes;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ApiDoePlus.Models.Usuarios;

[Table("Usuario")]
public class Usuario
{
    public Usuario()
    {
        Favoritas = new Collection<Instituicao>();
    }

    [Key]
    public int UsuarioId { get; set; }
    [Required]
    [StringLength(100)]
    public string? Nome { get; set; }
    [Required]
    [StringLength(100)]
    public string? Email { get; set; }
    [Required]
    [StringLength(16)]
    public string? Senha { get; set; }

    public virtual ICollection<Instituicao>? Favoritas { get; set; }
}
