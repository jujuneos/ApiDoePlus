using ApiDoePlus.Context;
using ApiDoePlus.Models.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ApiDoePlus.Controllers;

[Route("[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly ApiDoePlusDbContext _context;
    private readonly UserManager<ApplicationUser> _user;

    public UsuariosController(ApiDoePlusDbContext context, UserManager<ApplicationUser> user)
    {
        _context = context;
        _user = user;
    }

    [HttpGet("favoritas")]
    [Authorize]
    public ActionResult<IEnumerable<ApplicationUser>> GetInstituicoesFavoritas()
    {
        var usuario = _context.Users.Include(u => u.InstituicoesFavoritas).FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));

        List<ApplicationUser> favoritas = usuario.InstituicoesFavoritas.ToList();

        if (favoritas == null)
            return NotFound("Usuário não possui instituições favoritas.");

        return favoritas;
    }

    [HttpPost("favoritar/{id}")]
    [Authorize]
    public async Task<ActionResult> Favoritar(string id)
    {
        var instituicao = _context.Users.FirstOrDefault(i => i.Id.Equals(id));

        if (instituicao == null)
            return BadRequest("Instituição não localizada.");

        var usuario = _context.Users.FirstOrDefault(u => u.UserName.Equals(User.Identity.Name));

        if (usuario == null)
            return BadRequest("Usuário não encontrado.");

        if (usuario.InstituicoesFavoritas == null)
            usuario.InstituicoesFavoritas = new List<ApplicationUser>();

        usuario.InstituicoesFavoritas.Add(instituicao);

        var result = await _user.UpdateAsync(usuario);

        if (result.Succeeded)
            return Ok();
        else
            return BadRequest();
    }
}
