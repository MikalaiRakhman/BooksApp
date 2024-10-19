using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using BookApp.DAL.Data;
using BookApp.Domain.Entity;
using BookApp.BLL.Interfaces;
using BookApp.WEB.Models;

namespace BookApp.WEB.Controllers
{
    public class PublishersController : Controller
    {
        private readonly IPublisherService _publisherService;
        private readonly IBookService _bookService;

        public PublishersController(IPublisherService publisherService, IBookService bookService)
        {
           _bookService = bookService;
            _publisherService = publisherService;
        }

        // GET: Publishers
        public async Task<IActionResult> Index()
        {
            return View(await _publisherService.GetAllPublishersAsync());
        }

        // GET: Publishers/Details/5
        public async Task<IActionResult> Details(int id)
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

        // GET: Publishers/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Publishers/Create
        [HttpPost]        
        public async Task<IActionResult> Create(PublisherViewModel publisher)
        {
            if (ModelState.IsValid)
            {
                await _publisherService.AddPublisherAsync(
                    new Publisher 
                    {
                        Name = publisher.Name,
                        Address = publisher.Address
                    });
                
                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        // GET: Publishers/Edit/5
        public async Task<IActionResult> Edit(int id)
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

        [HttpPost]        
        public async Task<IActionResult> Edit(int id, PublisherViewModel publisher)
        {
            if (id != publisher.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var books = await _bookService.GetAllBooksAsync();
                var publisherBooks = books.Where(b => b.PublisherId == id).ToList();

                await _publisherService.UpdatePublisherAsync(
                    new Publisher 
                    {
                        Id = id,
                        Name = publisher.Name,
                        Address = publisher.Address,
                        Books = publisherBooks
                    });

                return RedirectToAction(nameof(Index));
            }
            return View(publisher);
        }

        // GET: Publishers/Delete/5
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

        // POST: Publishers/Delete/5
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
    }
}
