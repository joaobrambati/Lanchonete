using Lanchonete.Context;
using Lanchonete.Models;
using Lanchonete.Repositories.Interfaces;
using Lanchonete.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace Lanchonete.Controllers
{
    public class LancheController : Controller
    {
        private readonly ILancheRepository _lancheRepository;

        public LancheController (ILancheRepository lancheRepository)
        {
            _lancheRepository = lancheRepository;
        }

        public IActionResult List (string categoria)
        {
            IEnumerable<Lanche> lanches;
            string categoriaAtual = string.Empty;

            if (string.IsNullOrEmpty(categoria))
            {
                lanches = _lancheRepository.Lanches
                          .OrderBy(x => x.LancheId);
                categoriaAtual = "Todos Lanches";
            }
            else
            {
                if (string.Equals("Normal", categoria, StringComparison.OrdinalIgnoreCase))
                {
                    lanches = _lancheRepository.Lanches
                              .Where(x => x.Categoria.CategoriaNome.Equals("Normal"))
                              .OrderBy(x => x.Nome);
                }
                else
                {
                    lanches = _lancheRepository.Lanches
                              .Where(x => x.Categoria.CategoriaNome.Equals("Natural"))
                              .OrderBy(x => x.Nome);
                }
                categoriaAtual = categoria;
            }

            var lanchesListViewModel = new LancheListViewModel
            {
                Lanches = lanches,
                CategoriaAtual = categoriaAtual
            };

            return View(lanchesListViewModel);
        }

    }
}
