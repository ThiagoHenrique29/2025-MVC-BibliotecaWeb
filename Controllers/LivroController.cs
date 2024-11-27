using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Biblioteca.Models;
using System.Linq;

namespace Biblioteca.Controllers
{
    public class LivroController : Controller
    {
        private BibliotecaDbContext _contexto { get; set; }

        public LivroController(BibliotecaDbContext contexto)
        {
            _contexto = contexto;
        }

        // Ação para listar os livros, agora com suporte a pesquisa
        public ActionResult Listar(string search)
        {
            var livros = _contexto.Livros
                .Include(p => p.LivroEditora)
                .Include(p => p.AutoresDoLivro).ThenInclude(p => p.Autor)
                .AsQueryable();

            // Se houver algo no campo de busca, filtra os livros pelo título
            if (!string.IsNullOrEmpty(search))
            {
                livros = livros.Where(l => l.LivroTituloOriginal.Contains(search));
            }

            // Passa o valor da pesquisa de volta para a View
            ViewBag.SearchTerm = search;
    
            ViewData["Title"] = "Lista de Livros";
            ViewBag.dados = livros.ToList();

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
    }
}