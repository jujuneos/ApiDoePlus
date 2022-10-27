using ApiDoePlus.Context;
using ApiDoePlus.Models.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;

namespace ApiDoePlus.Controllers;

[Route("api/[controller]")]
[ApiController]
public class InstituicoesController : ControllerBase
{
    private readonly ApiDoePlusDbContext _context;
    

    public InstituicoesController(ApiDoePlusDbContext context)
    {
        _context = context;
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

    [HttpPut]
    [Authorize]
    public IActionResult Put([FromBody] ApplicationUser instituicao)
    {
        if (instituicao == null)
            return BadRequest();

        _context.Entry(instituicao).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(instituicao);
    }

    [HttpDelete("Deletar/{id}")]
    [Authorize]
    public ActionResult Delete(string id)
    {
        var instituicao = _context.Users.FirstOrDefault(i => i.Id.Equals(id));

        if (instituicao == null)
            return NotFound("Instituição não localizada.");

        _context.Users.Remove(instituicao);
        _context.SaveChanges();

        return Ok(instituicao);
    }

    [HttpPost("Avaliar/{id}")]
    [Authorize]
    public ActionResult Avaliar(string id, double avaliacao)
    {
        var instituicao = _context.Users.FirstOrDefault(i => i.Id.Equals(id));

        if (instituicao == null)
            return BadRequest("Instituição não localizada.");

        instituicao.QtdAvaliacoes++;
        instituicao.AvaliacaoTotal += avaliacao;
        instituicao.Avaliacao = instituicao.AvaliacaoTotal / instituicao.QtdAvaliacoes;

        _context.SaveChanges();

        return Ok();
    }
}