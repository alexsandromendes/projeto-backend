using ProjectBackendTest.Model;
using ProjectBackendTest.Models.Request;
using System;
using System.Collections.Generic;

namespace ProjectBackendTest.Services.Contracts
{
    public interface IUsuarioService
    {
        UsuarioRequest GetPorId(int Id);
        List<UsuarioRequest> GetAll();
        bool Salvar(UsuarioRequest UsuarioRequest);
        bool Atualizar(int Id, UsuarioRequest UsuarioRequest);
        bool Atualizar(Usuario Usuario);
        bool Remover(int Id);
        Usuario GetPorLogin(string login);
    }
}
