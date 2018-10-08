using System;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Cors;
using CSharp_BookAPI.DataServices;
using CSharp_BookAPI.Models;
using Newtonsoft.Json;

namespace CSharp_BookAPI.Controllers
{
    [Route("/")]
    public class BooksController : Controller
    {
        public IBookDataFactory BookDataFactory = new BookDataFactorySimple();

        [HttpGet("health")]
        [EnableCors("CorsPolicy")]
        public IActionResult Health()
        {
            var result = new JsonResult("ok");
            result.StatusCode = 200;
            return result;
        }

        [HttpGet()]
        [EnableCors("CorsPolicy")]
        public IActionResult GetBooks()
        {
            try
            {
                var books = new JsonResult(BookDataFactory.GetBooks());
                books.StatusCode = 200;
                return books;
            }
            catch (Exception ex)
            {
                var error = new JsonResult("An error occured while processing your request.");
                error.StatusCode = 500;
                return error;
            }
        }

        [HttpGet("{id}")]
        [EnableCors("CorsPolicy")]
        public IActionResult GetBook(string id)
        {
            try
            {
                JsonResult response;
                if (BookDataFactory.GetBook(id)._id != null) {
                    response = new JsonResult(BookDataFactory.GetBook(id));
                    response.StatusCode = 200;
                }
                else {
                    response = new JsonResult("No book found with that id");
                    response.StatusCode = 404;
                }
                return response;
            }
            catch (Exception ex)
            {
                var error = new JsonResult("An error occured while processing your request: " + ex );
                error.StatusCode = 500;
                return error;
            }
        }

        [HttpPost()]
        public IActionResult CreateBook([FromBody] Newtonsoft.Json.Linq.JObject body)
        {
            try
            {
                var result = new JsonResult(BookDataFactory.CreateBook(JsonConvert.DeserializeObject<Book>(body.ToString())));
                result.StatusCode = 201;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in CreateBook: " + ex);
                var error = new JsonResult("An error occured while processing your request: " + ex );
                error.StatusCode = 500;
                return error;
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(string id, [FromBody] Newtonsoft.Json.Linq.JObject body)
        {
            try
            {
                var result = new JsonResult(BookDataFactory.UpdateBook(id, JsonConvert.DeserializeObject<Book>(body.ToString())));
                result.StatusCode = 201;
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in CreateBook: " + ex);
                var error = new JsonResult("An error occured while processing your request: " + ex );
                error.StatusCode = 500;
                return error;
            }
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteBook(string id)
        {
            try
            {
                JsonResult response;
                if (BookDataFactory.DeleteBook(id)){
                    response = new JsonResult("Object with id: " + id + " deleted successfully");
                    response.StatusCode = 200;
                }
                else {
                    response = new JsonResult("No object with an id of: " + id + " found to delete");
                    response.StatusCode = 404;
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Exception in DeleteBook: " + ex);
                var error = new JsonResult("An error occured while processing your request: " + ex );
                error.StatusCode = 500;
                return error;
            }
        }
    }
}