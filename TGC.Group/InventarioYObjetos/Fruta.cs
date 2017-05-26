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
        private int aumentaComida = 40;

        public Fruta()
        {
            tipo = TiposRecursos.Comida;
        }

        public override bool combinable(Recurso otroRecurso)
        {
            return false;
        }

        public override bool usar(Personaje personaje)
        {
            if (personaje.hambre >= (100 - aumentaComida)) personaje.hambre = 100;
            else
            {
                personaje.hambre += 20;
            }
            personaje.quitarRecurso(this);
            return true;
        }
    }
}
