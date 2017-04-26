using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Group.Model;

namespace TGC.Group.InventarioYObjetos
{
    class Fruta : Recurso
    {
        public Fruta()
        {
            tipo = TiposRecursos.Comida;
        }

        public override bool combinable(Recurso otroRecurso)
        {
            return false;
        }


    }
}
