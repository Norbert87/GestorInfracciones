using GestorInfracciones.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorInfracciones.DAL
{
    public class VehiculoRepository: JsonIORepository
    {
        public VehiculoRepository()
        {
            this.filePath = this.GetApplicationRoot() + "\\Data\\vehiculos.json";
        }
        public List<Vehiculo> getAll()
        {
            string json = this.read();
            return JsonConvert.DeserializeObject<List<Vehiculo>>(json);
        }

        public void add(Vehiculo conductor)
        {
            List<Vehiculo> conductores = this.getAll();
            conductores.Add(conductor);
            string json = JsonConvert.SerializeObject(conductores);
            this.write(json);
        }

        public Vehiculo getById(string id)
        {
            List<Vehiculo> conductores = this.getAll();
            return conductores.Where(x => x.Matricula == id).FirstOrDefault();
        }

        public bool exists(string id)
        {
            Vehiculo conductor = this.getById(id);
            return conductor != null;
        }
    }
}