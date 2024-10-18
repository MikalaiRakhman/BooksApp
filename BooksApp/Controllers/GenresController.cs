using Microsoft.AspNetCore.Mvc;
using BookApp.Domain.Entity;
using BookApp.BLL.Interfaces;
using BooksApp.WEB.Models;

namespace BooksApp.WEB.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly IBookService _bookService;

        public GenresController(IGenreService genreService, IBookService bookService)
        {
            _genreService = genreService;
            _bookService = bookService;
        }
        
        public async Task<IActionResult> Index()
        {
            return View(await _genreService.GetAllGenresAsync());            
        }
        
        public IActionResult Create()
        {
            return View();
        }
              
        [HttpPost]        
        public async Task<IActionResult> Create(GenreViewModel genre)
        {
            if (ModelState.IsValid)
            {                
                await _genreService.AddGenreAsync(new Genre 
                {
                    Name = genre.Name,
                    Books = new List<Book>()  
                });

                return RedirectToAction(nameof(Index));
            }

            return View(genre);
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _genreService.GetGenreByIdAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }
                
        [HttpPost]        
        public async Task<IActionResult> Edit(int id, GenreViewModel genre)
        {
            if (id != genre.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var book = await _genreService.GetGenreByIdAsync(id);

                var genreEdited = new Genre 
                {
                    Id = id,
                    Name = genre.Name,
                    Books = book.Books,
                };

                await _genreService.UpdateGenreAsync(genreEdited);

                return RedirectToAction(nameof(Index));
            }

            return View(genre);
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var genre = await _genreService.GetGenreByIdAsync(id);

            if (genre == null)
            {
                return NotFound();
            }

            return View(genre);
        }
        
        [HttpPost, ActionName("Delete")]        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var genre = await _genreService.GetGenreByIdAsync(id);

            if (genre != null)
            {
               await _genreService.DeleteGenreAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
