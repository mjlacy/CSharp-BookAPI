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
            JsonResult result = new JsonResult("ok");
            result.StatusCode = 200;
            return result;
        }

        [HttpGet()]
        [EnableCors("CorsPolicy")]
        public IActionResult GetBooks()
        {
            try
            {
                JsonResult books = new JsonResult(BookDataFactory.GetBooks());
                books.StatusCode = 200;
                return books;
            }
            catch (Exception)
            {
                JsonResult error = new JsonResult("An error occured while processing your request.");
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
            catch (Exception)
            {
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
                string result = BookDataFactory.CreateBook(JsonConvert.DeserializeObject<Book>(body.ToString()));
                JsonResult response = new JsonResult(result);
                if (result == "The id given is not a valid id"){
                    response.StatusCode = 400;
                }
                else {
                    response.StatusCode = 201;
                }
                return response;
            }
            catch (Exception)
            {
                JsonResult error = new JsonResult("An error occured while processing your request");
                error.StatusCode = 500;
                return error;
            }
        }

        [HttpPut("{id}")]
        public IActionResult UpdateBook(string id, [FromBody] Newtonsoft.Json.Linq.JObject body)
        {
            try
            {
                var result = BookDataFactory.UpdateBook(id, JsonConvert.DeserializeObject<Book>(body.ToString()));
                JsonResult response;
                if(result == null){
                    response = new JsonResult("The id you specified is not a valid id");
                    response.StatusCode = 400;
                }
                else if(result.UpsertedId != null){
                    response = new JsonResult($"link: /{result.UpsertedId}");
                    response.StatusCode = 201;
                }
                else {
                    response = new JsonResult($"link: /{id}");
                    response.StatusCode = 200;
                }
                return response;
            }
            catch (Exception)
            {
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
            catch (Exception)
            {
                JsonResult error = new JsonResult("An error occured while processing your request");
                error.StatusCode = 500;
                return error;
            }
        }
    }
}