using BooksApp.WEB.Models;
using Microsoft.AspNetCore.Mvc;

namespace BooksApp.Controllers
{
    public class BooksController : Controller
    {
        private static List<BookViewModel> _bookViewModels = 
            [
            new BookViewModel {Id = 1, Name = "Johnny Mneminic", Author = "William Gibson", Year = 1981 },
            new BookViewModel {Id = 2, Name = "Dune", Author = "Frank Herbert", Year = 1963},
            new BookViewModel {Id = 3, Name = "The Witcher", Author = "Andrzej Sapkowski", Year = 1990}
            ];

        public IActionResult Index()
        {
            ViewBag.AnotherTitle = "List of books";       
            var bookViewModels = _bookViewModels;

            return View(bookViewModels);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]        
        public IActionResult Create([FromForm] BookViewModel book)
        {
            if (ModelState.IsValid)
            {
                book.Id = _bookViewModels.Count > 0 ? _bookViewModels.Max(s => s.Id) + 1 : 1;
                _bookViewModels.Add(book);
                return RedirectToAction(nameof(Index));
            }

            return View(book);
        }

        public IActionResult Edit(int id)
        {
            var book = _bookViewModels.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost]
        public IActionResult Edit(BookViewModel book)
        {
            var existingBook = _bookViewModels.FirstOrDefault(b => b.Id == book.Id);
            if (existingBook == null)
            {
                return NotFound();
            }

            existingBook.Name = book.Name;
            existingBook.Author = book.Author;
            existingBook.Year = book.Year;

            return RedirectToAction(nameof(Index));
        }

        public IActionResult Delete(int id)
        {
            var book = _bookViewModels.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }
            return View(book);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeleteConfirmed(int id)
        {
            var book = _bookViewModels.FirstOrDefault(b => b.Id == id);
            if (book == null)
            {
                return NotFound();
            }

            _bookViewModels.Remove(book);
            return RedirectToAction(nameof(Index));
        }
    }
}
