﻿using BookApp.Domain.Entity;
using BookApp.DAL.Repository.Interfaces;
using BookApp.BLL.Interfaces;

namespace BookApp.BLL.Services
{
    public class BookService : IBookService
    {
        private readonly IBookRepository _bookRepository;

        public BookService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public async Task<IEnumerable<Book>> GetAllBooksAsync()
        {
            return await _bookRepository.GetAllAsync();
        }

        public async Task<Book> GetBookByIdAsync(int id)
        {
            return await _bookRepository.GetByIdAsync(id);
        }

        public async Task AddBookAsync(Book book)
        {
            await _bookRepository.AddAsync(book);
        }

        public async Task UpdateBookAsync(Book book)
        {
            await _bookRepository.UpdateAsync(book);
        }

        public async Task DeleteBookAsync(int id)
        {
            await _bookRepository.DeleteAsync(id);
        }
    }
}
