namespace BooksApi.Controllers
{
    using System;
    using System.Collections.Generic;
    using BooksApi.Models;
    using BooksApi.Services;
    using Microsoft.AspNetCore.Mvc;

    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly BookService bookService;

        public BooksController(BookService bookService)
        {
            this.bookService = bookService ?? throw new ArgumentNullException(nameof(bookService));
        }

        [HttpGet]
        public ActionResult<List<Book>> Get()
        {
            return this.bookService.Get();
        }

        [HttpGet("{id:length(24)}", Name = "GetBook")]
        public ActionResult<Book> Get(string id)
        {
            var book = this.bookService.Get(id);

            return book ?? (ActionResult<Book>) this.NotFound();
        }

        [HttpPost]
        public ActionResult<Book> Create(Book book)
        {
            this.bookService.Create(book);

            return this.CreatedAtRoute("GetBook", new { id = book.Id.ToString() }, book);
        }

        [HttpPut("{id:length(24)}")]
        public IActionResult Update(string id, Book bookIn)
        {
            var book = this.bookService.Get(id);

            if (book == null)
            {
                return this.NotFound();
            }

            this.bookService.Update(id, bookIn);

            return this.NoContent();
        }

        [HttpDelete("{id:length(24)}")]
        public IActionResult Delete(string id)
        {
            var book = this.bookService.Get(id);

            if (book == null)
            {
                return this.NotFound();
            }

            this.bookService.Remove(book.Id);

            return this.NoContent();
        }
    }
}
