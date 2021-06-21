using ApiCatalogoLivros.Entities;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace ApiCatalogoLivros.Repositories
{
    public class LivroSqlServerRepository : ILivroRepository
    {
        private readonly SqlConnection sqlConnection;

        public LivroSqlServerRepository(IConfiguration configuration)
        {
            sqlConnection = new SqlConnection(configuration.GetConnectionString("Default"));
        }

        public async Task<List<Livro>> Obter(int pagina, int quantidade)
        {
            var livros = new List<Livro>();

            var comando = $"select * from Livros order by id offset {((pagina - 1) * quantidade)} rows fetch next {quantidade} rows only";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                livros.Add(new Livro
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Autor = (string)sqlDataReader["Autor"],
                    Editora = (string)sqlDataReader["Editora"],
                    Preco = (double)sqlDataReader["Preco"]
                });
            }

            await sqlConnection.CloseAsync();

            return livros;
        }

        public async Task<Livro> Obter(Guid id)
        {
            Livro livro = null;

            var comando = $"select * from Livros where Id = '{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                livro = new Livro
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Autor = (string)sqlDataReader["Autor"],
                    Editora = (string)sqlDataReader["Editora"],
                    Preco = (double)sqlDataReader["Preco"]
                };
            }

            await sqlConnection.CloseAsync();

            return livro;
        }

        public async Task<List<Livro>> Obter(string nome, string autor, string editora)
        {
            var livros = new List<Livro>();

            var comando = $"select * from Livros where Nome = '{nome}' and Autor = '{autor}' and Editora = '{editora}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (sqlDataReader.Read())
            {
                livros.Add(new Livro
                {
                    Id = (Guid)sqlDataReader["Id"],
                    Nome = (string)sqlDataReader["Nome"],
                    Autor = (string)sqlDataReader["Autor"],
                    Editora = (string)sqlDataReader["Editora"],
                    Preco = (double)sqlDataReader["Preco"]
                });
            }

            await sqlConnection.CloseAsync();

            return livros;
        }

        public async Task Inserir(Livro livro)
        {
            var comando = $"insert Livros (Id, Nome, Autor, Editora, Preco) values ('{livro.Id}', '{livro.Nome}', '{livro.Autor}','{livro.Editora}', {livro.Preco.ToString().Replace(",", ".")})";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Atualizar(Livro livro)
        {
            var comando = $"update Livros set Nome = '{livro.Nome}', Autor = '{livro.Autor}', Editora = '{livro.Editora}', Preco = {livro.Preco.ToString().Replace(",", ".")} where Id = '{livro.Id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public async Task Remover(Guid id)
        {
            var comando = $"delete from Livros where Id = '{id}'";

            await sqlConnection.OpenAsync();
            SqlCommand sqlCommand = new SqlCommand(comando, sqlConnection);
            sqlCommand.ExecuteNonQuery();
            await sqlConnection.CloseAsync();
        }

        public void Dispose()
        {
            sqlConnection?.Close();
            sqlConnection?.Dispose();
        }
    }
}