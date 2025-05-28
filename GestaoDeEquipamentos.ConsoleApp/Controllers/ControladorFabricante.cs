using GestaoDeEquipamentos.ConsoleApp.Compartilhado;
using GestaoDeEquipamentos.ConsoleApp.ModuloFabricante;
using Microsoft.AspNetCore.Mvc;
using System.Text;

namespace GestaoDeEquipamentos.ConsoleApp.Controllers
{
    [Route("fabricantes")]
    public class ControladorFabricante : Controller
    {
        [HttpGet("cadastrar")]
        public IActionResult ExibirFormularioCadastroFabricante([FromForm] string nome, [FromForm] string email, [FromForm] string telefone)
        {
            string conteudo = System.IO.File.ReadAllText("ModuloFabricante/Html/Cadastrar.html");

            return Content(conteudo, "text/html");
        }

        [HttpPost("cadastrar")]
        public IActionResult CadastrarFabricante([FromForm] string nome, [FromForm] string email, [FromForm] string telefone)
        {
            ContextoDados contextoDados = new ContextoDados(true);
            IRepositorioFabricante repositorioFabricante = new RepositorioFabricanteEmArquivo(contextoDados);

            Fabricante novoFabricante = new Fabricante(nome, email, telefone);

            repositorioFabricante.CadastrarRegistro(novoFabricante);

            string conteudo = System.IO.File.ReadAllText("Compartilhado/Html/Notificacao.html");

            StringBuilder sb = new StringBuilder(conteudo);

            sb.Replace("#mensagem#", $"O registro foi \"{novoFabricante.Nome}\" foi cadastrado com sucesso!");

            string conteudoString = sb.ToString();

            return Content(conteudoString, "text/html");
        }

        [HttpGet("editar/{id:int}")]
        public IActionResult ExibirFormularioEdicaoFabricante([FromRoute] int id)
        {
            ContextoDados contextoDados = new ContextoDados(true);
            IRepositorioFabricante repositorioFabricante = new RepositorioFabricanteEmArquivo(contextoDados);            

            Fabricante fabricanteSelecionado = repositorioFabricante.SelecionarRegistroPorId(id);

            string conteudo = System.IO.File.ReadAllText("ModuloFabricante/Html/Editar.html");

            StringBuilder sb = new StringBuilder(conteudo);

            sb.Replace("#id#", id.ToString());
            sb.Replace("#nome#", fabricanteSelecionado.Nome);
            sb.Replace("#email#", fabricanteSelecionado.Email);
            sb.Replace("#telefone#", fabricanteSelecionado.Telefone);

            string conteudoString = sb.ToString();

            return Content(conteudoString, "text/html");
        }

        [HttpPost("editar/{id:int}")]
        public IActionResult EditarFabricante([FromRoute] int id, [FromForm] string nome, [FromForm] string email, [FromForm] string telefone)
        {            
            ContextoDados contextoDados = new ContextoDados(true);
            IRepositorioFabricante repositorioFabricante = new RepositorioFabricanteEmArquivo(contextoDados);

            Fabricante fabricanteAtualizado = new Fabricante(nome, email, telefone);

            repositorioFabricante.EditarRegistro(id, fabricanteAtualizado);

            string conteudo = System.IO.File.ReadAllText("Compartilhado/Html/Notificacao.html");

            StringBuilder sb = new StringBuilder(conteudo);

            sb.Replace("#mensagem#", $"O registro foi \"{fabricanteAtualizado.Nome}\" foi editado com sucesso!");

            string conteudoString = sb.ToString();

            return Content(conteudoString, "text/html");
        }

        [HttpGet("excluir/{id:int}")]        
        public IActionResult ExibirFormularioExclusaoFabricante([FromRoute] int id)
        {
            ContextoDados contextoDados = new ContextoDados(true);
            IRepositorioFabricante repositorioFabricante = new RepositorioFabricanteEmArquivo(contextoDados);            

            Fabricante fabricanteSelecionado = repositorioFabricante.SelecionarRegistroPorId(id);

            string conteudo = System.IO.File.ReadAllText("ModuloFabricante/Html/Excluir.html");

            StringBuilder sb = new StringBuilder(conteudo);

            sb.Replace("#id#", id.ToString());
            sb.Replace("#fabricante#", fabricanteSelecionado.Nome);

            string conteudoString = sb.ToString();

            return Content(conteudoString, "text/html");
        }

        [HttpPost("excluir/{id:int}")]
        public IActionResult ExcluirFabricante([FromRoute] int id)
        {           
            ContextoDados contextoDados = new ContextoDados(true);
            IRepositorioFabricante repositorioFabricante = new RepositorioFabricanteEmArquivo(contextoDados);

            repositorioFabricante.ExcluirRegistro(id);

            string conteudo = System.IO.File.ReadAllText("Compartilhado/Html/Notificacao.html");

            StringBuilder sb = new StringBuilder(conteudo);

            sb.Replace("#mensagem#", $"O registro foi excluído com sucesso!");

            string conteudoString = sb.ToString();

            return Content(conteudoString, "text/html");
        }

        [HttpGet("visualizar")]
        public IActionResult VisualizarFabricantes()
        {
            ContextoDados contextoDados = new ContextoDados(true);
            IRepositorioFabricante repositorioFabricante = new RepositorioFabricanteEmArquivo(contextoDados);

            string conteudo = System.IO.File.ReadAllText("ModuloFabricante/Html/Visualizar.html");

            StringBuilder stringBuilder = new StringBuilder(conteudo);

            foreach (Fabricante f in repositorioFabricante.SelecionarRegistros())
            {
                string itemLista = $"<li>{f.ToString()} <a href=\"/fabricantes/editar/{f.Id}\">Editar</a> / <a href=\"/fabricantes/excluir/{f.Id}\">Excluir</a>  </li> #fabricante#";

                stringBuilder.Replace("#fabricante#", itemLista);
            }

            stringBuilder.Replace("#fabricante#", "");

            string conteudoString = stringBuilder.ToString();

            return Content(conteudoString, "text/html");
        }
    }
}
