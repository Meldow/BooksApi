namespace BooksApi.Services
{
    using System.Collections.Generic;
    using BooksApi.Models;
    using Microsoft.Extensions.Configuration;
    using MongoDB.Bson;
    using MongoDB.Driver;

    public class BookService
    {
        private readonly IMongoCollection<Book> books;

        public BookService(IConfiguration config)
        {
            var client = new MongoClient(config.GetConnectionString("BookstoreDb"));
            var database = client.GetDatabase("BookstoreDb");
            this.books = database.GetCollection<Book>("Books");
        }

        public List<Book> Get()
        {
            return this.books.Find(book => true).ToList();
        }

        public Book Get(string id)
        {
            var docId = new ObjectId(id);

            return this.books.Find<Book>(book => book.Id == docId).FirstOrDefault();
        }

        public Book Create(Book book)
        {
            this.books.InsertOne(book);
            return book;
        }

        public void Update(string id, Book bookIn)
        {
            var docId = new ObjectId(id);

            this.books.ReplaceOne(book => book.Id == docId, bookIn);
        }

        public void Remove(Book bookIn)
        {
            this.books.DeleteOne(book => book.Id == bookIn.Id);
        }

        public void Remove(ObjectId id)
        {
            this.books.DeleteOne(book => book.Id == id);
        }
    }
}
