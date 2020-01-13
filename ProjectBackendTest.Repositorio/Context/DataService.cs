using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using ProjectBackendTest.Model;
using ProjectBackendTest.Repositories.Contracts;
using ProjectBackendTest.Repository.Context;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace ProjectBackendTest.Repositories.Context
{
    public class DataService: IDataService
    {
        private readonly BancoContext contexto;

        public DataService(BancoContext contexto)
        {
            this.contexto = contexto;
        }
        public void InicializaDB()
        {
            contexto.Database.EnsureCreated();
            if (contexto.Usuarios.Any())
            {
                return;
            }

            contexto
               .Set<Usuario>()
                .Add(new Usuario
                {
                    Nome = "administrador",
                    Email = "admistrador@empresa.com.br",
                    IsAtivo = true,
                    DecryptedPassword = "admin@12345"
                });
            contexto.SaveChanges();
        }
        
    }
}
