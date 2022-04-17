using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Books.Controllers;

[ApiController]
 [Route("api/[controller]")]
public class BooksController : ControllerBase
{
    private static List<Booksitems> Books = new List<Booksitems> {
        new Booksitems
        {
                Term= "Access Token",
                Definition = "A credential that can be used by an application to access an API. It informs the API that the bearer of the token has been authorized to access the API and perform specific actions specified by the scope that has been granted."
         },
        new Booksitems
        {
                Term= "JWT",
                Definition = "An open, industry standard RFC 7519 method for representing claims securely between two parties. "
        },
        new Booksitems
        {
                Term= "OpenID",
                Definition = "An open standard for authentication that allows applications to verify users are who they say they are without needing to collect, store, and therefore become liable for a user’s login information."
        }


    };

    [HttpGet]
        public ActionResult<List<Booksitems>> Get()
        {
            return Ok(Books);
        }

    [HttpGet]
        [Route("{term}")]
        public ActionResult<Booksitems> Get(string term)
        {
            var booksitems = Books.Find(item => 
                    item.Term.Equals(term, StringComparison.InvariantCultureIgnoreCase));

            if (booksitems == null)
            {
                return NotFound();
            } else
            {
                return Ok(booksitems);
            }
        }  

    [HttpPost]
        public ActionResult Post(Booksitems booksitems)
        {
            var existingBooksitems = Books.Find(item =>
                    item.Term.Equals(booksitems.Term, StringComparison.InvariantCultureIgnoreCase));

            if (existingBooksitems != null)
            {
                return Conflict("Cannot create the term because it already exists.");
            }
            else
            {
                Books.Add(booksitems);
                var resourceUrl = Path.Combine(Request.Path.ToString(), Uri.EscapeUriString(booksitems.Term));
                return Created(resourceUrl, booksitems);
            }
        } 

    [HttpPut]
            public ActionResult Put(Booksitems booksitems)
            {
                var existingBooksitems = Books.Find(item =>
                item.Term.Equals(booksitems.Term, StringComparison.InvariantCultureIgnoreCase));

                if (existingBooksitems == null)
                {
                    return NotFound("Cannot update a nont existing term.");
                } else
                {
                    existingBooksitems.Definition = booksitems.Definition;
                    return Ok();
                }
            } 

    [HttpDelete]
        [Route("{term}")]
        public ActionResult Delete(string term)
        {
            var booksitems = Books.Find(item =>
                   item.Term.Equals(term, StringComparison.InvariantCultureIgnoreCase));

            if (booksitems == null)
            {
                return NotFound();
            }
            else
            {
                Books.Remove(booksitems);
                return NoContent();
            }
        }                


   /** private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    }; **/

    private readonly ILogger<BooksController> _logger;

    public BooksController(ILogger<BooksController> logger)
    {
        _logger = logger;
    }

    /**[HttpGet(Name = "GetWeatherForecast")]
    public IEnumerable<WeatherForecast> Get()
    {
        return Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();
    } **/
}
