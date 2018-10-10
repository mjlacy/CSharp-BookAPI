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

        ReplaceOneResult UpdateBook(string _id, Book book);

        bool DeleteBook(string _id);
    }
}
