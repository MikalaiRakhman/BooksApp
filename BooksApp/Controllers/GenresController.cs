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
        private readonly ILogger<GenresController> _logger;

        public GenresController(IGenreService genreService, IBookService bookService, IValidator<Genre> validator, ILogger<GenresController> logger)
        {
            _genreService = genreService;
            _bookService = bookService;
            _validator = validator;
            _logger = logger;
        }
        
        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Index(). GenresController.");
            _logger.LogInformation($"Exit method Task<IActionResult> Index(). GenresController.");

            return View(await _genreService.GetAllGenresAsync());            
        }
        
        public IActionResult Create()
        {
            _logger.LogInformation($"Enter method IActionResult Create(). GenresController.");
            _logger.LogInformation($"Exit method IActionResult Create(). GenresController.");

            return View();            
        }
              
        [HttpPost]        
        public async Task<IActionResult> Create(Genre genre)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Create(Genre genre). GenresController. Genre ID is {genre.Id}");

            var genreToValidate = await GetGenreWithAllPropertyAsync(genre);
            var result = await _validator.ValidateAsync(genreToValidate);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Error in the method Task<IActionResult> Create(Genre genre). GenresController. Genre ID is {genre.Id}. Error is {error.PropertyName}, {error.ErrorMessage}");

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

                _logger.LogInformation($"Added new Genre to database Genres. GenresController. Genre ID is {genreToValidate.Id}");
                _logger.LogInformation($"Exit method Task<IActionResult> Create(Genre genre). GenresController. Genre ID is {genre.Id}");

                return RedirectToAction(nameof(Index));
            }            
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Edit(int id). GenresController. Genre ID is {id}");

            var genre = await _genreService.GetGenreByIdAsync(id);

            _logger.LogInformation($"Exit method Task<IActionResult> Edit(int id). GenresController. Genre ID is {id}");

            return View(genre);
        }
                
        [HttpPost]        
        public async Task<IActionResult> Edit(int id, Genre genre)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Edit(int id, Genre genre). GenresController. Genre ID is {id}");

            var genreToValidate = await GetGenreWithAllPropertyAsync(genre);
            var result = await _validator.ValidateAsync(genreToValidate);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Error in the method Task<IActionResult> Create(Genre genre). GenresController. Genre ID is {genre.Id}. Error is {error.PropertyName}, {error.ErrorMessage}");

                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(genre);
            }

            else 
            {
                await _genreService.UpdateGenreAsync(
                    new Genre 
                    {
                        Id = id,
                        Name = genre.Name,
                        Books = genreToValidate.Books
                    });

                _logger.LogInformation($"Apdated Genre. GenresController. Genre ID is {id}");
                _logger.LogInformation($"Exit method Task<IActionResult> Edit(int id, Genre genre). GenresController. Genre ID is {id}");

                return RedirectToAction(nameof(Index));
            }
        }
        
        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Delete(int id). GenresController. Genre ID is {id}");

            var genre = await _genreService.GetGenreByIdAsync(id);

            _logger.LogInformation($"Exit method Task<IActionResult> Delete(int id). GenresController. Genre ID is {id}");

            return View(genre);
        }
        
        [HttpPost, ActionName("Delete")]        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> DeleteConfirmed(int id). GenresController. Genre ID is {id}");

            var genre = await _genreService.GetGenreByIdAsync(id);

            if (genre != null)
            {
               await _genreService.DeleteGenreAsync(id);
            }

            _logger.LogInformation($"Deleted Genre. GenresController. Genre ID is {id}");
            _logger.LogInformation($"Exit method Task<IActionResult> Delete(int id). GenresController. Genre ID is {id}");

            return RedirectToAction(nameof(Index));
        }

        private async Task<Genre> GetGenreWithAllPropertyAsync(Genre genre)
        {
            var booksFromDatabase = await _bookService.GetAllBooksAsync();
            var thisGenreBooks = booksFromDatabase.Where(b => b.GenreId == genre.Id);

            var genreForView = new Genre();

            if (thisGenreBooks != null)
            {
                genreForView = new Genre
                {
                    Name = genre.Name,
                    Books = thisGenreBooks.ToList()
                };
            }

            else
            {
                genreForView = new Genre
                {
                    Name = genre.Name,
                    Books = new List<Book>()
                };
            }

            return genreForView;
        }
    }
}
