using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectBackendTest.Controllers;
using ProjectBackendTest.Models.Request;
using ProjectBackendTest.Services.Contracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ProjectBackend.Test
{
    [TestClass]
    public class UsuarioControllerTest
    {
        public readonly IUsuarioService MockUsuarioService;
        Mock<IUsuarioService> mockUsuarioService = new Mock<IUsuarioService>();
        Mock<IUsuarioService> _mockUsuarioService = new Mock<IUsuarioService>();
        public UsuarioControllerTest()
        {
            List<UsuarioRequest> usuarios = new List<UsuarioRequest>
            {
                new UsuarioRequest
                {
                    Id = 1,
                    Nome = "Joao",
                    Senha = "123456",
                    Email = "joao@empresa.com.br",
                    IsAtivo = true
                },
                new UsuarioRequest
                {
                    Id = 2,
                    Nome = "Maria",
                    Senha = "123456",
                    Email = "maria@empresa.com.br",
                    IsAtivo = true
                },
                new UsuarioRequest
                {
                    Id = 3,
                    Nome = "Antonio",
                    Senha = "123456",
                    Email = "antonio@empresa.com.br",
                    IsAtivo = true
                }
            };

            _mockUsuarioService.Setup(mr => mr.GetAll()).Returns(usuarios);

            _mockUsuarioService.Setup(mr => mr.GetPorId(
                It.IsAny<int>())).Returns((int i) => usuarios.Where(
                x => x.Id == i).SingleOrDefault());

            _mockUsuarioService.Setup(r => r.Remover(It.IsAny<int>()));

            _mockUsuarioService.Setup(mr => mr.Salvar(It.IsAny<UsuarioRequest>())).Returns(
                (UsuarioRequest target) =>
                {
                    if (target.Id.Equals(default(int)))
                    {
                        target.Id = usuarios.Count + 1;
                        usuarios.Add(target);
                    }
                    else
                    {
                        var original = usuarios.Where(
                            q => q.Id == target.Id).Single();

                        if (original == null)
                        {
                            return false;
                        }

                        original.Nome = target.Nome;
                        original.Email = target.Email;
                        original.Senha = target.Senha;
                        original.IsAtivo = target.IsAtivo;
                    }

                    return true;
                });
            _mockUsuarioService.Setup(mr => mr.Atualizar(It.IsAny<int>(), It.IsAny<UsuarioRequest>()))
                .Callback((int id, UsuarioRequest u) =>
                {
                    var original = usuarios.Where(
                            q => q.Id == u.Id.Value).Single();
                    original.Nome = u.Nome;

                    //do something
                });

            this.MockUsuarioService = _mockUsuarioService.Object;
        }


        [TestMethod]
        public void TestPostUsuario_OK()
        {

            Mock<IUsuarioService> mockUsuarioService = new Mock<IUsuarioService>();
            var controller = new UsuariosController(mockUsuarioService.Object);
            var item = GetDemoPessoa();
            var result = controller.PostUsuario(item) as CreatedAtActionResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);
        }

        //   [TestMethod]
        //   public async Task PutUsuario_ReturnOK()
        //   {
        //       Mock<IUsuarioService> mockUsuarioService = new Mock<IUsuarioService>();
        //       var controller = new UsuariosController(mockUsuarioService.Object);

        //       var request = new UsuarioRequest();

        //       var result = await controller.PutUsuario(2, request) as BadRequestObjectResult;
        ////       Assert.IsNotNull(result);
        //       Assert.AreEqual(400, result.StatusCode);
        //   }


        [TestMethod]
        public async Task TestPutUsuarioNOK_IdDiferente()
        {
            Mock<IUsuarioService> mockPessoaService = new Mock<IUsuarioService>();
            var controller = new UsuariosController(mockPessoaService.Object);

            // var item = GetDemoPessoa();
            var usuario = this.MockUsuarioService.GetPorId(1);
            var request = new UsuarioRequest
            {
                Id = usuario.Id,
                Nome = "joao005",
                Senha = usuario.Senha,
                Email = usuario.Email,
                IsAtivo = true
            };

            var result = await controller.PutUsuario(10, request) as BadRequestObjectResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);
        }
        [TestMethod]
        public async Task GetReturnsNotFound()
        {
            // Arrange
            Mock<IUsuarioService> mockPessoaService = new Mock<IUsuarioService>();
            var controller = new UsuariosController(mockPessoaService.Object);

            // Act
            var result = await controller.GetUsuarios(10) as NotFoundResult;

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotFoundResult));
        }
        [TestMethod]
        public async Task TestGetPessoaById_NOK()
        {
            Mock<IUsuarioService> mockPessoaService = new Mock<IUsuarioService>();
            var controller = new UsuariosController(mockPessoaService.Object);
            var result = await controller.GetUsuarios(30) as NotFoundResult;
            Assert.IsNotNull(result);
            Assert.AreEqual(404, result.StatusCode);
        }
        [TestMethod]
        public async Task TestDeleteReturnsOk()
        {
            // Arrange
            var mockService = new Mock<IUsuarioService>();
            var controller = new UsuariosController(mockService.Object);

            var usuario = this.MockUsuarioService.GetPorId(1);
            // Act
            var result = await controller.DeleteUsuario(1) as OkObjectResult;

            mockService.Verify(r => r.Remover(usuario.Id.Value), Times.Once());
        }
        [TestMethod]
        public void TestPostMethod()
        {
            // Arrange
            var mockRepository = new Mock<IUsuarioService>();
            var controller = new UsuariosController(mockRepository.Object);

            // Act
            var result = controller.PostUsuario(GetDemoPessoa()) as CreatedAtActionResult;

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual(201, result.StatusCode);
        }

        [TestMethod]
        public async Task TesteNome_Pessoa_Vazio_NOK()
        {

            var request = new UsuarioRequest { Nome = "" };

            Mock<IUsuarioService> mockPessoaService = new Mock<IUsuarioService>();
            var controller = new UsuariosController(mockPessoaService.Object);
            controller.ModelState.AddModelError("Nome", "O nome é obrigatório.");
            var result = controller.PostUsuario(request) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

        }
        [TestMethod]
        public async Task TesteNome_Usuario_Invalido_NOK()
        {

            var request = new UsuarioRequest { Nome = "Inválido" };

            Mock<IUsuarioService> mockPessoaService = new Mock<IUsuarioService>();
            var controller = new UsuariosController(mockPessoaService.Object);
            controller.ModelState.AddModelError("Nome", "O nome é invalido.");
            var result = controller.PostUsuario(request) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

        }
        public async Task TesteNome_Email_Vazio_NOK()
        {

            var request = new UsuarioRequest { Email = "" };

            Mock<IUsuarioService> mockPessoaService = new Mock<IUsuarioService>();
            var controller = new UsuariosController(mockPessoaService.Object);
            controller.ModelState.AddModelError("Email", "O email é obrigatório.");
            var result = controller.PostUsuario(request) as BadRequestObjectResult;

            Assert.IsNotNull(result);
            Assert.AreEqual(400, result.StatusCode);

        }

        UsuarioRequest GetDemoPessoa()
        {
            return new UsuarioRequest
            {
                Id = 1,
                Nome = "Santos",
                Senha = "123456",
                Email = "joao@empresa.com.br",
                IsAtivo = true
            };

        }
    }
}
