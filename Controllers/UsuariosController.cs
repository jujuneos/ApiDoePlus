using ApiDoePlus.Context;
using ApiDoePlus.Models.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http.Abstractions;

namespace ApiDoePlus.Controllers;

[Route("[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly ApiDoePlusDbContext _context;
    private readonly AuthenticatedUser _user;

    public UsuariosController(ApiDoePlusDbContext context, AuthenticatedUser user)
    {
        _context = context;
        _user = user;
    }

    [HttpGet("favoritas")]
    [Authorize]
    public ActionResult<IEnumerable<ApplicationUser>> GetInstituicoesFavoritass(string id)
    {
        var usuario = _context.Users.Include(u => u.InstituicoesFavoritas).FirstOrDefault(u => u.Id.Equals(id));

        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        List<ApplicationUser> favoritas = usuario.InstituicoesFavoritas.ToList();

        if (favoritas == null)
            return NotFound("Usuário não possui instituições favoritas.");

        return favoritas;
    }

    [HttpGet("favoritar/{id}")]
    [Authorize]
    public ActionResult Favoritar(string id)
    {
        var instituicao = _context.Users.FirstOrDefault(i => i.Id.Equals(id));

        if (instituicao == null)
            return BadRequest("Instituição não localizada.");

        var usuario = _context.Users.FirstOrDefault(u => u.Email.Equals(_user.Email));

        if (usuario == null)
            return BadRequest("Usuário não encontrado.");

        if (usuario.InstituicoesFavoritas != null)
            usuario.InstituicoesFavoritas.Add(instituicao);

        _context.SaveChanges();

        return Ok();
    }

    [HttpGet("{id}", Name = "ObterUsuario")]
    public ActionResult<ApplicationUser> Get(int id)
    {
        var usuario = _context.Users.FirstOrDefault(u => u.Id.Equals(id));

        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    [HttpPut("{id}")]
    [Authorize]
    public ActionResult Put(int id, ApplicationUser usuario)
    {
        if (!id.Equals(usuario.Id))
            return BadRequest();

        _context.Entry(usuario).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(usuario);
    }

    [HttpDelete("{id}")]
    [Authorize]
    public ActionResult Delete(int id)
    {
        var usuario = _context.Users.FirstOrDefault(u => u.Id.Equals(id));

        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        _context.Users.Remove(usuario);
        _context.SaveChanges();

        return Ok(usuario);
    }
}
