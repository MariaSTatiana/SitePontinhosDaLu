using Microsoft.AspNetCore.Mvc;
using MySql.Data.MySqlClient;
using PontinhosDaLu.Models;
using System.Collections.Generic;


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
                        Nome = reader.GetString("nome"),
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
    }
}