using System;
using System.Collections.Generic;
using CSharp_BookAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using System.Linq;
using System.Threading.Tasks;

namespace CSharp_BookAPI.DataServices
{
    public class BookDataFactorySimple : IBookDataFactory
    {
        static IMongoClient client = new MongoClient("mongodb://localhost:27017");
        static IMongoDatabase database = client.GetDatabase("books");
        IMongoCollection<Book> collection = database.GetCollection<Book>("books");

        public Books GetBooks()
        {
            List<Book> booksList = collection.AsQueryable().ToList();

            Books books = new Books(booksList);

            return books;
        }

        public Book GetBook(string _id)
        {
            var book = collection.Find(new BsonDocument { { "_id", ObjectId.Parse(_id) } });
            if(book.CountDocuments() > 0){
                return book.Single();
            }
            else {
                return new Book();
            }
        }

        public Dictionary<string, string> CreateBook(Book book)
        {
            ObjectId newId = ObjectId.GenerateNewId();
            book._id = newId;
            collection.InsertOne(book);
            return new Dictionary<string, string>(){
                { "link", $"/{newId}" },
            };
        }

        public Dictionary<string, string> UpdateBook(string _id, Book book)
        {
            book._id = ObjectId.Parse(_id);
            ReplaceOneResult result = collection.ReplaceOne(new BsonDocument{{ "_id", ObjectId.Parse(_id) }}, book, new UpdateOptions { IsUpsert = true });
            if (result.ModifiedCount > 0) {
                return new Dictionary<string, string>(){
                    { "link", $"/{_id}" },
                };
            }
            return new Dictionary<string, string>(){
                { "link", $"/{result.UpsertedId}" },
            };
        }

        public bool DeleteBook(string _id){
            if (GetBook(_id)._id == null) {
                return false;
            }

            DeleteResult result = collection.DeleteOne(new BsonDocument { { "_id", ObjectId.Parse(_id) } });
            return true;
        }
    }
}
