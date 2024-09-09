using Lanchonete.Models;
using Lanchonete.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
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

        [Authorize]
        [HttpGet] // Formulário de confirmação
        public IActionResult Checkout()
        {
            return View();
        }

        [Authorize]
        [HttpPost] // Processamento do pedido
        public IActionResult Checkout(Pedido pedido)
        {
            int totalItensPedido = 0;
            decimal precoTotalPedido = 0.0m;

            // obtem os itens do carrinho de compra do cliente
            List<CarrinhoCompraItem> items = _carrinhoCompra.GetCarrinhoCompraItens();
            _carrinhoCompra.CarrinhoCompraItems = items;

            // verifica se existem itens no pedido
            if(_carrinhoCompra.CarrinhoCompraItems.Count == 0)
            {
                ModelState.AddModelError("", "Seu carrinho esta vazio, que tal incluir um lanche...");
            }

            // calcular o total de itens e o total do pedido
            foreach(var item in items)
            {
                totalItensPedido += item.Quantidade;
                precoTotalPedido += (item.Lanche.Preco * item.Quantidade);
            }

            // atribui os valores obtidos ao pedido
            pedido.TotalItensPedido = totalItensPedido;
            pedido.PedidoTotal = precoTotalPedido;

            // valida os dados do pedido
            if (ModelState.IsValid)
            {
                // criar o pedido e os detalhes
                _pedidoRepository.CriarPedido(pedido);

                // define mensagens ao cliente
                ViewBag.TotalPedido = _carrinhoCompra.GetCarrinhoCompraTotal();

                // limpar o carrinho ao concluir o pedido
                _carrinhoCompra.LimparCarrinho();

                // exibe a View com dados do cliente e do pedido concluido
                return View("~/Views/Pedido/CheckoutCompleto.cshtml", pedido);
            }
            return View(pedido);
        }

    }
}
