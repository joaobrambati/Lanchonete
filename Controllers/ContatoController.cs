using Microsoft.AspNetCore.Mvc;

namespace Lanchonete.Controllers
{
    public class ContatoController : Controller
    {
        public IActionResult Index ()
        {
            return View();
        }
    }
}
