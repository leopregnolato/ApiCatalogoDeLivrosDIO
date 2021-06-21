using ApiCatalogoLivros.Exceptions;
using ApiCatalogoLivros.InputModel;
using ApiCatalogoLivros.Services;
using ApiCatalogoLivros.ViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoLivros.Controllers.V1
{
    [Route("api/v1/[controller]")]
    [ApiController]
    public class LivrosController : ControllerBase
    {
        private readonly ILivroService _livroService;

        public LivrosController(ILivroService livroService)
        {
            _livroService = livroService;
        }

        /// <summary>
        /// Buscar todos os jogos de forma paginada
        /// </summary>
        /// <remarks>
        /// Não é possível retornar os jogos sem paginação
        /// </remarks>
        /// <param name="pagina">Indica qual página está sendo consultada. Mínimo 1</param>
        /// <param name="quantidade">Indica a quantidade de reistros por página. Mínimo 1 e máximo 50</param>
        /// <response code="200">Retorna a lista de jogos</response>
        /// <response code="204">Caso não haja jogos</response>   
        [HttpGet]
        public async Task<ActionResult<IEnumerable<LivroViewModel>>> Obter([FromQuery, Range(1, int.MaxValue)] int pagina = 1, [FromQuery, Range(1, 50)] int quantidade = 5)
        {
            var livros = await _livroService.Obter(pagina, quantidade);

            if (livros.Count() == 0)
                return NoContent();

            return Ok(livros);
        }

        /// <summary>
        /// Buscar um jogo pelo seu Id
        /// </summary>
        /// <param name="idJogo">Id do jogo buscado</param>
        /// <response code="200">Retorna o jogo filtrado</response>
        /// <response code="204">Caso não haja jogo com este id</response>   
        [HttpGet("{idJogo:guid}")]
        public async Task<ActionResult<LivroViewModel>> Obter([FromRoute] Guid idLivro)
        {
            var livro = await _livroService.Obter(idLivro);

            if (livro == null)
                return NoContent();

            return Ok(livro);
        }

        /// <summary>
        /// Inserir um jogo no catálogo
        /// </summary>
        /// <param name="jogoInputModel">Dados do jogo a ser inserido</param>
        /// <response code="200">Cao o jogo seja inserido com sucesso</response>
        /// <response code="422">Caso já exista um jogo com mesmo nome para a mesma produtora</response>   
        [HttpPost]
        public async Task<ActionResult<LivroViewModel>> InserirLivro([FromBody] LivroInputModel livroInputModel)
        {
            try
            {
                var livro = await _livroService.Inserir(livroInputModel);

                return Ok(livro);
            }
            catch (LivroJaCadastradoException ex)
            {
                return UnprocessableEntity("Já existe um livro com este nome para esta editora");
            }
        }

        /// <summary>
        /// Atualizar um jogo no catálogo
        /// </summary>
        /// /// <param name="idJogo">Id do jogo a ser atualizado</param>
        /// <param name="jogoInputModel">Novos dados para atualizar o jogo indicado</param>
        /// <response code="200">Cao o jogo seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>   
        [HttpPut("{idJogo:guid}")]
        public async Task<ActionResult> AtualizarLivro([FromRoute] Guid idJogo, [FromBody] JogoInputModel jogoInputModel)
        {
            try
            {
                await _livroService.Atualizar(idLivro, livroInputModel);

                return Ok();
            }
            catch (LivroNaoCadastradoException ex)
            {
                return NotFound("Não existe este livro");
            }
        }

        /// <summary>
        /// Atualizar o preço de um jogo
        /// </summary>
        /// /// <param name="idJogo">Id do jogo a ser atualizado</param>
        /// <param name="preco">Novo preço do jogo</param>
        /// <response code="200">Cao o preço seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>   
        [HttpPatch("{idJogo:guid}/preco/{preco:double}")]
        public async Task<ActionResult> AtualizarLivro([FromRoute] Guid idLivro, [FromRoute] double preco)
        {
            try
            {
                await _livroService.Atualizar(idLivro, preco);

                return Ok();
            }
            catch (LivroNaoCadastradoException ex)
            {
                return NotFound("Não existe este livro");
            }
        }

        /// <summary>
        /// Excluir um jogo
        /// </summary>
        /// /// <param name="idJogo">Id do jogo a ser excluído</param>
        /// <response code="200">Cao o preço seja atualizado com sucesso</response>
        /// <response code="404">Caso não exista um jogo com este Id</response>   
        [HttpDelete("{idJogo:guid}")]
        public async Task<ActionResult> ApagarLivro([FromRoute] Guid idLivro)
        {
            try
            {
                await _livroService.Remover(idLivro);

                return Ok();
            }
            catch (LivroNaoCadastradoException ex)
            {
                return NotFound("Não existe este livro");
            }
        }

    }
}