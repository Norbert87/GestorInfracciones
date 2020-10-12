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
    public class VehiculoController : ApiController
    {
        private VehiculoRepository repository = new VehiculoRepository();
        // GET: api/Vehiculo
        public List<Vehiculo> Get()
        {
            return repository.getAll();
        }

        // GET: api/Vehiculo/5
        [ResponseType(typeof(Vehiculo))]
        public IHttpActionResult Get(string id)
        {

            Vehiculo vehiculo = repository.getById(id);
            if (vehiculo == null)
                return NotFound();
            return Ok(vehiculo);
        }

        // POST: api/Vehiculo
        [ResponseType(typeof(Vehiculo))]
        public IHttpActionResult Post(Vehiculo vehiculo)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if (repository.exists(vehiculo.Matricula))
            {
                return BadRequest("Ya existe el vehiculo");
            }

            repository.add(vehiculo);

            return CreatedAtRoute("DefaultApi", new { id = vehiculo.Matricula }, vehiculo); ;
        }
    }
}
