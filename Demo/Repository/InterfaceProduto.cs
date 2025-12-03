using Demo.Domain.Produto;

namespace Demo.Repository
{
    public interface InterfaceProduto
    {
        Task Add(Produto objeto);
        Task Delete (Produto objeto);
        Task Update(Produto objeto);
        Task<Produto> GetEntityById(int id);
        Task<List<Produto>> GetList();
    }
}
