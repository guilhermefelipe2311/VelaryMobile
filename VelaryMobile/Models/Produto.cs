namespace VelaryMobile.Models
{
    public class Produto
    {
        public int Id { get; set; }

        public string Nome { get; set; } = string.Empty;

        public string Descricao { get; set; } = string.Empty;

        public decimal Preco { get; set; }

        public int Quantidade { get; set; }

        public string ImagemUrl { get; set; } = string.Empty;

        public int CategoriaId { get; set; }

        public Categoria? Categoria { get; set; }
    }
}