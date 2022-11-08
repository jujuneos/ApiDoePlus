using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiDoePlus.ViewModels;
using ApiDoePlus.Models.Autenticacao;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiDoePlus.Models;
using ApiDoePlus.Context;
using Microsoft.EntityFrameworkCore;

namespace ApiDoePlus.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AutenticacaoController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly FotosContext _dbContext;

    public AutenticacaoController(
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        FotosContext dbContext
        )
    {
        _config = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
        _dbContext = dbContext;
    }

    [HttpPost("cadastrarUsuario")]
    public async Task<ActionResult<TokenModel>> RegistrarUsuario([FromBody] CadastroUsuarioViewModel model)
    {
        var user = new ApplicationUser { 
            UserName = model.Nome, 
            Email = model.Email
        };

        var result = await _userManager.CreateAsync(user, model.Senha);

        if (result.Succeeded)
            return GetTokenCadastroUsuario(model);
        else
            return BadRequest("Usuário ou senha inválidos.");
    }

    [HttpPost("cadastrarInstituicao")]
    public async Task<ActionResult<TokenModel>> RegistrarInstituicao([FromForm] CadastroInstituicaoViewModel model)
    {
        var ong = new ApplicationUser
        {
            UserName = model.Nome,
            Email = model.UserName,
            Tipo = model.Tipo,
            Descricao = model.Descricao,
            PhoneNumber = model.Telefone,
            Endereco = model.Endereco,
            ChavePix = model.ChavePix,
            Banco = model.Banco,
            Agencia = model.Agencia,
            Conta = model.Conta,
            PicPay = model.PicPay,
            Latitude = model.Latitude,
            Longitude = model.Longitude,
            Avaliacao = model.Avaliacao,
            AvaliacaoTotal = model.AvaliacaoTotal,
            QtdAvaliacoes = model.QtdAvaliacoes,
            Site = model.Site
        };

        var result = await _userManager.CreateAsync(ong, model.Senha);

        if (model.Files.Count > 0)
        {
            foreach (var file in model.Files)
            {
                if (file.Length > 0)
                {
                    var memoryStream = new MemoryStream();
                    file.CopyTo(memoryStream);

                    var foto = new Fotos()
                    {
                        Bytes = memoryStream.ToArray(),
                        Descricao = file.FileName,
                        FileExtension = Path.GetExtension(file.FileName),
                        SizePhoto = file.Length,
                        InstituicaoId = ong.Id
                    };

                    _dbContext.Add(foto);
                }
            }
        }

        _dbContext.SaveChanges();

        if (result.Succeeded)
            return GetTokenCadastroInstituicao(model);
        else
            return BadRequest("Usuário ou senha inválidos.");
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<TokenModel>> Login([FromBody] LoginViewModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Senha, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
            return GetTokenLogin(model);
        else
        {
            ModelState.AddModelError(string.Empty, "Login inválido.");
            return BadRequest(ModelState);
        }
    }

    [HttpPost("logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return Ok();
    }

    private TokenModel GetTokenLogin(LoginViewModel model)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, model.UserName),
            new Claim("Valor", "Valor2"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiracao = DateTime.Now.AddYears(1);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiracao,
            signingCredentials: creds
        );

        return new TokenModel()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiracao,
            Authenticated = true,
            Message = "Ok"
        };
    }

    private TokenModel GetTokenCadastroUsuario(CadastroUsuarioViewModel model)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, model.Email),
            new Claim("Valor", "Valor2"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiracao = DateTime.Now.AddYears(1);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiracao,
            signingCredentials: creds
        );

        return new TokenModel()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiracao
        };
    }

    private TokenModel GetTokenCadastroInstituicao(CadastroInstituicaoViewModel model)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, model.UserName),
            new Claim("Valor", "Valor2"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiracao = DateTime.Now.AddYears(1);

        JwtSecurityToken token = new JwtSecurityToken(
            issuer: null,
            audience: null,
            claims: claims,
            expires: expiracao,
            signingCredentials: creds
        );

        return new TokenModel()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiracao
        };
    }
}
