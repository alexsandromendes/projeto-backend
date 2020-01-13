using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectBackendTest.Model;
using ProjectBackendTest.Models.Request;
using ProjectBackendTest.Repositories.Contracts;
using ProjectBackendTest.Repository;
using ProjectBackendTest.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBackend.Test
{
    [TestClass]
    public class UsuarioServiceTest
    {
        public readonly IUsuarioRepository MockUsuarioRepository;
        Mock<IUsuarioRepository> mockUsuarioRepository = new Mock<IUsuarioRepository>();

        public UsuarioServiceTest()
        {
            List<Usuario> usuarios = new List<Usuario>
            {
                new Usuario
                {
                    Id = 1,
                    Nome = "Joao",
                    Senha = "123456",
                    Email = "joao@empresa.com.br",
                    IsAtivo = true
                },
                new Usuario
                {
                    Id = 2,
                    Nome = "Maria",
                    Senha = "123456",
                    Email = "maria@empresa.com.br",
                    IsAtivo = true
                },
                new Usuario
                {
                    Id = 3,
                    Nome = "Antonio",
                    Senha = "123456",
                    Email = "antonio@empresa.com.br",
                    IsAtivo = true
                }
            };



            mockUsuarioRepository.Setup(mr => mr.GetAll()).Returns(usuarios);

            mockUsuarioRepository.Setup(mr => mr.Get(
                It.IsAny<int>())).Returns((int i) => usuarios.Where(
                x => x.Id == i).SingleOrDefault());

            mockUsuarioRepository.Setup(r => r.Remove(It.IsAny<Usuario>()));


            // mockPessoaRepository.Setup(r => r.Update(It.IsAny<Pessoa>()));
            mockUsuarioRepository.Setup(mr => mr.Update(It.IsAny<Usuario>()))
                   .Callback((Usuario pessoa) =>
                   {
                       var original = usuarios.Where(
                            q => q.Id == pessoa.Id).Single();
                       original = pessoa;

                       //do something
                   });
;

            this.MockUsuarioRepository = mockUsuarioRepository.Object;
        }
        [TestMethod]
        public void TestSalvarPessoa()
        {
            var _service = new UsuarioService(this.MockUsuarioRepository); 
            _service.Salvar(GetDemoPessoa());
            mockUsuarioRepository.Verify(r => r.Add(It.IsAny<Usuario>()), Times.Exactly(1));
            mockUsuarioRepository.Verify(r => r.Add(It.IsAny<Usuario>()), Times.Once());
        }
        [TestMethod]
        public void TestDeleteOK()
        {
            var _service = new UsuarioService(this.MockUsuarioRepository);
            var usuario = this.MockUsuarioRepository.Get(1);

            _service.Remover(1);

            mockUsuarioRepository.Verify(r => r.Remove(usuario), Times.Once());
        }
        [TestMethod]
        public void TestGetPessoa()
        {
            var _service = new UsuarioService(this.MockUsuarioRepository);
            var usuarioRepository = this.MockUsuarioRepository.Get(2);

            var usuarioService =   _service.GetPorId(2);
            Assert.AreEqual(usuarioRepository.Nome, usuarioService.Nome);
        }
        [TestMethod]
        public void TestGetAll()
        {
            var _service = new UsuarioService(this.MockUsuarioRepository);

            var usuarioService = _service.GetAll();

            Assert.AreEqual(3 , usuarioService.Count);
        }

        [TestMethod]
        public void TestAtualizarOK()
        {
            var _service = new UsuarioService(this.MockUsuarioRepository);
            var usuario = this.MockUsuarioRepository.Get(2);

            usuario.Nome = "Andreia";

            _service.Atualizar(usuario);
            usuario = this.MockUsuarioRepository.Get(2);
            Assert.AreEqual(usuario.Nome, "Andreia");
        }

        UsuarioRequest GetDemoPessoa()
        {
            return new UsuarioRequest
            {
                Id = 1,
                Nome = "Funlano",
                Senha = "1234564545454545",
                Email = "joao@empresa.com.br",
                IsAtivo = true
            };

        }
    }
}
