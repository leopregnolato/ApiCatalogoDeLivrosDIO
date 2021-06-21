using ApiCatalogoLivros.Entities;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApiCatalogoLivros.Repositories
{
    public interface ILivroRepository : IDisposable
    {
        Task<List<Livro>> Obter(int pagina, int quantidade);
        Task<Livro> Obter(Guid id);
        Task<List<Livro>> Obter(string nome, string autor, string editora);        
        Task Inserir(Livro jogo);
        Task Atualizar(Livro jogo);
        Task Remover(Guid id);
    }
}