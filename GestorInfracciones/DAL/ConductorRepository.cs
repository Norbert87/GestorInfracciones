using GestorInfracciones.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorInfracciones.DAL
{
    public class ConductorRepository: JsonIORepository
    {
        public ConductorRepository() {
            this.filePath = this.GetApplicationRoot() + "\\Data\\conductores.json";
        }
        public List<Conductor> getAll() {
            string json = this.read();
            return JsonConvert.DeserializeObject<List<Conductor>>(json);
        }

        public void add(Conductor conductor)
        {
            List<Conductor> conductores = this.getAll();
            conductores.Add(conductor);
            string json = JsonConvert.SerializeObject(conductores);
            this.write(json);
        }
        public void update(Conductor conductor)
        {
            List<Conductor> conductores = this.getAll();
            Conductor oldConductor = conductores.Where(c => c.DNI == conductor.DNI).FirstOrDefault();
            conductores.Remove(oldConductor);
            conductores.Add(conductor);
            string json = JsonConvert.SerializeObject(conductores);
            this.write(json);
        }


        public Conductor getById(string id) {
            List<Conductor> conductores = this.getAll();
            return conductores.Where(x => x.DNI == id).FirstOrDefault();
        }

        public bool exists(string id)
        {
            Conductor conductor = this.getById(id);
            return conductor!=null;
        }
    }
}