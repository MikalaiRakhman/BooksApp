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
        private readonly ILogger<BooksController> _logger;

        public BooksController(IPublisherService publisherService, IGenreService genreService, IBookService bookService, IValidator<Book> validator, ILogger<BooksController> logger)
        {
            _bookService = bookService;
            _publisherService = publisherService;
            _genreService = genreService;
            _validator = validator;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Index(). BooksController.");

            var booksFromBooksDatabase = await _bookService.GetAllBooksAsync();
            var booksToViews = new List<Book>();

            foreach (var book in booksFromBooksDatabase)
            {
                booksToViews.Add(await GetBookWithAllPropertysAsync(book));
            }

            _logger.LogInformation("Exit method Task<IActionResult> Index()");

            return View(booksToViews);
        }
        
        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Details(int id). BooksController. Number ID is {id}");

            var book = await _bookService.GetBookByIdAsync(id);
            var bookToView = GetBookWithAllPropertysAsync(book);

            _logger.LogInformation($"Exit method Task<IActionResult> Details(int id). BooksController. Number ID is {id}");

            return View(book);
        }
        
        public async Task<IActionResult> Create()
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Create(). BooksController.");

            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenresAsync(), "Id", "Name");
            ViewData["PublisherId"] = new SelectList(await _publisherService.GetAllPublishersAsync(), "Id", "Name");

            _logger.LogInformation($"Exit method Task<IActionResult> Create(). BooksController.");

            return View();
        }
   
        [HttpPost]        
        public async Task<IActionResult> Create(Book book)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Create(Book book). BooksController. Book ID is {book.Id}");

            var bookForValidate = await GetBookWithAllPropertysAsync(book);
            var result = await _validator.ValidateAsync(bookForValidate);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Error in the method Task<IActionResult> Create(Book book). BooksController. Book ID is {book.Id}. Error is {error.PropertyName}, {error.ErrorMessage}");

                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenresAsync(), "Id", "Name", book.GenreId);
                ViewData["PublisherId"] = new SelectList(await _publisherService.GetAllPublishersAsync(), "Id", "Address", book.PublisherId);                

                return View(book);
            }

            else
            {
                await _bookService.AddBookAsync(bookForValidate);

                _logger.LogInformation($"Added new book to database Books. BooksController. Book ID is {bookForValidate.Id}");
                _logger.LogInformation($"Exit method Task<IActionResult> Create(Book book). BooksController. Book ID is {book.Id}");

                return RedirectToAction(nameof(Index));
            }
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Edit(int id). BooksController. Book ID is {id}");

            var book = await _bookService.GetBookByIdAsync(id);

            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenresAsync(), "Id", "Name", book.GenreId);
            ViewData["PublisherId"] = new SelectList(await _publisherService.GetAllPublishersAsync(), "Id", "Name", book.PublisherId);

            _logger.LogInformation($"Exit method Task<IActionResult> Edit(int id). BooksController. Book ID is {id}");

            return View(book);
        }

        [HttpPost]        
        public async Task<IActionResult> Edit(int id, Book book)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Edit(int id, Book book). BooksController. Book ID is {id}");

            var bookForValidate = await GetBookWithAllPropertysAsync(book);
            var result = await _validator.ValidateAsync(bookForValidate);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Error in the method Task<IActionResult> Edit(int id, Book book). BooksController. Book ID is {id}. Error is {error.PropertyName}, {error.ErrorMessage}");

                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenresAsync(), "Id", "Name", book.GenreId);
                ViewData["PublisherId"] = new SelectList(await _publisherService.GetAllPublishersAsync(), "Id", "Address", book.PublisherId);               

                return View(book);
            }

            else                    
            {
                await _bookService.UpdateBookAsync(bookForValidate);

                _logger.LogInformation($"Book updated. BooksController. Book ID is {bookForValidate.Id}");
                _logger.LogInformation($"Exit method Task<IActionResult> Create(Book book). BooksController. Book ID is {id}");

                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Delete(int id). BooksController. Book ID is {id}");

            var book = await _bookService.GetBookByIdAsync(id);

            var bookForView = await GetBookWithAllPropertysAsync(book);

            _logger.LogInformation($"Exit method Task<IActionResult> Delete(int id). BooksController. Book ID is {id}");

            return View(book);
        }

        [HttpPost, ActionName("Delete")]        
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> DeleteConfirmed(int id). BooksController. Book ID is {id}");

            var book = await _bookService.GetBookByIdAsync(id);

            if (book != null)
            {
                await _bookService.DeleteBookAsync(id);

                _logger.LogInformation($"Book ID {id} deleted from database Books");
            }

            _logger.LogInformation($"Exit method Task<IActionResult> DeleteConfirmed(int id). BooksController. Book ID is {id}");

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
