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
    public ActionResult<dynamic> GetInstituicoesFavoritas()
    {
        var usuario = _context
            .Users
            .Where(u => u.UserName.Equals(User.Identity.Name))
            .Include(u => u.InstituicoesFavoritas)
            .SingleOrDefault();

        var instituicoes = usuario
            .InstituicoesFavoritas
            .Select(i => new
            {
                id = i.Id,
                nome = i.UserName
            })
            .ToList();

        if (instituicoes == null)
            return NotFound("Usuário não possui instituições favoritas.");

        return instituicoes;
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

        if (usuario.InstituicoesFavoritas.Contains(instituicao))
            return BadRequest("Instituição já está favoritada.");

        usuario.InstituicoesFavoritas.Add(instituicao);

        var result = await _user.UpdateAsync(usuario);

        if (result.Succeeded)
            return Ok();
        else
            return BadRequest();
    }
}
