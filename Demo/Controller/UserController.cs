using Demo.Domain.User;
using Demo.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using System.Text;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<UserAplication> _userManager;
        private readonly SignInManager<UserAplication> _signInManager;
        public UsersController(UserManager<UserAplication> userManager,
            SignInManager<UserAplication> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }



        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/adiciona-usuario")]
        public async Task<IActionResult> AdicionaUsuario([FromBody] AddUserRequest login)
        {
            if (string.IsNullOrWhiteSpace(login.email) || string.IsNullOrWhiteSpace(login.senha)
                || string.IsNullOrWhiteSpace(login.rg))
                return Ok("Falta alguns dados");


            var user = new UserAplication
            {
                UserName = login.email,
                Email = login.email,
                RG = login.rg
            };

            var resultado = await _userManager.CreateAsync(user, login.senha);

            if (resultado.Errors.Any()) return Ok(resultado.Errors);

            var userId = await _userManager.GetUserIdAsync(user);
            var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

            code = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(code));
            code = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(code));

            var resultado2 = await _userManager.ConfirmEmailAsync(user, code);

            if (resultado2.Succeeded)
                return Ok("Usuário Adicionado com Sucesso");
            else
                return Ok("Erro ao confirmar usuários");
        }
    }
}