

using ProjectBackendTest.Model;

namespace ProjectBackendTest.Repositories.Contracts
{
    public interface IUsuarioRepository:IBaseRepository<Usuario>
    {
        bool Adicionar(Usuario pessoa);
    }
}
