using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    public enum TiposRecursos { NO_ASIGNADO, Madera, Piedra, Comida, Bebida, Encendedor };

    abstract class Recurso
    { 
  

        public int cantidad { get; set; }
        public String descripcion { get; set; }
        abstract public  bool combinable(Recurso otroRecurso);
        public TiposRecursos tipo { get; set; }
    }
}
