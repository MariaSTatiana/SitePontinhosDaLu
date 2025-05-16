using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using MySql.Data.MySqlClient;
using PontinhosDaLu.Models;
using System.Collections.Generic;
using System.Linq;
using PontinhosDaLu.Helpers;


namespace PontinhosDaLu.Controllers
{
    public class ProdutoController : Controller
    {
        public IActionResult Index()
        {
            List<Produto> produtos = new List<Produto>();
            Conexao conexaoDB = new Conexao();
            MySqlConnection conexao = conexaoDB.ObterConexao();
            try
            {
                conexao.Open();
                string query = "SELECT * FROM produtos";
                MySqlCommand cmd = new MySqlCommand(query, conexao);
                MySqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Produto p = new Produto
                    {
                        Id = reader.GetInt32("id"),
                        NomeProduto = reader.GetString("nome"),
                        Preco = reader.GetDecimal("preco"),
                        Imagem = reader.GetString("imagem")
                    };
                    produtos.Add(p);
                }
                reader.Close();
            }
            finally
            {
                conexao.Close();
            }

            return View(produtos);
        }
        [HttpPost]
        public IActionResult AdicionarAoCarrinho(int id)
        {
            if (id <= 0)
            {
                return BadRequest("ID do produto inválido.");
            }

            List<Models.CarrinhoItem> carrinho = HttpContext.Session.GetObjectFromJson<List<Models.CarrinhoItem>>("carrinho") ?? new List<Models.CarrinhoItem>();

            var itemExistente = carrinho.FirstOrDefault(i => i.ProdutoId == id);
            if (itemExistente != null)
            {
                itemExistente.Quantidade++;
            }
            else
            {
                // Aqui você pode buscar mais informações do produto, se necessário, para salvar Nome, Preço e Imagem.
                carrinho.Add(new Models.CarrinhoItem { ProdutoId = id, Quantidade = 1 });
            }

            HttpContext.Session.SetObjectAsJson("carrinho", carrinho);

            // Redireciona para o mesmo página de produtos ou para a página de carrinho, conforme sua escolha.
            return RedirectToAction("Index");
        }

    }
}