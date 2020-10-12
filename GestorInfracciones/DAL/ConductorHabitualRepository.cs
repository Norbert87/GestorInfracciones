using GestorInfracciones.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorInfracciones.DAL
{
    public class ConductorHabitualRepository: JsonIORepository
    {
        private ConductorRepository conductorRepository = new ConductorRepository();
        private VehiculoRepository vehiculoRepository = new VehiculoRepository();

        public ConductorHabitualRepository()
        {
            this.filePath = this.GetApplicationRoot() + "\\Data\\conductoreshabituales.json";
            
        }

        public List<ConductorHabitual> getAll()
        {
            string json = this.read();
            return JsonConvert.DeserializeObject<List<ConductorHabitual>>(json);
        }

        public void add(Conductor conductor, Vehiculo vehiculo)
        {
            List<ConductorHabitual> conductores = this.getAll();
            ConductorHabitual conductorHabitual = new ConductorHabitual() { DNI = conductor.DNI, Matricula = vehiculo.Matricula };
            conductores.Add(conductorHabitual);
            string json = JsonConvert.SerializeObject(conductores);
            this.write(json);
        }

        public bool exists(Conductor conductor, Vehiculo vehiculo)
        {
            List<ConductorHabitual> all = this.getAll();
            ConductorHabitual conductorHabitual = all.Where(x => x.Matricula == vehiculo.Matricula && x.DNI == conductor.DNI).ToList().FirstOrDefault();
            return conductorHabitual != null;
        }

        public List<Conductor> getByVehiculo(Vehiculo vehiculo)
        {
            List<ConductorHabitual> all = this.getAll();
            List<ConductorHabitual> conductoresHabitules = all.Where(x => x.Matricula == vehiculo.Matricula).ToList();
            List<string> ids = conductoresHabitules.Select(x => x.DNI).ToList();
            List<Conductor> conductores = this.conductorRepository.getAll().Where(c => ids.Contains(c.DNI)).ToList();

            return conductores;
        }

        public List<Vehiculo> getByConductor(Conductor conductor)
        {
            List<ConductorHabitual> all = this.getAll();
            List<ConductorHabitual> conductoresHabitules = all.Where(x => x.Matricula == conductor.DNI).ToList();
            List<string> ids = conductoresHabitules.Select(x => x.Matricula).ToList();
            List<Vehiculo> vehiculos = this.vehiculoRepository.getAll().Where(v=> ids.Contains(v.Matricula)).ToList();

            return vehiculos;
        }


    }
}