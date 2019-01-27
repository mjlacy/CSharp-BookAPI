using System.Collections.Generic;
using MongoDB.Driver;
using CSharp_BookAPI.Models;

namespace CSharp_BookAPI.DataServices
{
    public interface IBookDataFactory
    {
        Books GetBooks();

        Book GetBook(string _id);

        string CreateBook(Book book);

        ReplaceOneResult UpsertBook(string _id, Book book);

        UpdateResult UpdateBook(string _id, object book);

        bool DeleteBook(string _id);
    }
}
