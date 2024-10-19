using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookApp.Domain.Entity;
using BookApp.BLL.Interfaces;
using FluentValidation;

namespace BooksApp.WEB.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;
        private readonly IPublisherService _publisherService;
        private readonly IValidator<Book> _validator;

        public BooksController(IPublisherService publisherService, IGenreService genreService, IBookService bookService, IValidator<Book> validator)
        {
            _bookService = bookService;
            _publisherService = publisherService;
            _genreService = genreService;
            _validator = validator;
        }
        
        public async Task<IActionResult> Index()
        {
            var booksFromBooksDatabase = await _bookService.GetAllBooksAsync();
            var booksToViews = new List<Book>();

            foreach (var book in booksFromBooksDatabase)
            {
                booksToViews.Add(await GetBookWithAllPropertysAsync(book));
            }

            return View(booksToViews);
        }
        
        public async Task<IActionResult> Details(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var book = await _bookService.GetBookByIdAsync(id);
            var bookToView = GetBookWithAllPropertysAsync(book);

            if (book == null)
            {
                return NotFound();
            }

            return View(book);
        }
        
        public async Task<IActionResult> Create()
        {
            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenresAsync(), "Id", "Name");
            ViewData["PublisherId"] = new SelectList(await _publisherService.GetAllPublishersAsync(), "Id", "Name");

            return View();
        }
   
        [HttpPost]        
        public async Task<IActionResult> Create(Book book)
        {
            var bookForValidate = await GetBookWithAllPropertysAsync(book);
            var result = await _validator.ValidateAsync(bookForValidate);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenresAsync(), "Id", "Name", book.GenreId);
                ViewData["PublisherId"] = new SelectList(await _publisherService.GetAllPublishersAsync(), "Id", "Address", book.PublisherId);

                return View(book);
            }

            else
            {
                await _bookService.AddBookAsync(bookForValidate);

                return RedirectToAction(nameof(Index));
            }
        }
        
        public async Task<IActionResult> Edit(int id)
        {            
            var book = await _bookService.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenresAsync(), "Id", "Name", book.GenreId);
            ViewData["PublisherId"] = new SelectList(await _publisherService.GetAllPublishersAsync(), "Id", "Name", book.PublisherId);

            return View(book);
        }

        [HttpPost]        
        public async Task<IActionResult> Edit(int id, Book book)
        {
            var bookForValidate = await GetBookWithAllPropertysAsync(book);
            var result = await _validator.ValidateAsync(bookForValidate);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenresAsync(), "Id", "Name", book.GenreId);
                ViewData["PublisherId"] = new SelectList(await _publisherService.GetAllPublishersAsync(), "Id", "Address", book.PublisherId);

                return View(book);
            }

            else                    
            {
                await _bookService.UpdateBookAsync(bookForValidate);
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

            var bookForView = await GetBookWithAllPropertysAsync(book);

            return View(book);
        }

        [HttpPost, ActionName("Delete")]        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if (book != null)
            {
                await _bookService.DeleteBookAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<Book> GetBookWithAllPropertysAsync(Book book)
        {
            var bookForView = new Book
            {
                Id = book.Id,
                Name = book.Name,
                Author = book.Author,
                Year = book.Year,
                PublisherId = book.PublisherId,
                Publisher = await _publisherService.GetPublisherByIdAsync(book.PublisherId),
                GenreId = book.GenreId,
                Genre = await _genreService.GetGenreByIdAsync(book.GenreId),
            };

            return bookForView;
        }
    }
}
