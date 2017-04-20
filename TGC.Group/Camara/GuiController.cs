using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Input;

namespace TGC.Group.Camara
{
    class GuiController
    {
        private static GuiController instance;
        private GuiController() { }

        public static GuiController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GuiController();
                    instance.D3dInput = new TgcD3dInput();
                }
                return instance;
            }
        }

        public D3DDevice D3dDevice { get; set; }

        public float ElapsedTime { get; set; }

        public TgcD3dInput D3dInput { get; set; }

    }
}
