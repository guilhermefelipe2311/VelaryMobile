using VelaryMobile.Models;

namespace VelaryMobile.Services
{
    public class CarrinhoService
    {
        private readonly List<ItemPedido> _itens = new();

        public List<ItemPedido> Itens => _itens;

        public void AdicionarProduto(Produto produto)
        {
            var itemExistente = _itens.FirstOrDefault(
                item => item.ProdutoId == produto.Id);

            if (itemExistente != null)
            {
                itemExistente.Quantidade++;
                return;
            }

            _itens.Add(new ItemPedido
            {
                ProdutoId = produto.Id,
                Produto = produto,
                Quantidade = 1,
                PrecoUnitario = produto.Preco
            });
        }

        public void RemoverProduto(Produto produto)
        {
            var item = _itens.FirstOrDefault(
                item => item.ProdutoId == produto.Id);

            if (item != null)
            {
                _itens.Remove(item);
            }
        }

        public decimal CalcularTotal()
        {
            return _itens.Sum(
                item => item.Quantidade * item.PrecoUnitario);
        }

        public void Limpar()
        {
            _itens.Clear();
        }
    }
}