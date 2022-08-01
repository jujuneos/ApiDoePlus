using ApiDoePlus.Context;
using ApiDoePlus.Models.Instituicoes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

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
    public ActionResult<IEnumerable<Instituicao>> Get()
    {
        var instituicoes = _context.Instituicoes.ToList();
        if (!instituicoes.Any())
            return NotFound("Nenhuma instituição cadastrada.");
        return instituicoes;
    }

    [HttpGet("{id:int}")]
    public ActionResult<Instituicao> Get(int id)
    {
        var instituicao = _context.Instituicoes.FirstOrDefault(x => x.InstituicaoId == id);
        if (instituicao == null)
            return NotFound();
        return instituicao;
    }

    [HttpPost]
    public ActionResult Post(Instituicao instituicao)
    {
        if (instituicao == null)
            return BadRequest();

        _context.Instituicoes.Add(instituicao);
        _context.SaveChanges();

        return new CreatedAtRouteResult("Obter instituição", new { instituicao.InstituicaoId }, instituicao);
    }

    [HttpPut("{id:int}")]
    public ActionResult Put(int id, Instituicao instituicao)
    {
        if (id != instituicao.InstituicaoId)
            return BadRequest();

        _context.Entry(instituicao).State = EntityState.Modified;
        _context.SaveChanges();

        return Ok(instituicao);
    }

}