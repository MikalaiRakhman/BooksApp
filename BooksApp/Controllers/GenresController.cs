using Microsoft.AspNetCore.Mvc;
using BookApp.Domain.Entity;
using BookApp.BLL.Interfaces;
using FluentValidation;

namespace BooksApp.WEB.Controllers
{
    public class GenresController : Controller
    {
        private readonly IGenreService _genreService;
        private readonly IBookService _bookService;
        private readonly IValidator<Genre> _validator;

        public GenresController(IGenreService genreService, IBookService bookService, IValidator<Genre> validator)
        {
            _genreService = genreService;
            _bookService = bookService;
            _validator = validator;
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
        public async Task<IActionResult> Create(Genre genre)
        {
            var genreToValidate = new Genre 
            {
                Name = genre.Name,
                Books = new List<Book>()
            };

            var result = await _validator.ValidateAsync(genreToValidate);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(genre);
            }

            else
            {                
                await _genreService.AddGenreAsync(new Genre 
                {
                    Name = genre.Name,
                    Books = new List<Book>()  
                });

                return RedirectToAction(nameof(Index));
            }            
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            var genre = await _genreService.GetGenreByIdAsync(id);

            return View(genre);
        }
                
        [HttpPost]        
        public async Task<IActionResult> Edit(int id, Genre genre)
        {
            var booksList = await _bookService.GetAllBooksAsync();
            var booksWithThisGenreName = booksList.Where(b => b.GenreId == id);            

            var genreToValidate = new Genre();

            if (booksWithThisGenreName == null)
            {
                genreToValidate = new Genre
                {
                    Name = genre.Name,
                    Books = new List<Book>()
                };
            }

            else
            {
                genreToValidate = new Genre
                {
                    Name = genre.Name,
                    Books = booksWithThisGenreName.ToList()
                };
            }            

            var result = await _validator.ValidateAsync(genreToValidate);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(genre);
            }

            else 
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
        }
        
        public async Task<IActionResult> Delete(int id)
        {
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
