using GestorInfracciones.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GestorInfracciones.DAL
{
    public class TipoInfraccionRepository: JsonIORepository
    {
        public TipoInfraccionRepository()
        {
            this.filePath = this.GetApplicationRoot() + "\\Data\\tipoinfracciones.json";
        }
        public List<TipoInfraccion> getAll()
        {
            string json = this.read();
            return JsonConvert.DeserializeObject<List<TipoInfraccion>>(json);
        }

        public void add(TipoInfraccion tipoInfraccion)
        {
            List<TipoInfraccion> tipoInfracciones = this.getAll();
            tipoInfracciones.Add(tipoInfraccion);
            string json = JsonConvert.SerializeObject(tipoInfracciones);
            this.write(json);
        }

        public TipoInfraccion getById(string id)
        {
            List<TipoInfraccion> tipoInfracciones = this.getAll();
            return tipoInfracciones.Where(x => x.Identificador == id).FirstOrDefault();
        }

        public bool exists(string id)
        {
            TipoInfraccion tipoInfraccion = this.getById(id);
            return tipoInfraccion != null;
        }
    }
}