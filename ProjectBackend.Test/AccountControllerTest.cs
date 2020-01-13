using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectBackendTest;
using ProjectBackendTest.Controllers;
using ProjectBackendTest.Model;
using ProjectBackendTest.Models.Request;
using ProjectBackendTest.Services.Contracts;
using ProjectBackendTest.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectBackend.Test
{
    [TestClass]
    public class AccountControllerTest
    {
        public Mock<IConfiguration> _configuration;
        public readonly IUsuarioService MockUsuarioService;
        Mock<IUsuarioService> mockUsuarioService = new Mock<IUsuarioService>();
        Mock<IUsuarioService> _mockUsuarioService = new Mock<IUsuarioService>();

        public AccountControllerTest()
        {
            List<Usuario> usuarios = new List<Usuario>
            {
                new Usuario
                {
                    Id = 1,
                    Nome = "Joao",
                    DecryptedPassword = "123456",
                    Email = "joao@empresa.com.br",
                    IsAtivo = true,
                },
                new Usuario
                {
                    Id = 2,
                    Nome = "Maria",
                    DecryptedPassword = "123456",
                    Email = "maria@empresa.com.br",
                    IsAtivo = true
                },
                new Usuario
                {
                    Id = 3,
                    Nome = "Antonio",
                    DecryptedPassword = "123456",
                    Email = "antonio@empresa.com.br",
                    IsAtivo = true
                }
            };


            _mockUsuarioService.Setup(mr => mr.GetPorLogin(
                It.IsAny<string>())).Returns((string i) =>  usuarios.Where(
                x => x.Nome == i).FirstOrDefault());
            _configuration = new Mock<IConfiguration>();
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "Jwt:Issuer")]).Returns("http://localhost:44363/");
            _configuration.SetupGet(x => x[It.Is<string>(s => s == "Jwt:Key")]).Returns("12345678910111213141516");




            this.MockUsuarioService = _mockUsuarioService.Object;
        }
        [TestMethod]
        public async Task TestLogin__Senha_Invalida_Unauthorized()
        {

            Mock<IUsuarioService> mockPessoaService = new Mock<IUsuarioService>();
            var login = new LoginModel();
            login.Login = "Maria";
            login.Password = "testes12122";
            var controller = new AccountController(_configuration.Object, mockPessoaService.Object);
            var result = await controller.Login(login) as UnauthorizedResult;
            Assert.AreEqual(401, result.StatusCode);
        }
        [TestMethod]
        public async Task TestLogin_OK()
        {

            Mock<IUsuarioService> mockPessoaService = new Mock<IUsuarioService>();
            var login = new LoginModel();
            login.Login = "Maria";
            login.Password = "123456";
            var controller = new AccountController(_configuration.Object, this._mockUsuarioService.Object);
            var result = await controller.Login(login) as OkObjectResult;
            Assert.AreEqual(200, result.StatusCode);
        }

        [TestMethod]
        public async Task TestLogin_Invalido_OK()
        {

            Mock<IUsuarioService> mockPessoaService = new Mock<IUsuarioService>();
            var login = new LoginModel();
            login.Login = "Jose";
            login.Password = "123456";
            var controller = new AccountController(_configuration.Object, this._mockUsuarioService.Object);
            var result = await controller.Login(login) as UnauthorizedResult;
            Assert.AreEqual(401, result.StatusCode);
        }

        //[TestMethod]
        //public async Task Test_Troca_Senha_OK()
        //{

        //    Mock<IUsuarioService> mockPessoaService = new Mock<IUsuarioService>();
        //    var model = new ChangePassword();
        //    model.Login = "Jose";
        //    model.Password = "testesenha";
        //    model.PasswordConfirm = "testesenha";
        //    var controller = new AccountController(_configuration.Object, this._mockUsuarioService.Object);
        //    var result = await controller.ChangePassword(model) as BadRequestResult;
        //    Assert.AreEqual(401, result.StatusCode);
        //}
    }
}
