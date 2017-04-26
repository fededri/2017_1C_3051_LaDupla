using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using TGC.Core.Text;
using TGC.Group.Model;

namespace TGC.Group.InventarioYObjetos
{
    class Palmera : Crafteable
    {
       
        

        public Palmera(TgcMesh mesh)
        {
            this.mesh = mesh;
            this.destruirse = false;
        }


        public override Recurso dameTuRecurso()
        {
            return new Madera();
        }


        public override string nombreRecurso()
        {
            return "Madera";
        }

        public override void render()
        {
            base.render();
            

        }

      public bool esConsumible()
        {
            return true;
        }


    }
}
