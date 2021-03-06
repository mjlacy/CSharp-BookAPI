﻿using System;
using System.Collections.Generic;
using CSharp_BookAPI.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Bson.Serialization;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace CSharp_BookAPI.DataServices
{
    public class BookDataFactorySimple : IBookDataFactory
    {
        static IMongoClient client = new MongoClient("mongodb://localhost:27017");
        static IMongoDatabase database = client.GetDatabase("books");
        IMongoCollection<Book> collection = database.GetCollection<Book>("books");

        public bool Health() {
            BsonDocument ping = database.RunCommand<BsonDocument>(new BsonDocument("ping", 1));
            return ping.GetValue("ok") == 1;
        }
        public Books GetBooks(Book book)
        {   
            Dictionary<string, object> conditions = new Dictionary<string, object>();

            if (book.bookId != 0) {
                conditions["bookId"] = book.bookId;
            }
            if (book.title != null) {
                conditions["title"] = book.title;
            }
            if (book.author != null) {
                conditions["author"] = book.author;
            }
            if (book.year != 0) {
                conditions["year"] = book.year;
            }
            
            List<Book> booksList = collection.Find(conditions.ToBsonDocument()).ToList();
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

        public Book CreateBook(Book book)
        {
            ObjectId? Object_id;
            if(book._id == null) {
                Object_id = ObjectId.GenerateNewId();
                book._id = Object_id;
            }
            else { 
                Object_id = book._id;
            }
            collection.InsertOne(book);
            return book;
        }

        public ReplaceOneResult UpsertBook(string _id, Book book)
        {
            ObjectId Object_id;
            if (!ObjectId.TryParse(_id, out Object_id)) {
                return null;
            }
            book._id = Object_id;

            return collection.ReplaceOne(new BsonDocument { { "_id", Object_id } }, book, new UpdateOptions { IsUpsert = true });
        }

        public UpdateResult UpdateBook(string _id, object book)
        {
            ObjectId Object_id;
            if (!ObjectId.TryParse(_id, out Object_id)) {
                return null;
            }

            return collection.UpdateOne(new BsonDocument { { "_id", Object_id } }, new BsonDocument {{"$set", BsonDocument.Parse(book.ToString())}});
        }

        public DeleteResult DeleteBook(string _id)
        {
            ObjectId Object_id;
            if (!ObjectId.TryParse(_id, out Object_id)) {
                return null;
            }

            return collection.DeleteOne(new BsonDocument { { "_id", ObjectId.Parse(_id) } });
        }
    }
}
