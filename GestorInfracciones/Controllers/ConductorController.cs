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
        /// <summary>
        /// Obtiene la lista de conductores
        /// </summary>
        public List<Conductor> Get()
        {
            return repository.getAll();
        }

        // GET: api/Conductor/top/{n}
        /// <summary>
        /// Obtiene los top n coductores
        /// </summary>
        [ResponseType(typeof(List<Conductor>))]
        [Route("api/Conductor/top/{n}", Name = "GetTopNconductores")]
        [HttpGet]
        public List<Conductor> getInfraccionByConductor(int n)
        {
            return repository.getAll().Take(n).ToList();
        }

        /// <summary>
        /// Obtiene los datos del conductor
        /// </summary>
        // GET: api/Conductor/5
        [ResponseType(typeof(Conductor))]
        public IHttpActionResult Get(string id)
        {
           
            Conductor conductor = repository.getById(id);
            if (conductor == null)
                return NotFound();
            return Ok(conductor);
        }

        /// <summary>
        /// Registra nuevo conductor
        /// </summary>
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
    }
}
