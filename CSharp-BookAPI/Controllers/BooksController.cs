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
    [EnableCors("CorsPolicy")]
    public class BooksController : Controller
    {
        public IBookDataFactory BookDataFactory = new BookDataFactorySimple();

        [HttpGet("health")]
        public IActionResult Health()
        {
            try{
               if (BookDataFactory.Health()){
                    JsonResult result = new JsonResult("ok");
                    result.StatusCode = 200;
                    return result;
                } else {
                    JsonResult result = new JsonResult("Error connecting to database");
                    result.StatusCode = 500;
                    return result;
                } 
            }
            catch (Exception ex) {
                Console.WriteLine($"Exception Thrown: {ex}");
                JsonResult result = new JsonResult("Error connecting to database");
                result.StatusCode = 500;
                return result;
            }
            
        }

        [HttpGet()]
        public IActionResult GetBooks()
        {
            try
            {
                JsonResult books = new JsonResult(BookDataFactory.GetBooks());
                books.StatusCode = 200;
                return books;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Thrown: {ex}");
                JsonResult error = new JsonResult("An error occured while processing your request.");
                error.StatusCode = 500;
                return error;
            }
        }

        [HttpGet("{id}")]
        public IActionResult GetBook(string id)
        {
            try
            {
                JsonResult response;
                if (BookDataFactory.GetBook(id) != null) {
                    response = new JsonResult(BookDataFactory.GetBook(id));
                    response.StatusCode = 200;
                    Response.Headers.Add("Location", $"/{id}");
                }
                else {
                    response = new JsonResult("No book found with that id");
                    response.StatusCode = 404;
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Thrown: {ex}");
                JsonResult error = new JsonResult("An error occured while processing your request");
                error.StatusCode = 500;
                return error;
            }
        }

        [HttpPost()]
        public IActionResult CreateBook([FromBody] Newtonsoft.Json.Linq.JObject body)
        {
            try
            {
                Book result = BookDataFactory.CreateBook(JsonConvert.DeserializeObject<Book>(body.ToString()));
                JsonResult response = new JsonResult(result);
                response.StatusCode = 201;
                Response.Headers.Add("Location", $"/{result._id}");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Thrown: {ex}");
                JsonResult error = new JsonResult("An error occured while processing your request");
                error.StatusCode = 500;
                return error;
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpsertBook(string id, [FromBody] Newtonsoft.Json.Linq.JObject body)
        {
            try
            {
                var result = BookDataFactory.UpsertBook(id, JsonConvert.DeserializeObject<Book>(body.ToString()));
                JsonResult response;
                if(result == null){
                    response = new JsonResult("The id you specified is not a valid id");
                    response.StatusCode = 400;
                }
                response = new JsonResult(body);
                Response.Headers.Add("Location", $"/{id}");
                if(result.UpsertedId != null){
                    response.StatusCode = 201;
                }
                else {
                    response.StatusCode = 200;
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Thrown: {ex}");
                JsonResult error = new JsonResult("An error occured while processing your request");
                error.StatusCode = 500;
                return error;
            }
        }

        [HttpPatch("{id}")]
        public IActionResult UpdateBook(string id, [FromBody] Newtonsoft.Json.Linq.JObject body){
            try
            {
                var result = BookDataFactory.UpdateBook(id, body);
                JsonResult response;
                if(result == null){
                    response = new JsonResult("The id you specified is not a valid id");
                    response.StatusCode = 400;
                }
                else if (result.MatchedCount == 0){
                    response = new JsonResult($"No object with an id of: {id} found to modify");
                    response.StatusCode = 404;
                }
                else {
                    response = new JsonResult(new Newtonsoft.Json.Linq.JObject());
                    Response.Headers.Add("Location", $"/{id}");
                    response.StatusCode = 200;
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Thrown: {ex}");
                JsonResult error = new JsonResult("An error occured while processing your request");
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
                if (BookDataFactory.DeleteBook(id).DeletedCount > 0){
                    response = new JsonResult($"Object with id: {id} deleted successfully");
                    response.StatusCode = 200;
                }
                else {
                    response = new JsonResult($"No object with an id of: {id} found to delete");
                    response.StatusCode = 404;
                }
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception Thrown: {ex}");
                JsonResult error = new JsonResult("An error occured while processing your request");
                error.StatusCode = 500;
                return error;
            }
        }
    }
}