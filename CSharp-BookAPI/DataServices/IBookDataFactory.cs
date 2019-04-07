using System.Collections.Generic;
using MongoDB.Driver;
using CSharp_BookAPI.Models;

namespace CSharp_BookAPI.DataServices
{
    public interface IBookDataFactory
    {
        bool Health();

        Books GetBooks(Book book);

        Book GetBook(string _id);

        Book CreateBook(Book book);

        ReplaceOneResult UpsertBook(string _id, Book book);

        UpdateResult UpdateBook(string _id, object book);

        DeleteResult DeleteBook(string _id);
    }
}
