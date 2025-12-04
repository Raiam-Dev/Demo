using Demo.Domain.User;
using Demo.Models;
using Demo.Token;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Demo.Controller
{
    public class TokenController : ControllerBase
    {
        private readonly UserManager<UserAplication> _userManager;
        private readonly SignInManager<UserAplication> _signInManager;

        public TokenController(SignInManager<UserAplication> signInManager,
        UserManager<UserAplication> userManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [AllowAnonymous]
        [Produces("application/json")]
        [HttpPost("/api/CreateToken")]
        public async Task<IActionResult> CreateToken(InputLoginRegister input)
        {
            if (string.IsNullOrWhiteSpace(input.Email) || string.IsNullOrWhiteSpace(input.Senha))
                return Unauthorized();

            var result = await _signInManager.PasswordSignInAsync(input.Email, input.Senha,
                false, lockoutOnFailure: false);

            if (!result.Succeeded) return Unauthorized();

            var claims = new Dictionary<string, string>
            {
                { ClaimTypes.Name, input.Email },
                { ClaimTypes.Email, input.Email},
                { ClaimTypes.Role, "Admin" }
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
    }
}
