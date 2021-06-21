using ApiCatalogoLivros.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ApiCatalogoLivros.Repositories
{
    public class LivroRepository : ILivroRepository
    {
        private static Dictionary<Guid, Livro> livros = new Dictionary<Guid, Livro>()
        {
            {Guid.Parse("0ca314a5-9282-45d8-92c3-2985f2a9fd04"), new Livro{ Id = Guid.Parse("0ca314a5-9282-45d8-92c3-2985f2a9fd04"), Nome = "Dom Casmurro", Autor="Machado de Assis", Editora = "Atica", Preco = 200} },
            {Guid.Parse("eb909ced-1862-4789-8641-1bba36c23db3"), new Livro{ Id = Guid.Parse("eb909ced-1862-4789-8641-1bba36c23db3"), Nome = "1Q84", Autor="Hariki Murakami", Editora = "Moderna", Preco = 190} },
            {Guid.Parse("5e99c84a-108b-4dfa-ab7e-d8c55957a7ec"), new Livro{ Id = Guid.Parse("5e99c84a-108b-4dfa-ab7e-d8c55957a7ec"), Nome = "Poesias", Autor="Carlos Drummond de Andrade", Editora = "Abril", Preco = 180} },
            {Guid.Parse("da033439-f352-4539-879f-515759312d53"), new Livro{ Id = Guid.Parse("da033439-f352-4539-879f-515759312d53"), Nome = "A revolução dos Bichos", Autor="George Orwell", Editora = "Saraiva", Preco = 170} },
            {Guid.Parse("92576bd2-388e-4f5d-96c1-8bfda6c5a268"), new Livro{ Id = Guid.Parse("92576bd2-388e-4f5d-96c1-8bfda6c5a268"), Nome = "Tieta", Autor="Jorge Amado", Editora = "Globo", Preco = 80} },
            {Guid.Parse("c3c9b5da-6a45-4de1-b28b-491cbf83b589"), new Livro{ Id = Guid.Parse("c3c9b5da-6a45-4de1-b28b-491cbf83b589"), Nome = "Cem anos de Solidão", Autor="Gabriel Garcia Marquez", Editora = "Sextante", Preco = 190} }
        };

        public Task<List<Livro>> Obter(int pagina, int quantidade)
        {
            return Task.FromResult(livros.Values.Skip((pagina - 1) * quantidade).Take(quantidade).ToList());
        }

        public Task<Livro> Obter(Guid id)
        {
            if (!livros.ContainsKey(id))
                return Task.FromResult<Livro>(null);

            return Task.FromResult(livros[id]);
        }

        public Task<List<Livro>> Obter(string nome, string autor, string editora)
        {
            return Task.FromResult(livros.Values.Where(livro => livro.Nome.Equals(nome) && livro.Autor.Equals(autor) && livro.Editora.Equals(editora)).ToList());
        }

        public Task<List<Livro>> ObterSemLambda(string nome, string autor, string editora)
        {
            var retorno = new List<Livro>();

            foreach(var livro in livros.Values)
            {
                if (livro.Nome.Equals(nome) && livro.Autor.Equals(autor) && livro.Editora.Equals(editora))
                    retorno.Add(livro);
            }

            return Task.FromResult(retorno);
        }

        public Task Inserir(Livro livro)
        {
            livros.Add(livro.Id, livro);
            return Task.CompletedTask;
        }

        public Task Atualizar(Livro livro)
        {
            livros[livro.Id] = livro;
            return Task.CompletedTask;
        }

        public Task Remover(Guid id)
        {
            livros.Remove(id);
            return Task.CompletedTask;
        }

        public void Dispose()
        {
            //Fechar conexão com o banco
        }
    }
}