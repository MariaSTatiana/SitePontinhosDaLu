using Microsoft.AspNetCore.Mvc;
using PontinhosDaLu.Helpers;
using PontinhosDaLu.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;

namespace PontinhosDaLu.Controllers
{
    public class CarrinhoController : Controller
    {
        private static List<CarrinhoItem> carrinho = new List<CarrinhoItem>();
        public IActionResult Index()
        {
            // Recupera o carrinho da sessão (ou cria um novo se estiver vazio)   
            var carrinho = HttpContext.Session.GetObjectFromJson<List<ItemCarrinho>>("Carrinho") ?? new List<ItemCarrinho>();
            return View(carrinho);
        }

        public IActionResult Adicionar(int id, string nome, decimal preco, string imagem)
        {
            // Recupera o carrinho da sessão   
            var carrinho = HttpContext.Session.GetObjectFromJson<List<ItemCarrinho>>("Carrinho") ?? new List<ItemCarrinho>();

            // Verifica se o item já existe no carrinho   
            var itemExistente = carrinho.FirstOrDefault(p => p.ProdutoId == id);
            if (itemExistente != null)
            {
                itemExistente.Quantidade++;
            }
            else
            {
                carrinho.Add(new ItemCarrinho
                {
                    ProdutoId = id,
                    NomeProduto = nome, // FIX: Change the type of Nome in ItemCarrinho to string   
                    Preco = preco,
                    Imagem = imagem,
                    Quantidade = 1
                });
            }

            // Salva o carrinho de volta na sessão   
            HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);

            return RedirectToAction("Index");
        }


        public IActionResult Remover(int id)
        {
            var carrinho = HttpContext.Session.GetObjectFromJson<List<ItemCarrinho>>("Carrinho") ?? new List<ItemCarrinho>();

            var itemExistente = carrinho.Find(p => p.ProdutoId == id);
            if (itemExistente != null)
            {
                carrinho.Remove(itemExistente);
            }

            HttpContext.Session.SetObjectAsJson("Carrinho", carrinho);
            return RedirectToAction("Index");
        }
        public IActionResult ZerarCarrinho()
        {
            HttpContext.Session.Remove("Carrinho");
            return RedirectToAction("Index");
        }

        public IActionResult FinalizarCompra(string email, string pagamento)
        {
            var carrinho = HttpContext.Session.GetObjectFromJson<List<ItemCarrinho>>("Carrinho") ?? new List<ItemCarrinho>();

            if (!carrinho.Any())
            {
                TempData["Mensagem"] = "Seu carrinho está vazio!";
                return RedirectToAction("Index");
            }

            string mensagem = "<h2>Confirmação de Compra</h2><p>Obrigado por sua compra!</p><ul>";
            foreach (var item in carrinho)
            {
                mensagem += $"<li>{item.NomeProduto} - R$ {item.Preco.ToString("F2")} (Qtd: {item.Quantidade})</li>";
            }
            mensagem += $"</ul><p>Forma de pagamento escolhida: <strong>{pagamento}</strong></p>";
            mensagem += "<p><strong>Pix:</strong> 000.000.000-00</p>";
            mensagem += "<p><strong>Transferência Bancária:</strong> Banco XYZ, Agência 1234, Conta 56789-0</p>";

            var smtpClient = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                Credentials = new NetworkCredential("seuemail@gmail.com", "suasenha"),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress("seuemail@gmail.com"),
                Subject = "Confirmação de Compra",
                Body = mensagem,
                IsBodyHtml = true,
            };

            mailMessage.To.Add(email);
            smtpClient.Send(mailMessage);

            HttpContext.Session.Remove("Carrinho"); // Limpa o carrinho após a compra

            TempData["Mensagem"] = "Compra finalizada! Um e-mail de confirmação foi enviado.";
            return RedirectToAction("Index");
        }

        public class ItemCarrinho
        {
            public int ProdutoId { get; set; }
            public string NomeProduto { get; set; } = string.Empty; // FIX: Change the type of Nome from int to string   
            public decimal Preco { get; set; }
            public string Imagem { get; set; } = string.Empty;
            public int Quantidade { get; set; }
        }
    }
}


