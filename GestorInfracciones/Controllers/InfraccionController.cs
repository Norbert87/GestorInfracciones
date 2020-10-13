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


        // GET: api/Infraccion
        /// <summary>
        /// Obtiene la lista de infracciones registradas
        /// </summary>
        public List<Infraccion> Get()
        {
            return repository.getAll();
        }
        // GET: api/Infraccion/5
        /// <summary>
        /// Obtiene datos de una infracción dado el identificador
        /// </summary>
        /// <param name="id">Identificador de sacción</param>
        [ResponseType(typeof(Infraccion))]
        public IHttpActionResult Get(string id)
        {

            Infraccion infraccion = repository.getById(id);
            if (infraccion == null)
                return NotFound();
            return Ok(infraccion);
        }

        // GET: api/Infraccion/conductor/5
        /// <summary>
        /// Obtiene las infracciones dado un conductor
        /// </summary>
        [ResponseType(typeof(List<Infraccion>))]
        [Route("api/infraccion/conductor/{id}", Name = "GetInfraccionByConductor")]
        [HttpGet]
        public List<Infraccion> getInfraccionByConductor(string id)
        {
            return repository.getByConductor(id);
        }

        // GET: api/Infraccion/conductor/5
        /// <summary>
        /// Obtiene el top N de infracciones
        /// </summary>
        [ResponseType(typeof(void))]
        [Route("api/infraccion/top/{n}", Name = "getTopNinfracciones")]
        [HttpGet]
        public IHttpActionResult getTopNinfracciones(int n)
        {
            var resul =  repository.getAll()
                .GroupBy(i => i.TipoInfraccionIdentificador)
                .Select( group => new {tipo = group.Key, cantidad = group.Count()})
                .OrderByDescending(x => x.cantidad).Take(n);
            return Ok(resul);
        }

        // POST: api/Infraccion
        /// <summary>
        /// Registra una infracción Consideraciones: Si no se indica conductor, se buscará entre los conductores habituales. 
        /// Si solo existe un conductor habitual se asignará automaticamente. Si existen varios, no se asignará)
        /// </summary>
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

        // PUT: api/infraccion/{id}
        /// <summary>
        /// Acutaliza una infracción Consideraciones: Si no se indica conductor, se buscará entre los conductores habituales. 
        /// Si solo existe un conductor habitual se asignará automaticamente. Si existen varios, no se asignará)
        /// </summary>
        [ResponseType(typeof(Infraccion))]
        public IHttpActionResult Put(string id, Infraccion infraccion)
        {
            Infraccion oldinfraccion = repository.getById(id);
            if (oldinfraccion == null) {
                return BadRequest("No existe registrada esta infracción");
            }

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
            else
            {
                conductor = conductorRepository.getById(infraccion.DNI);
                if (conductor == null)
                {
                    return BadRequest("El conductor no está registrado");
                }
            }

            //Volvemos a devolver puntos si el conductor es diferente           
            if (oldinfraccion.DNI != null && oldinfraccion.DNI != infraccion.DNI)
            {
                Conductor conductorOld = conductorRepository.getById(oldinfraccion.DNI);
                TipoInfraccion tipoOld = tipoRepository.getById(oldinfraccion.TipoInfraccionIdentificador);
                conductorOld.Puntos += tipoOld.Puntos;
                conductorRepository.update(conductorOld);
            }

            repository.update(infraccion);

            //Si existe un nuevo conductor asignamos acutalizamos puntos
            if (conductor!=null)
            {
                conductor.Puntos -= tipo.Puntos;
                conductorRepository.update(conductor);
            }

            return CreatedAtRoute("DefaultApi", new { id = infraccion.Identificador }, infraccion);
        }
    }
}
