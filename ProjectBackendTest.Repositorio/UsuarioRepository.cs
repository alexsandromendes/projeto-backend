using Microsoft.EntityFrameworkCore;
using ProjectBackendTest.Model;
using ProjectBackendTest.Repositories;
using ProjectBackendTest.Repositories.Contracts;
using ProjectBackendTest.Repository.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ProjectBackendTest.Repository
{
    public class UsuarioRepository : BaseRepository<Usuario>, IUsuarioRepository
    {
        private readonly BancoContext _context;

        public UsuarioRepository(BancoContext context):base(context)
        {}
        private BancoContext _appContext => (BancoContext)_context;

        public bool Adicionar(Usuario usuario)
        {
            _context.Add(usuario);
            _context.SaveChanges();
            return true;
        }
    }

}
