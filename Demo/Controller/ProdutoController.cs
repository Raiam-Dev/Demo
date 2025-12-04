using Demo.Domain.Produto;
using Demo.Repository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Demo.Controller
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
    public class ProdutoController : ControllerBase
    {
        private readonly InterfaceProduto _repositoryProduto;

        public ProdutoController(InterfaceProduto repositoryProduto)
        {
            _repositoryProduto = repositoryProduto;
        }

        [HttpGet("todos-produtos")]
        public async Task<IActionResult> TodosProdutos() => Ok(await _repositoryProduto.GetList());

        [HttpGet("pegar-produto")]
        public async Task<IActionResult> PegarProduto (Guid id) 
        {
          var produto = await _repositoryProduto.GetEntityById(id);
          if(produto == null) return NoContent();
          return Ok(produto);
        }

        [HttpPost("criar-produto")]
        public async Task<IActionResult> CriarProduto(Produto produto)
        {
          if(produto.IsValid()) await _repositoryProduto.Add(produto);

          return CreatedAtAction(nameof(CriarProduto), new { id = produto.Id }, produto);
        }

        [HttpPut("atualizar-produto")]
        public async Task<IActionResult> AtualizarProduto(Produto produto)
        {
            if (produto.IsValid()) await _repositoryProduto.Update(produto);

            return Ok(produto);
        }

        [HttpDelete("deletar-produto")]
        public async Task<IActionResult> DeletarProduto(Produto produto)
        {
            if (produto.IsValid()) await _repositoryProduto.Delete(produto);

            return Ok();
        }

    }
}
