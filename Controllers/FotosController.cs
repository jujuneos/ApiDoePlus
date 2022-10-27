using ApiDoePlus.Context;
using ApiDoePlus.Models.Autenticacao;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ApiDoePlus.Controllers;

[Route("api/[controller]")]
[ApiController]
public class FotosController : Controller
{
    private readonly FotosContext _fotosContext;

    public FotosController(FotosContext context)
    {
        _fotosContext = context;
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
}
