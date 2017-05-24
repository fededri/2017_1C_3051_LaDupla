using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Group.Model;

namespace TGC.Group.InventarioYObjetos
{
    class Hacha : Recurso
    {
        public Hacha()
        {
            tipo = TiposRecursos.Hacha;
        }

        public override bool combinable(Recurso recurso)
        {
            return false;
        }

         public override bool usar(Personaje personaje)
        {
            return false;
        }
    }
}
