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
        private int reduceSed = 80;
        public Botella()
        {           
            tipo = TiposRecursos.Bebida;
        }

        public override bool combinable(Recurso recurso)
        {
            return false;
        }

        public override bool usar(Personaje personaje)
        {           
                if (personaje.sed > (100 - reduceSed)) personaje.sed = 100;
                else
                {
                    personaje.sed += reduceSed;
                }
                tieneAgua = false;
                personaje.quitarRecurso(this);
                return true;

        }
    }
}
