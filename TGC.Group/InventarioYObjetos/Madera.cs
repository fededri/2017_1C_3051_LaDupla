using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Group.Model;
using static TGC.Group.Model.TiposRecursos;

namespace TGC.Group.InventarioYObjetos
{
    class Madera : Recurso
    {

        public Madera()
        {
            descripcion = "Madera obtenida de un arbol, permite crear fogatas y barcas";
        }

        public override bool combinable(Recurso otroRecurso)
        {       
            if(otroRecurso.tipo == TiposRecursos.Madera)
            {
                return true;
            } else if(otroRecurso.tipo == Encendedor)
            {
                return true;
            }  else
            {
                return false;
            }

          
        }

       
    }
}
