using Demo.Domain.Produto;
using Demo.Infra.Data;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;

namespace Demo.Repository
{
    public class RepositoryProduto : InterfaceProduto
    {
        private readonly ContextBase _dbContext;

        public RepositoryProduto(ContextBase dbContext) => _dbContext = dbContext;

        public async Task Add(Produto objeto)
        {
            await _dbContext.Produtos.AddAsync(objeto);
            await _dbContext.SaveChangesAsync();
        }

        public async Task Delete(Produto objeto)
        {
            _dbContext.Produtos.Remove(objeto);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<Produto> GetEntityById(Guid id)
        {
            var produto = await _dbContext.Produtos.FindAsync(id);
            return produto;
        }

        public async Task<List<Produto>> GetList()
        {
            return await _dbContext.Produtos.ToListAsync();
        }

        public async Task Update(Produto objeto)
        {
            _dbContext.Produtos.Update(objeto);
            await _dbContext.SaveChangesAsync();
        }
    }
}
