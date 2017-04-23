using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Group.Model;

namespace TGC.Group.InventarioYObjetos
{
    class Piedra : Recurso
    {
        public Piedra()
        {
            descripcion = "Piedra, utilizada para crear dagas de piedra";
        }

        public override bool combinable(Recurso otroRecurso)
        {
            return false;
        }
    }
}
