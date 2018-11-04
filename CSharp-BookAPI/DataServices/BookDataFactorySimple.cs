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

        public BookDataFactorySimple(){}

        public BookDataFactorySimple(Object collection){}

        public Books GetBooks()
        {
            List<Book> booksList = collection.AsQueryable().ToList();

            Books books = new Books(booksList);

            return books;
        }

        public Book GetBook(string _id)
        {
            ObjectId Object_id;
            if (!ObjectId.TryParse(_id, out Object_id)) {
                return null;
            }

            IFindFluent<Book, Book> book = collection.Find(new BsonDocument { { "_id", Object_id } });
            if (book.CountDocuments() > 0)
            {
                return book.Single();
            }
            else
            {
                return null;
            }
        }

        public string CreateBook(Book book)
        {
            ObjectId? Object_id;
            if(book._id == null) {
                Object_id = ObjectId.GenerateNewId();
                book._id = Object_id;
            }
            else {
                if (book._id.ToString() == "000000000000000000000000"){
                    return "The id given is not a valid id";
                }
                else {
                    Object_id = book._id;
                }
            }
            collection.InsertOne(book);
            return $"link: /{Object_id}";
        }

        public ReplaceOneResult UpdateBook(string _id, Book book)
        {
            ObjectId Object_id;
            if (!ObjectId.TryParse(_id, out Object_id)) {
                return null;
            }
            book._id = Object_id;
            return collection.ReplaceOne(new BsonDocument { { "_id", Object_id } }, book, new UpdateOptions { IsUpsert = true });
        }

        public bool DeleteBook(string _id)
        {
            if (GetBook(_id) == null)
            {
                return false;
            }

            collection.DeleteOne(new BsonDocument { { "_id", ObjectId.Parse(_id) } });
            return true;
        }
    }
}
