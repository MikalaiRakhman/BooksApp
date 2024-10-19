using Microsoft.AspNetCore.Mvc;
using BookApp.Domain.Entity;
using BookApp.BLL.Interfaces;
using FluentValidation;

namespace BookApp.WEB.Controllers
{
    public class PublishersController : Controller
    {
        private readonly IPublisherService _publisherService;
        private readonly IBookService _bookService;
        private readonly IValidator<Publisher> _validator;

        public PublishersController(IPublisherService publisherService, IBookService bookService, IValidator<Publisher> validator)
        {
           _bookService = bookService;
            _publisherService = publisherService;
            _validator = validator;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _publisherService.GetAllPublishersAsync());
        }

        public async Task<IActionResult> Details(int id)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id);
            
            return View(publisher);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]        
        public async Task<IActionResult> Create(Publisher publisher)
        {
            var publisherForValidate = await GetPublisherWithAllPropertyAsync(publisher);
            var result = await _validator.ValidateAsync(publisherForValidate);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(publisher);
            }

            else                
            {
                await _publisherService.AddPublisherAsync(publisherForValidate);               
                
                return RedirectToAction(nameof(Index));
            }            
        }

        public async Task<IActionResult> Edit(int id)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id);

            return View(publisher);
        }

        [HttpPost]        
        public async Task<IActionResult> Edit(int id, Publisher publisher)
        {
            var publisherForValidate = await GetPublisherWithAllPropertyAsync(publisher);
            var result = await _validator.ValidateAsync(publisherForValidate);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(publisher);
            }

            else
            {
                var books = await _bookService.GetAllBooksAsync();                

                await _publisherService.UpdatePublisherAsync(
                    new Publisher
                    {
                        Id = id,
                        Name = publisher.Name,
                        Address = publisher.Address,
                        Books = publisherForValidate.Books,
                    });

                return RedirectToAction(nameof(Index));
            }            
        }

        public async Task<IActionResult> Delete(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var publisher = await _publisherService.GetPublisherByIdAsync(id);

            if (publisher == null)
            {
                return NotFound();
            }

            return View(publisher);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var publisher = await _publisherService.GetPublisherByIdAsync(id);

            if (publisher != null)
            {
                await _publisherService.DeletePublisherAsync(id);
            }

            return RedirectToAction(nameof(Index));
        }

        private async Task<Publisher> GetPublisherWithAllPropertyAsync(Publisher publisher)
        {
            var booksFromDatabase = await _bookService.GetAllBooksAsync();
            var thisAuthorBooks = booksFromDatabase.Where(b => b.PublisherId == publisher.Id);

            var publisherForView = new Publisher();

            if (thisAuthorBooks != null) 
            {
                publisherForView = new Publisher
                {
                    Name = publisher.Name,
                    Address = publisher.Address,
                    Books = thisAuthorBooks.ToList(),
                };
            }

            else
            {
                publisherForView = new Publisher
                {
                    Name = publisher.Name,
                    Address = publisher.Address,
                    Books = new List<Book>(),
                };
            }

            return publisherForView;
        }
    }
}
