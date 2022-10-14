using ApiDoePlus.Context;
using ApiDoePlus.Models.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiDoePlus.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstituicoesController : ControllerBase
{
    private readonly ApiDoePlusDbContext _context;
    private readonly FotosContext _fotosContext;

    public InstituicoesController(ApiDoePlusDbContext context, FotosContext fotosContext)
    {
        _context = context;
        _fotosContext = fotosContext;
    }

    [HttpGet]
    public ActionResult<IEnumerable<ApplicationUser>> GetInstituicoes()
    {
        var instituicoes = _context.Users.Where(u => u.Tipo != null).ToList();
        if (!instituicoes.Any())
            return NotFound("Nenhuma instituição cadastrada.");
        return instituicoes;
    }

    [HttpGet("{tipo}")]
    public ActionResult<IEnumerable<ApplicationUser>> GetPorTipo(string tipo)
    {
        var instituicoes = _context.Users.Where(x => x.Tipo != null && x.Tipo.Equals(tipo)).ToList();

        if (!instituicoes.Any())
            return NotFound("Nenhuma instituição desse tipo encontrada.");
        return instituicoes;
    }

    [HttpGet("Foto/{id}")]
    public IActionResult GetFoto(string id)
    {
        var foto = _fotosContext.fotos.Where(x => x.InstituicaoId == id).FirstOrDefault();

        if (foto == null)
            return NotFound("Nenhuma foto cadastrada para esta instituição.");
        return Ok(foto);
    }

    [HttpGet("Fotos/{id}")]
    public IActionResult GetFotos(string id)
    {
        var fotos = _fotosContext.fotos.Where(x => x.InstituicaoId == id).ToList();

        if (!fotos.Any())
            return NotFound("Nenhuma foto cadastrada para esta instituição.");
        return Ok(fotos);
    }

    [HttpPut("{id}")]
    [Authorize]
    public ActionResult Put(int id, ApplicationUser instituicao)
    {
        if (!id.Equals(instituicao.Id))
            return BadRequest();

        _context.Entry(instituicao).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(instituicao);
    }

    [HttpDelete("{id:int}")]
    [Authorize]
    public ActionResult Delete(int id)
    {
        var instituicao = _context.Users.FirstOrDefault(i => i.Id.Equals(id));

        if (instituicao == null)
            return NotFound("Instituição não localizada.");

        _context.Users.Remove(instituicao);
        _context.SaveChanges();

        return Ok(instituicao);
    }
}