using System.Drawing.Printing;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.DotNet.Scaffolding.Shared.Project;
using Microsoft.IdentityModel.Tokens;
using Projeto02.Context;
using Projeto02.Models;


namespace Projeto02.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsuarioController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IConfiguration _configuration;
    private readonly AppDbContext _context;
    public UsuarioController(UserManager<ApplicationUser> userManager,
    SignInManager<ApplicationUser> signInManager,
    IConfiguration configuration, AppDbContext context)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _configuration = configuration;
        _context = context;
    }
    [HttpGet]
    public ActionResult<string> Get()
    {
        return " << Controlador UsuariosController :: WebApiUsuarios >>";
    }
    [HttpPost("Criar")]
    public async Task<ActionResult<UserToken>> CreateUser([FromBody] UserInfo model)
    {
        var pacienteCad = _context.Colaboradors.FirstOrDefault(e => e.Email
        == model.Email && e.Cpf == model.Cpf);
        if (pacienteCad != null)
        {
            var user = new ApplicationUser
            {
                UserName = model.Email,
                Email = model.Email,
                Cpf = model.Cpf
            };
            var result = await _userManager.CreateAsync(user,
            model.Password);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Basic");
                var roles = await _userManager.GetRolesAsync(user);
                return BuildToken(model, roles);
            }
            else
            {
                return BadRequest("Usuário ou senha inválidos");
            }
        }
        else
        { return BadRequest("Colaborador não cadastrado"); }
    }
    [HttpGet("Check")]
    public async Task<ActionResult<String>> CheckUser(int Cpf, string Email)
    {
        var pacienteCad = _context.Colaboradors.FirstOrDefault(e => e.Email
        == Email && e.Cpf == Cpf);
        var userExiste = _context.Users.FirstOrDefault(u => u.Email ==
        Email && u.Cpf == Cpf);
        if (userExiste != null)
        { return BadRequest("Usuario já cadastrado"); }
        else
        {
            if (pacienteCad != null)
            {
                return "OK";
            }
            else
            { return BadRequest("Colaborador não cadastrado"); }
        }
    }
    
    [HttpPost("Login")]
    public async Task<ActionResult<UserToken>> Login([FromBody] UserInfo userInfo)
    {
        var result = await
        _signInManager.PasswordSignInAsync(userInfo.Email, userInfo.Password, isPersistent: false, lockoutOnFailure: false);
        if (result.Succeeded)
        {
            var user = await
        _userManager.FindByNameAsync(userInfo.Email);
            var roles = await _userManager.GetRolesAsync(user);
            return BuildToken(userInfo, roles);
        }
        else
        {
            ModelState.AddModelError(string.Empty, "login inválido.");
            return BadRequest(ModelState);
        }
    }
    private UserToken BuildToken(UserInfo userInfo, IList<string>
    userRoles)
    {
        int idCol = 0;
        var colabolador = _context.Colaboradors.FirstOrDefault(c=>c.Cpf==userInfo.Cpf);
        if (colabolador != null){
            idCol = colabolador.CodigoColab;
        }
        var claims = new List<Claim>
{
new Claim(JwtRegisteredClaimNames.UniqueName, userInfo.Email),
new Claim("meuValor", "oque voce quiser"),
new Claim(JwtRegisteredClaimNames.Jti,
Guid.NewGuid().ToString())
};
        foreach (var userRole in userRoles)
        {
            claims.Add(new Claim(ClaimTypes.Role, userRole));
        }
        var key = new
        SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:key"]));
        var creds = new SigningCredentials(key,
        SecurityAlgorithms.HmacSha256);
        // tempo de expiração do token: 1 hora
        var expiration = DateTime.UtcNow.AddHours(1);
        JwtSecurityToken token = new JwtSecurityToken(
        issuer: null,
        audience: null,
        claims: claims,
        expires: expiration,
        signingCredentials: creds);

        return new UserToken()
        {
            Token = new JwtSecurityTokenHandler().WriteToken(token),
            Expiration = expiration,
            Roles = userRoles,
            IdCol = idCol
        };
    }
}