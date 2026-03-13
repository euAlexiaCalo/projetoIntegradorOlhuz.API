using Microsoft.AspNetCore.Mvc;

namespace projetoIntegradorOlhuz.API.Controllers
{
    public class UsuarioController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
