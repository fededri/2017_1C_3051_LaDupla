using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Group.Model;

namespace TGC.Group.InventarioYObjetos
{
    class Motor : Recurso
    {
        public override bool usar(Personaje personaje)
        {
            return false;
        }

        public override bool combinable(Recurso otroRecurso)
        {
            return true;
        }

        public Motor()
        {
            descripcion = "Piedra, utilizada para crear dagas de piedra";
            tipo = TiposRecursos.Motor;
        }
    }
}
