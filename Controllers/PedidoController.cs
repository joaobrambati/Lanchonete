using Lanchonete.Models;
using Lanchonete.Repositories.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Lanchonete.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository _pedidoRepository;
        private readonly CarrinhoCompra _carrinhoCompra;

        public PedidoController(IPedidoRepository pedidoRepository, CarrinhoCompra carrinhoCompra)
        {
            _pedidoRepository = pedidoRepository;
            _carrinhoCompra = carrinhoCompra;
        }

        [HttpGet] // Formulário de confirmação
        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost] // Processamento
        public IActionResult Checkout(Pedido pedido)
        {
            return View(pedido);
        }

    }
}
