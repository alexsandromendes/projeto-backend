using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using ProjectBackendTest.Model;
using ProjectBackendTest.Repositories.Contracts;
using ProjectBackendTest.Repository;
using ProjectBackendTest.Repository.Context;
using System.Collections.Generic;
using System.Linq;

namespace ProjectBackend.Test
{
    [TestClass]
    public class UsuarioRepositoryTest
    {
        public readonly IUsuarioRepository MockUsuarioRepository;
        public UsuarioRepositoryTest()
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

            Mock<IUsuarioRepository> mockUsuarioRepository = new Mock<IUsuarioRepository>();

            mockUsuarioRepository.Setup(mr => mr.GetAll()).Returns(usuarios);

            mockUsuarioRepository.Setup(mr => mr.Get(
                It.IsAny<int>())).Returns((int i) => usuarios.Where(
                x => x.Id == i).Single());


            mockUsuarioRepository.Setup(mr => mr.Adicionar(It.IsAny<Usuario>())).Returns(
                (Usuario target) =>
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

            this.MockUsuarioRepository = mockUsuarioRepository.Object;
        }
        //public BancoContext context;

        //public readonly IUsuarioRepository MockUsuarioRepository;
        

        [TestMethod]
        public void TestGetUsuario()
        {
            int id = 1;
            Usuario usuario = this.MockUsuarioRepository.Get(id);

            Assert.IsNotNull(usuario); 
            Assert.IsInstanceOfType(usuario, typeof(Usuario));
            Assert.AreEqual("Joao", usuario.Nome); 
        }
        [TestMethod]
        public void TestReturnAllUsuario()
        {
            // Try finding all products
            IList<Usuario> usuarios= this.MockUsuarioRepository.GetAll().ToList();

            Assert.IsNotNull(usuarios); 
            Assert.AreEqual(3, usuarios.Count); 
        }
        [TestMethod]
        public void TestInsert()
        {
            Usuario newUsuario = new Usuario
            {
                Nome = "fulano",
                Senha = "123456",
                Email = "fulano@empresa.com.br",
                IsAtivo = true
            };

            int usuarioCount = this.MockUsuarioRepository.GetAll().ToList().Count;
            Assert.AreEqual(3, usuarioCount);

            this.MockUsuarioRepository.Adicionar(newUsuario);

            Usuario usuario2 = this.MockUsuarioRepository.Get(4);
            usuarioCount = this.MockUsuarioRepository.GetAll().ToList().Count;
            Assert.AreEqual(4, usuarioCount);

            Usuario usuario = this.MockUsuarioRepository.Get(4);
            Assert.IsNotNull(usuario); 
            Assert.AreEqual("fulano", usuario.Nome);
            Assert.IsInstanceOfType(usuario, typeof(Usuario)); 
            Assert.AreEqual(4, usuario.Id); 
        }
        [TestMethod]
        public void TestUpdate()
        {
            Usuario usuario = this.MockUsuarioRepository.Get(2);
            usuario.Nome = "ana";
            usuario.IsAtivo = false;
            this.MockUsuarioRepository.Update(usuario);
            this.MockUsuarioRepository.Save();
            var usuarioAtualizado = this.MockUsuarioRepository.Get(1);
            //Assert.AreEqual("and", usuarioAtualizado.Nome);
            //Assert.AreEqual(false, usuarioAtualizado.IsAtivo);
        }

    }
}
