using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;

namespace Biblioteca.Controllers
{
    public class LivroController : Controller
    {
        private BibliotecaDbContext _contexto { get; set; }

        public LivroController(BibliotecaDbContext contexto)
        {
            _contexto = contexto;
        }

        // Ação para listar os livros
        public ActionResult Listar()
        {
            var TodosOsLivros = _contexto.Livros
                .Include(p => p.LivroEditora)
                .Include(p => p.AutoresDoLivro).ThenInclude(p => p.Autor);

            ViewData["Title"] = "Lista de Livros";
            ViewBag.dados = TodosOsLivros;

            return View();
        }

        // Ação para mostrar os detalhes do livro
        public ActionResult Detalhar(int? id)
        {
            if (id == null)
            {
                return NotFound(); // Retorna 404 caso o ID seja nulo
            }

            // Busca o livro pelo ID, incluindo editora e autores
            var livro = _contexto.Livros
                .Include(p => p.LivroEditora)
                .Include(p => p.AutoresDoLivro)
                    .ThenInclude(p => p.Autor)
                .FirstOrDefault(l => l.LivroID == id);

            // Verifica se o livro foi encontrado
            if (livro == null)
            {
                return NotFound(); // Retorna 404 se o livro não for encontrado
            }

            return View(livro); // Passa o livro para a view
        }

        // Ação para Excluir o livro
        [HttpPost]
        [ActionName("Excluir")]
        public ActionResult ExcluirConfirmado(int id)
        {
            // Busca o livro pelo ID
            var livro = _contexto.Livros.FirstOrDefault(l => l.LivroID == id);

            // Verifica se o livro foi encontrado
            if (livro == null)
            {
                return NotFound(); // Se não encontrar o livro, retorna 404
            }

            // Remove o livro do contexto
            _contexto.Livros.Remove(livro);

            // Salva as alterações no banco de dados
            _contexto.SaveChanges();

            // Redireciona para a lista de livros após a exclusão
            return RedirectToAction("Listar");
        }
    }
}
