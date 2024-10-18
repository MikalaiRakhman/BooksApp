using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BookApp.Domain.Entity;
using BookApp.BLL.Interfaces;
using BooksApp.WEB.Models;

namespace BooksApp.WEB.Controllers
{
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;
        private readonly IGenreService _genreService;
        private readonly IPublisherService _publisherService;

        public BooksController(IPublisherService publisherService, IGenreService genreService, IBookService bookService)
        {
            _bookService = bookService;
            _publisherService = publisherService;
            _genreService = genreService;
        }
        
        public async Task<IActionResult> Index()
        {
            var booksFromBooksDatabase = await _bookService.GetAllBooksAsync();
            var booksToViews = new List<Book>();

            foreach (var book in booksFromBooksDatabase)
            {
                booksToViews.Add(new Book
                {
                    Id = book.Id,
                    Name = book.Name,
                    Author = book.Author,
                    Year = book.Year,
                    PublisherId = book.PublisherId,
                    Publisher = await _publisherService.GetPublisherByIdAsync(book.PublisherId),
                    GenreId = book.GenreId,
                    Genre = await _genreService.GetGenreByIdAsync(book.GenreId),
                });
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

            var bookToView = new Book 
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
        public async Task<IActionResult> Create([FromForm] BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                await _bookService.AddBookAsync(new Book
                {
                    Name = book.Name,
                    Author = book.Author,
                    Year = book.Year,
                    PublisherId = book.PublisherId,
                    Publisher = await _publisherService.GetPublisherByIdAsync(book.PublisherId),
                    GenreId = book.GenreId,
                    Genre = await _genreService.GetGenreByIdAsync(book.GenreId),
                });
                
                return RedirectToAction(nameof(Index));
            }

            ViewData["GenreId"] = new SelectList(await _genreService.GetAllGenresAsync(), "Id", "Name", book.GenreId);
            ViewData["PublisherId"] = new SelectList(await _publisherService.GetAllPublishersAsync(), "Id", "Address", book.PublisherId);

            return View(book);
        }
        
        public async Task<IActionResult> Edit(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

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
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Book book)
        {
            if (id != book.Id)
            {
                return NotFound();
            }

            bool modelIsValid =
                book.Name != null
                && book.Name.Length > 0
                && book.Author != null
                && book.Author.Length > 0
                && book.Year != null
                && book.PublisherId != null
                && book.GenreId != null;

            if (modelIsValid)
            {
                var bookToUpdate = await _bookService.GetBookByIdAsync(id);
                if (bookToUpdate == null)
                {
                    return NotFound();
                }

                bookToUpdate.Name = book.Name;
                bookToUpdate.Author = book.Author;
                bookToUpdate.Year = book.Year;
                bookToUpdate.PublisherId = book.PublisherId;
                bookToUpdate.GenreId = book.GenreId;

                await _bookService.UpdateBookAsync(bookToUpdate);
                return RedirectToAction(nameof(Index));
            }

            ViewBag.GenreId = new SelectList(await _genreService.GetAllGenresAsync(), "Id", "Name", book.GenreId);
            ViewBag.PublisherId = new SelectList(await _publisherService.GetAllPublishersAsync(), "Id", "Name", book.PublisherId);

            return View(book);
        }

        public async Task<IActionResult> Delete(int id)
        {
            var book = await _bookService.GetBookByIdAsync(id);

            if (book == null)
            {
                return NotFound();
            }

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
    }
}
