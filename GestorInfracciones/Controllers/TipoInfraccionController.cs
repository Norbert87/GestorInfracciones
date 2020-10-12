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
    public class TipoInfraccionController : ApiController
    {
        private TipoInfraccionRepository repository = new TipoInfraccionRepository();
        // GET: api/TipoInfraccion
        public List<TipoInfraccion> Get()
        {
            return repository.getAll();
        }

        // GET: api/TipoInfraccion/5
        [ResponseType(typeof(TipoInfraccion))]
        public IHttpActionResult Get(string id)
        {

            TipoInfraccion tipoInfraccion = repository.getById(id);
            if (tipoInfraccion == null)
                return NotFound();
            return Ok(tipoInfraccion);
        }

        // POST: api/TipoInfraccion
        [ResponseType(typeof(TipoInfraccion))]
        public IHttpActionResult Post(TipoInfraccion tipoInfraccion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (repository.exists(tipoInfraccion.Identificador))
            {
                return BadRequest("Ya existe el tipoInfraccion");
            }

            repository.add(tipoInfraccion);

            return CreatedAtRoute("DefaultApi", new { id = tipoInfraccion.Identificador }, tipoInfraccion); ;
        }
    }
}
