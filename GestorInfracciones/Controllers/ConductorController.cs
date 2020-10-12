using GestorInfracciones.DAL;
using GestorInfracciones.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;

namespace GestorInfracciones.Controllers
{
    public class ConductorController : ApiController
    {
        private ConductorRepository repository = new ConductorRepository();
        // GET: api/Conductor
        public List<Conductor> Get()
        {
            return repository.getAll();
        }

        // GET: api/Conductor/top/{n}
        [ResponseType(typeof(List<Conductor>))]
        [Route("api/conductor/top/{n}", Name = "GetTopNconductores")]
        [HttpGet]
        public List<Conductor> getInfraccionByConductor(int n)
        {
            return repository.getAll().Take(n).ToList();
        }

        // GET: api/Conductor/5
        [ResponseType(typeof(Conductor))]
        public IHttpActionResult Get(string id)
        {
           
            Conductor conductor = repository.getById(id);
            if (conductor == null)
                return NotFound();
            return Ok(conductor);
        }

        // POST: api/Conductor
        [ResponseType(typeof(Conductor))]
        public IHttpActionResult Post(Conductor conductor)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (repository.exists(conductor.DNI)) {
                return BadRequest("Ya existe el conductor");
            }

            repository.add(conductor);

            return CreatedAtRoute("DefaultApi", new { id = conductor.DNI }, conductor); ;
        }

        //// POST: api/Conductor
        //public void Post([FromBody]string value)
        //{
        //}

        //// PUT: api/Conductor/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE: api/Conductor/5
        //public void Delete(int id)
        //{
        //}
    }
}
