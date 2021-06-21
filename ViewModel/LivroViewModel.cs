using System;

namespace ApiCatalogoLivros.ViewModel
{
    public class LivroViewModel
    {
        public Guid Id { get; set; }
        public string Nome { get; set; }
        public string Autor { get; set; }
        public string Editora { get; set; }
        public double Preco { get; set; }
    }
}