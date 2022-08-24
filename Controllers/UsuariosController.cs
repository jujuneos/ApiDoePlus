using ApiDoePlus.Context;
using ApiDoePlus.Models.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiDoePlus.Controllers;

[Route("[controller]")]
[ApiController]
public class UsuariosController : ControllerBase
{
    private readonly ApiDoePlusDbContext _context;

    public UsuariosController(ApiDoePlusDbContext context)
    {
        _context = context;
    }

    [HttpGet("favoritas")]
    [Authorize]
    public ActionResult<IEnumerable<ApplicationUser>> GetInstituicoesFavoritas(int id)
    {
        var usuario = _context.Users.Include(u => u.InstituicoesFavoritas).FirstOrDefault(u => u.Id.Equals(id));

        if (usuario == null)
            return NotFound("Usuário não encontrado.");

        List<ApplicationUser> favoritas = usuario.InstituicoesFavoritas.ToList();

        if (favoritas == null)
            return NotFound("Usuário não possui instituições favoritas.");

        return favoritas;
    }

    [HttpGet("{id:int}", Name = "ObterUsuario")]
    public ActionResult<ApplicationUser> Get(int id)
    {
        var usuario = _context.Users.FirstOrDefault(u => u.Id.Equals(id));

        if (usuario == null)
            return NotFound();

        return Ok(usuario);
    }

    [HttpPut("{id:int}")]
    [Authorize]
    public ActionResult Put(int id, ApplicationUser usuario)
    {
        if (!id.Equals(usuario.Id))
            return BadRequest();

        _context.Entry(usuario).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(usuario);
    }

    [HttpDelete("{id:int}")]
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
