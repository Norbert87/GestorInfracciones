using GestorInfracciones.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorInfracciones.DAL
{
    public class InfraccionRepository: JsonIORepository
    {
        private ConductorRepository conductorRepository = new ConductorRepository();
        private TipoInfraccionRepository tipoRepository = new TipoInfraccionRepository();
        public InfraccionRepository()
        {
            this.filePath = this.GetApplicationRoot() + "\\Data\\infracciones.json";
        }

        public List<Infraccion> getAll()
        {
            string json = this.read();
            return JsonConvert.DeserializeObject<List<Infraccion>>(json);
        }

        public Infraccion getById(string id)
        {
            List<Infraccion> infracciones = this.getAll();
            return infracciones.Where(x => x.Identificador == id).FirstOrDefault();
        }

        public List<Infraccion> getByVehiculo(Vehiculo vehiculo)
        {
            List<Infraccion> infracciones = this.getAll();
            return infracciones.Where(x => x.Matricula == vehiculo.Matricula).ToList();
        }

        public List<Infraccion> getByConductor(string id)
        {
            List<Infraccion> infracciones = this.getAll();
            return infracciones.Where(x => x.DNI == id).ToList();
        }

        public Infraccion add(TipoInfraccion tipo, Vehiculo vehiculo, DateTime fecha, Conductor conductor = null )
        {
            List<Infraccion> infracciones = this.getAll();
            Infraccion infraccion = new Infraccion() { Identificador= Guid.NewGuid().ToString(), Fecha = fecha, TipoInfraccionIdentificador = tipo.Identificador, DNI = conductor != null ? conductor.DNI : null, Matricula = vehiculo.Matricula };
            infracciones.Add(infraccion);
            string json = JsonConvert.SerializeObject(infracciones);
            this.write(json);
            return infraccion;
        }

        public void update(Infraccion infraccion)
        {
            List<Infraccion> infracciones = this.getAll();
            Infraccion oldinfraccion = infracciones.Where(i => i.Identificador == infraccion.Identificador).FirstOrDefault();
            infracciones.Remove(oldinfraccion);
            
            infracciones.Add(infraccion);
            string json = JsonConvert.SerializeObject(infracciones);
            this.write(json);
        }

    }
}