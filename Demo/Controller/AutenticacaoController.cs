using Demo.Domain.User;
using Demo.Models;
using Demo.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;

namespace Demo.Controller
{
    [Authorize(Roles = "Admin")]
    [ApiController]
    [Route("api/[controller]")]
    public class AutenticacaoController : ControllerBase
    {
        private readonly UserManager<UserAplication> _userManager;
        private readonly SignInManager<UserAplication> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AutenticacaoController(SignInManager<UserAplication> signInManager,
        UserManager<UserAplication> userManager,
        RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        [AllowAnonymous]
        [HttpPost("cadastrar")]
        public async Task<IActionResult> Cadrastrar([FromBody] UserRequest user)
        {
            if (string.IsNullOrWhiteSpace(user.Email) || string.IsNullOrWhiteSpace(user.Password)
                || string.IsNullOrWhiteSpace(user.Rg))
                return Ok("Falta alguns dados");

            var userAplication = new UserAplication
            {
                UserName = user.Nome,
                Email = user.Email,
                RG = user.Rg
            };

            //Criando Usuario
            var resultado = await _userManager.CreateAsync(userAplication, user.Password);

            if (resultado.Errors.Any()) return Conflict(resultado.Errors);

            //Adicionando Roles
            await _userManager.AddToRoleAsync(userAplication, "Padrao");

            //Pegando Usuario
            var userId = await _userManager.GetUserIdAsync(userAplication);

            //Gerando confirmação E-mail
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(userAplication);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            //Confirmando email
            var resultado2 = await _userManager.ConfirmEmailAsync(userAplication, code);

            if (resultado2.Succeeded)
                return Ok("Usuário Adicionado com Sucesso");
            else
                return Ok("Erro ao confirmar usuários");
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Logar(InputLoginRegister input)
        {
            if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Senha))
                return Unauthorized();

            var result = await _signInManager.PasswordSignInAsync(input.Email, input.Senha,
                false, lockoutOnFailure: false);

            if (!result.Succeeded) return NotFound();

            var user = await _userManager.FindByEmailAsync(input.Email);
            var roles = await _userManager.GetRolesAsync(user!);

            var claims = new List<Claim>
            {
                new Claim("name", input.Email) ,
                new Claim("email", input.Email),
            };

            foreach (string role in roles) 
            { 
                claims.Add(new Claim("roles", role)); 
            };

            var token = new TokenJWTBuilder()
                .AddSecurityKey(JwtSecurityKey.Create("43443FDFDF34DF34343fdf344SDFSDFSDFSDFSDF4545354345SDFGDFGDFGDFGdffgfdGDFGDGR%"))
                .AddClaims(claims)
                .AddSubject("Canal Dev Net Core")
                .AddIssuer("Teste.Securiry.Bearer")
                .AddAudience("Teste.Securiry.Bearer")
                .AddExpiry(5)
                .Builder();
            
            return Ok(token.value);
        }

        [HttpPost("criar-role")]
        public async Task<IActionResult> CriarRole(string roleName)
        {
            var role = new IdentityRole(roleName);

            if (await _roleManager.RoleExistsAsync(roleName)) return Ok("Role já existente");

            var result = await _roleManager.CreateAsync(role);

            if (result.Succeeded) {
                return Ok("Role criada com sucesso");
            }
            else 
            {
                return Ok("Erro ao criar a role:"+result.Errors);
            }

        }

        [HttpGet("logout")]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Logar");
        }

        [HttpGet("users")]
        public async Task<IActionResult> ListarUsuarios() 
        {
            var users = await _userManager.Users.ToListAsync();
            return Ok(users);
        }

    }
}
