using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestorInfracciones.DAL
{
    interface IOInterface
    {
        void create();
        string read();
        void write(string text);
    }
}
