using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Group.Model;

namespace TGC.Group.InventarioYObjetos
{
    class Encendedor : Recurso
    {
        public Encendedor()
        {
            tipo = TiposRecursos.Encendedor;
        }

        public override bool combinable(Recurso otroRecurso)
        {
            return otroRecurso.tipo == TiposRecursos.Madera;
        }

        public override bool usar(Personaje personaje)
        {          
            return false;
        }
    }
}
