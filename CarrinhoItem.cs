namespace PontinhosDaLu.Models
{
    public class CarrinhoItem
    {
        public int ProdutoId { get; set; }
        public string NomeProduto { get; set; } = string.Empty;
        public decimal Preco { get; set; }
        public string Imagem { get; set; } = string.Empty;
        public int Quantidade { get; set; }
    }
}
