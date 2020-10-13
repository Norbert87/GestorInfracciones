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
    public class ConductorHabitualController : ApiController
    {
        private ConductorHabitualRepository conductorHabitualRepository = new ConductorHabitualRepository();
        private ConductorRepository conductorRepository = new ConductorRepository();
        private VehiculoRepository vehiculoRepository = new VehiculoRepository();

        // GET: api/ConductorHabitual/5
        /// <summary>
        /// Obtiene los conductores habituales para un vehículo
        /// </summary>
        [ResponseType(typeof(List<Conductor>))]
        [Route("api/ConductorHabitual/{id}", Name = "getConductoresHabitualesByVehiculo")]
        [HttpGet]
        public IHttpActionResult Get(string id)
        {
            Vehiculo vehiculo = vehiculoRepository.getById(id);
            if (vehiculo == null)
                return NotFound();

            return Ok(conductorHabitualRepository.getByVehiculo(vehiculo));
        }


        // POST: api/ConductorHabitual
        /// <summary>
        /// Añade un conductor habitual a un vehículo
        /// </summary>
        [ResponseType(typeof(List<Conductor>))]
        public IHttpActionResult Post(ConductorHabitual conductorHabitual)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            
            Conductor conductor = conductorRepository.getById(conductorHabitual.DNI);

            if (conductor== null)
            {
                return BadRequest("No existe conductor");
            }

            List<Vehiculo> vehiculosHabituales = conductorHabitualRepository.getByConductor(conductor);
            if (vehiculosHabituales.Count == 10)
            {
                return BadRequest("El conductor ha superado el máximo de vehiculos habituales");
            }

            Vehiculo vehiculo = vehiculoRepository.getById(conductorHabitual.Matricula);
            if (vehiculo == null)
            {
                return BadRequest("No existe vehiculo");
            }

            if (conductorHabitualRepository.exists(conductor, vehiculo)) {
                return BadRequest("El vehiculo ya esta asociado a este conductor");
            }

            conductorHabitualRepository.add(conductor, vehiculo);

            return CreatedAtRoute("getConductoresHabitualesByVehiculo", new { id = vehiculo.Matricula }, conductorHabitualRepository.getByVehiculo(vehiculo)); 
        }


        // DELETE: api/ConductorHabitual
        /// <summary>
        /// Quita un conductor habitual de un vehículo
        /// </summary>
        [ResponseType(typeof(List<Conductor>))]
        public IHttpActionResult Delete(ConductorHabitual conductorHabitual)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Conductor conductor = conductorRepository.getById(conductorHabitual.DNI);

            if (conductor == null)
            {
                return BadRequest("No existe conductor");
            }

            Vehiculo vehiculo = vehiculoRepository.getById(conductorHabitual.Matricula);
            if (vehiculo == null)
            {
                return BadRequest("No existe vehiculo");
            }

            if (conductorHabitualRepository.exists(conductor, vehiculo))
            {
                conductorHabitualRepository.remove(conductor, vehiculo);
            }
            else
            {
                return BadRequest("No se localiza ese conductor habitual para el vehículo dado");
            }

            return CreatedAtRoute("getConductoresHabitualesByVehiculo", new { id = vehiculo.Matricula }, conductorHabitualRepository.getByVehiculo(vehiculo));
        }

    }
}
