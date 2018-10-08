using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CSharp_BookAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Book
    {
        [BsonElement("_id")]
        public ObjectId? _id { get; set; }

        [JsonProperty("bookId")]
        public int bookId { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("author")]
        public string author { get; set; }

        [JsonProperty("year")]
        public int year { get; set; }

        public Book() {}

        public Book(ObjectId? _id, int bookId, string title, string author, int year)
        {
            this._id = _id;
            this.bookId = bookId;
            this.title = title;
            this.author = author;
            this.year = year;
        }
    }

    public class Books
    {
        [JsonProperty("books")]
        public List<Book> books { get; set; }

        public Books (List<Book> books)
        {
            this.books = books;
        }
    }
}
