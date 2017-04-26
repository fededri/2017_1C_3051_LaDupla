using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Group.Model;

namespace TGC.Group.InventarioYObjetos
{
    class Botella : Recurso
    {
        public bool tieneAgua { get; set; }

        public Botella()
        {           
            tipo = TiposRecursos.Bebida;
        }

        public override bool combinable(Recurso recurso)
        {
            return false;
        }
    }
}
