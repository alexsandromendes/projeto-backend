using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using ProjectBackendTest.Model;
using ProjectBackendTest.Repositories.Contracts;
using ProjectBackendTest.Services.Contracts;
using ProjectBackendTest.ViewModel;

namespace ProjectBackendTest.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        public IConfiguration _config;
        private readonly IUsuarioService _usuarioService;

        public AccountController(IConfiguration config, IUsuarioService usuarioService)
        {
            _config = config;
            _usuarioService = usuarioService;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {

            IActionResult response = Unauthorized();
            var user = Authenticate(model);
            if (user != null)
            {
                string tokenString = BuildToken(user);
                response = Ok(new { token = tokenString });
            }
            return response;
        }
        private string BuildToken(UserModel user)
        {
            var direitos = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Name),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var chave = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credenciais = new SigningCredentials(chave, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _config["Jwt:Issuer"],
                audience: _config["Jwt:Issuer"],
                claims: direitos,
                signingCredentials: credenciais,
                expires: DateTime.Now.AddMinutes(30)
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        private UserModel Authenticate(LoginModel login)
        {
            UserModel user = null;

            var userView = _usuarioService.GetPorLogin(login.Login);
            if (userView != null && userView.DecryptedPassword == login.Password)
            {
                user = new UserModel { Name = userView.Nome,  Email = userView.Email, Birthdate = DateTime.Now, Roles = new string[] { } };
            }

            return user;
        }
        [Authorize]
        [HttpPost("trocarsenha")]
        public  async Task<IActionResult> ChangePassword([FromBody]ChangePassword model)
        {
            try
            {
                Usuario usuario = _usuarioService.GetPorLogin(model.Login);
                if (usuario == null)
                {
                    return NotFound();
                }
                if(model.PasswordConfirm != model.Password)
                {
                    return BadRequest("Senhas devem ser as iguais");
                }
                usuario.DecryptedPassword = model.PasswordConfirm;
                _usuarioService.Atualizar(usuario);
                return Ok(usuario);
            }
            catch (Exception ex)
            {

                return BadRequest(ex);
            }
        }
        private class UserModel
        {
            public string Name { get; set; }
            public string Email { get; set; }
            public DateTime Birthdate { get; set; }
            public string[] Roles { get; set; }
        }
    }
}