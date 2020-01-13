using System;
using System.Collections.Generic;
using System.Linq;
using ProjectBackendTest.Model;
using ProjectBackendTest.Models.Request;
using ProjectBackendTest.Repositories.Contracts;
using ProjectBackendTest.Services.Contracts;

namespace ProjectBackendTest.Services
{
    public  class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _repository;

        public  UsuarioService(IUsuarioRepository repository)
        {
            _repository = repository;
        }
        public bool Atualizar(int id, UsuarioRequest request)
        {
            try
            {
                var _usuario = _repository.Get(id);
                if(_usuario == null)
                {
                    return false;
                }
                _usuario.Email = request.Email;
                _usuario.Nome = request.Nome;
                _usuario.Senha = request.Senha;
                _usuario.IsAtivo = request.IsAtivo;
                 _repository.Update(_usuario);
                 _repository.Save();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public bool Atualizar(Usuario _usuario)
        {
            try
            {
                _repository.Update(_usuario);
                _repository.Save();
                return true;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }

        public UsuarioRequest GetPorId(int id)
        {
            var usuario = _repository.Get(id);

            if (usuario == null)
            {
                return null;
            }
            var response = new UsuarioRequest
            {
                Id = usuario.Id,
                Nome = usuario.Nome,
                Email = usuario.Email,
                IsAtivo = usuario.IsAtivo,

            };
            return response;
        }

        public List<UsuarioRequest> GetAll()
        {
            List<UsuarioRequest> responseList = new List<UsuarioRequest>();
            var result = _repository.GetAll();

            foreach(Usuario usuario in result){
                var response = new UsuarioRequest
                {
                    Id = usuario.Id,
                    Nome = usuario.Nome,
                    Email = usuario.Email,
                    IsAtivo = usuario.IsAtivo,
                };
                responseList.Add(response);

            }

            return responseList;
        }

        public bool Remover(int id)
        {
            try
            {
                var pessoa = _repository.Get(id);
                if (pessoa == null)
                {
                    return false;
                }
                _repository.Remove(pessoa);
                _repository.Save();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public bool Salvar(UsuarioRequest request)
        {
            try
            {
                var _usuario = new Usuario
                {
                    Nome = request.Nome,
                    Email = request.Email,
                    DecryptedPassword = request.Senha,
                    IsAtivo = request.IsAtivo
                };
                _repository.Add(_usuario);
                _repository.Save();
                return true;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public Usuario GetPorLogin(string Login)
        {
            return _repository.Find(x => x.Nome == Login).FirstOrDefault();
        }
    }
}
