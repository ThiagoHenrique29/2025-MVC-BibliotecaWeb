using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Biblioteca.Controllers
{
    public class LivroController : Controller
    {
        private BibliotecaDbContext _contexto { get; set; }
        

        public LivroController(BibliotecaDbContext contexto)
        {
            _contexto = contexto;
        }
        
        public ActionResult Listar()
        {
            var TodosOsLivros = _contexto.Livros.
                Include(p=>p.LivroEditora).
                Include(p=>p.AutoresDoLivro).ThenInclude(p=>p.Autor);
            
            
            
            ViewData["Title"] = "Lista de Livros";
            ViewBag.dados = TodosOsLivros;
            
            return View();
        }

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
