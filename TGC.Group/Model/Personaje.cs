using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TGC.Group.Model
{
    class Personaje
    {
        int vida { get; set; }
        int hambre { get; set; }
        int sed { get; set; }
        int energia { get; set; }
        private List<Recurso> recursos;
        public Hud.Hud hud;

        public Personaje(Hud.Hud hud)
        {
            recursos = new List<Recurso>();
            this.hud = hud;
        }


        public bool inventarioLleno()
        {
            return recursos.Count()>=6;
        }

        public void agregarRecurso(Recurso recurso)
        {
            if (recursos.Count() >= 6) throw new Exception("Se quiso agregar un recurso pero el inventario esta lleno");
            recursos.Add(recurso);
            hud.agregarItem(recurso);
        }





    }
}
