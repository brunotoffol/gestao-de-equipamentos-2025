using Microsoft.AspNetCore.Mvc;

namespace GestaoDeEquipamentos.ConsoleApp.Controllers
{
    [Route("/")]
    public class ControladorInicial : Controller
    {
        public IActionResult PaginalInicial()
        {
            string conteudo = System.IO.File.ReadAllText("Compartilhado/Html/PaginaInicial.html");

            return Content(conteudo, "text/html");
        }
    }
}
