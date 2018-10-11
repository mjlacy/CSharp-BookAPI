using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using Newtonsoft.Json;
using System.Collections.Generic;

namespace CSharp_BookAPI.Models
{
    [BsonIgnoreExtraElements]
    public class Book
    {
        [JsonIgnore]
        [BsonElement("_id")]
        public ObjectId? _id { get; set; }

        [JsonProperty("_id")]
        [BsonIgnore]
        public string _id_serialized
        {
            get => _id.ToString();
            set {
                ObjectId Object_id;
                if( ObjectId.TryParse(value, out Object_id)){
                    _id = Object_id;
                } 
                else {
                    _id = ObjectId.Parse("000000000000000000000000");
                }}
        }

        [JsonProperty("bookId")]
        public int bookId { get; set; }

        [JsonProperty("title")]
        public string title { get; set; }

        [JsonProperty("author")]
        public string author { get; set; }

        [JsonProperty("year")]
        public int year { get; set; }

        public Book() {}

        public Book(ObjectId _id, int bookId, string title, string author, int year)
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
