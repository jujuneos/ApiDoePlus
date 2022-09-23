using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using ApiDoePlus.ViewModels;
using ApiDoePlus.Models.Autenticacao;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiDoePlus.Models;

namespace ApiDoePlus.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AutenticacaoController : ControllerBase
{
    private readonly IConfiguration _config;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AutenticacaoController(
        IConfiguration configuration,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager
        )
    {
        _config = configuration;
        _userManager = userManager;
        _signInManager = signInManager;
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
    public async Task<ActionResult<TokenModel>> RegistrarInstituicao([FromBody] CadastroInstituicaoViewModel model)
    {
        var ong = new ApplicationUser
        {
            UserName = model.Nome,
            Email = model.Email,
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
            Longitude = model.Longitude
        };

        List<Foto> photoList = new List<Foto>();

        if (model.Files.Count > 0)
        {
            foreach (var file in model.Files)
            {
                if (file.Length > 0)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await file.CopyToAsync(memoryStream);
                        if (memoryStream.Length < 2097152)
                        {
                            var foto = new Foto()
                            {
                                Bytes = memoryStream.ToArray(),
                                Descricao = file.FileName,
                                FileExtension = Path.GetExtension(file.FileName),
                                Size = file.Length
                            };

                            photoList.Add(foto);
                        }
                        else
                        {
                            ModelState.AddModelError("File", "O arquivo é muito grande.");
                        }
                    }
                }
            }
        }

        model.Fotos = photoList;

        var result = await _userManager.CreateAsync(ong, model.Senha);

        if (result.Succeeded)
            return GetTokenCadastroInstituicao(model);
        else
            return BadRequest("Usuário ou senha inválidos.");
    }

    [HttpPost]
    [Route("login")]
    public async Task<ActionResult<TokenModel>> Login([FromBody] LoginViewModel model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.Email, model.Senha, isPersistent: false, lockoutOnFailure: false);

        if (result.Succeeded)
            return GetTokenLogin(model);
        else
        {
            ModelState.AddModelError(string.Empty, "Login inválido.");
            return BadRequest(ModelState);
        }
    }

    private TokenModel GetTokenLogin(LoginViewModel model)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, model.Email),
            new Claim("Valor", "Valor2"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiracao = DateTime.Now.AddHours(1);

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
            ValidoAte = expiracao
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

        var expiracao = DateTime.Now.AddHours(1);

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
            ValidoAte = expiracao
        };
    }

    private TokenModel GetTokenCadastroInstituicao(CadastroInstituicaoViewModel model)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.UniqueName, model.Email),
            new Claim("Valor", "Valor2"),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["JWT:Secret"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var expiracao = DateTime.Now.AddHours(1);

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
            ValidoAte = expiracao
        };
    }
}
