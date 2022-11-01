using System.ComponentModel.DataAnnotations;

namespace ApiDoePlus.ViewModels;
public class LoginViewModel
{
    [Required(ErrorMessage = "E-mail é obrigatório!")]
    public string? UserName { get; set; }

    [Required(ErrorMessage = "Senha é obrigatória!")]
    public string? Senha { get; set; }
}
