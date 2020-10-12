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
    public class InfraccionController : ApiController
    {
        private InfraccionRepository repository = new InfraccionRepository();
        private ConductorHabitualRepository conductorHabitualRepository = new ConductorHabitualRepository();
        private ConductorRepository conductorRepository = new ConductorRepository();
        private VehiculoRepository vehiculoRepository = new VehiculoRepository();
        private TipoInfraccionRepository tipoRepository = new TipoInfraccionRepository();

        // GET: api/infraccion/5
        [ResponseType(typeof(Infraccion))]
        public IHttpActionResult Get(string id)
        {

            Infraccion infraccion = repository.getById(id);
            if (infraccion == null)
                return NotFound();
            return Ok(infraccion);
        }

        // POST: api/infraccion
        [ResponseType(typeof(Infraccion))]
        public IHttpActionResult Post(Infraccion infraccion)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Vehiculo vehiculo = vehiculoRepository.getById(infraccion.Matricula);
            if (vehiculo == null)
            {
                return BadRequest("No existe vehículo");
            }

            TipoInfraccion tipo = tipoRepository.getById(infraccion.TipoInfraccionIdentificador);
            if (tipo == null)
            {
                return BadRequest("No existe tipo de infracción");
            }
            Conductor conductor = null;
            if (infraccion.DNI == null)
            {
                List<Conductor> conductoresHabituales = conductorHabitualRepository.getByVehiculo(vehiculo);
                if (conductoresHabituales.Count == 1)
                {
                    conductor = conductoresHabituales.FirstOrDefault();
                }
            }
            else {
                conductor = conductorRepository.getById(infraccion.DNI);
                if (conductor == null) {
                    return BadRequest("El conductor no está registrado");
                }
            }

            Infraccion infraccionNew = repository.add(tipo, vehiculo, infraccion.Fecha, conductor);

            if (conductor != null) {
                conductor.Puntos -= tipo.Puntos;
                conductorRepository.update(conductor);
            }

            return CreatedAtRoute("DefaultApi", new { id = infraccionNew.Identificador }, infraccionNew);
        }

        // GET: api/infraccion/conductor/5
        [ResponseType(typeof(List<Infraccion>))]
        [Route("api/infraccion/conductor/{id}", Name = "GetInfraccionByConductor")]
        [HttpGet]
        public List<Infraccion> getInfraccionByConductor(string id)
        {
            return repository.getByConductor(id);
        }
    }
}
