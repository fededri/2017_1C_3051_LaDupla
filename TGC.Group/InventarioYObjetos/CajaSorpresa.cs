using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Geometry;
using TGC.Core.SceneLoader;
using TGC.Group.Model;

namespace TGC.Group.InventarioYObjetos
{
    class CajaSorpresa : Crafteable
    {
        public TgcBox box { get; set; }

        public override string nombreRecurso()
        {
            return "Caja Sorpresa";
        } 

        public CajaSorpresa(TgcBox box)
        {
            this.box = box;
            this.mesh = box.toMesh("caja");

        }

        public override Recurso dameTuRecurso()
        {
            return new Motor();
        }
    }
}
