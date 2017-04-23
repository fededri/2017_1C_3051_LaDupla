using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using TGC.Group.Model;

namespace TGC.Group.InventarioYObjetos
{
    class Roca : Crafteable
    {

        public Roca(TgcMesh mesh)
        {
            this.mesh = mesh;
        }

        public override Recurso dameTuRecurso()
        {
            return new Piedra();
        }
    }
}
