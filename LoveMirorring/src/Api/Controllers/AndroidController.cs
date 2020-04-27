using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Api.Controllers
{
    class Film
    {
        public string name { get; set; }
        public int annee { get; set; }
        public string realisateur { get; set; }
    }

    [Route("api/[controller]")]
    public class AndroidController : Controller
    {
        // GET: api/<controller>
        //[HttpGet]
        //public string Get()
        //{
        //    return "[{\"nom\":\"Green book\",\"annee\":2019,\"realisateur\":\"Peter Farelly\"},{\"nom\":\"Joker\",\"annee\":2019,\"realisateur\":\"Todd Phillips\"}]";
        //}

        [HttpGet]
        public IActionResult Get()
        {
            List<Film> films = new List<Film>();
            Film film = new Film();
            film.name = "La vie des autres";
            film.annee = 2007;
            film.realisateur = "Florian Henkel";
            films.Add(film);

            Film film2 = new Film();
            film2.name = "Le pont des espions";
            film2.annee = 2015;
            film2.realisateur = "Steven Spielberg";
            films.Add(film2);

            Film film3 = new Film();
            film3.name = "Green book";
            film3.annee = 2019;
            film3.realisateur = "Peter Farelly";
            films.Add(film3);

            Film film4 = new Film();
            film4.name = "Joker";
            film4.annee = 2019;
            film4.realisateur = "Todd Phillips";
            films.Add(film4);

            return new JsonResult(films);
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        [HttpPost]
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
