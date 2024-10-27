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
        private readonly ILogger<PublishersController> _logger;

        public PublishersController(IPublisherService publisherService, IBookService bookService, IValidator<Publisher> validator, ILogger<PublishersController> logger)
        {
            _bookService = bookService;
            _publisherService = publisherService;
            _validator = validator;
            _logger = logger;
        }

        public async Task<IActionResult> Index()
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Details(int id). PublishersController.");
            _logger.LogInformation($"Exit method Task<IActionResult> Index(). PublishersController.");

            return View(await _publisherService.GetAllPublishersAsync());            
        }

        public async Task<IActionResult> Details(int id)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Details(int id). PublishersController. ID is {id}");

            var publisher = await _publisherService.GetPublisherByIdAsync(id);

            _logger.LogInformation($"Exit method Task<IActionResult> Index(). PublishersController. ID is {id}");

            return View(publisher);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]        
        public async Task<IActionResult> Create(Publisher publisher)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Details(int id). PublishersController. ID is {publisher.Id}");

            var publisherForValidate = await GetPublisherWithAllPropertyAsync(publisher);
            var result = await _validator.ValidateAsync(publisherForValidate);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Error in the method Task<IActionResult> Create(Publisher publisher). PublisherController. Publisher ID is {publisherForValidate.Id}. Error is {error.PropertyName}, {error.ErrorMessage}");

                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(publisher);
            }

            else                
            {
                await _publisherService.AddPublisherAsync(publisherForValidate);

                _logger.LogInformation($"Added new Publisher to database Publishers. PublisherController. Publisher ID is {publisherForValidate.Id}");
                _logger.LogInformation($"Exit method Task<IActionResult> Index(). PublishersController. ID is {publisherForValidate.Id}");

                return RedirectToAction(nameof(Index));
            }            
        }

        public async Task<IActionResult> Edit(int id)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Edit(int id). PublishersController. ID is {id}");

            var publisher = await _publisherService.GetPublisherByIdAsync(id);

            _logger.LogInformation($"Exit method Task<IActionResult> Edit(int id). PublishersController. ID is {id}");

            return View(publisher);
        }

        [HttpPost]        
        public async Task<IActionResult> Edit(int id, Publisher publisher)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Edit(int id, Publisher publisher). PublishersController. ID is {id}");

            var publisherForValidate = await GetPublisherWithAllPropertyAsync(publisher);
            var result = await _validator.ValidateAsync(publisherForValidate);

            if (!result.IsValid)
            {
                foreach (var error in result.Errors)
                {
                    _logger.LogError($"Error in the method Task<IActionResult> Edit(int id, Publisher publisher). PublisherController. Publisher ID is {publisherForValidate.Id}. Error is {error.PropertyName}, {error.ErrorMessage}");

                    ModelState.AddModelError(error.PropertyName, error.ErrorMessage);
                }

                return View(publisher);
            }

            else
            {
                await _publisherService.UpdatePublisherAsync(
                    new Publisher
                    {
                        Id = id,
                        Name = publisher.Name,
                        Address = publisher.Address,
                        Books = publisherForValidate.Books,
                    });

                _logger.LogInformation($"Updated Publisher. PublisherController. Publisher ID is {publisherForValidate.Id}");
                _logger.LogInformation($"Exit method Task<IActionResult> Edit(int id, Publisher publisher). PublishersController. ID is {publisherForValidate.Id}");

                return RedirectToAction(nameof(Index));
            }            
        }

        public async Task<IActionResult> Delete(int id)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> Delete(int id). PublishersController. ID is {id}");

            var publisher = await _publisherService.GetPublisherByIdAsync(id);

            _logger.LogInformation($"Exit method Task<IActionResult> Delete(int id). PublishersController. ID is {id}");

            return View(publisher);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            _logger.LogInformation($"Enter method Task<IActionResult> DeleteConfirmed(int id). PublishersController. ID is {id}");

            var publisher = await _publisherService.GetPublisherByIdAsync(id);

            if (publisher != null)
            {
                _logger.LogInformation($"Publisher ID {id} deleted from database Publishers");

                await _publisherService.DeletePublisherAsync(id);
            }

            _logger.LogInformation($"Exit method Task<IActionResult> DeleteConfirmed(int id). PublishersController. ID is {id}");

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> GetPublisherBooks(string publisherName)
        {
            var allPublishers = await _publisherService.GetAllPublishersAsync();
            var publisherWithName = allPublishers.FirstOrDefault(p => p.Name == publisherName);

            if (publisherWithName != null)
            {
                var allBooks = await _bookService.GetAllBooksAsync();
                var booksOfThisPublisher = allBooks.Where(b => b.PublisherId == publisherWithName.Id).ToList();

                return View(booksOfThisPublisher);
            }

            else
            {
                return Content($"Publisher with name {publisherName} not found");
            }
        }

        public async Task<IActionResult> GetBookByNameFromPublisherName (string publisherName, string bookName)
        {
            var allPublishers = await _publisherService.GetAllPublishersAsync();
            var publisherWithName = allPublishers.FirstOrDefault(p => p.Name == publisherName);

            if (publisherWithName != null)
            {
                var allBooks = await _bookService.GetAllBooksAsync();
                var booksOfThisPublisher = allBooks.Where(b => b.PublisherId == publisherWithName.Id);
                var bookOfThisPublisherWithBookName = booksOfThisPublisher.FirstOrDefault(b => b.Name == bookName);

                return View(bookOfThisPublisherWithBookName);
            }

            else
            {
                return Content($"Book with name {bookName} not found in the publisher {publisherName}");
            }
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
