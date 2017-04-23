using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Text;

namespace TGC.Group.Camara
{
    class GuiController
    {
        private static GuiController instance;
        public  TgcText2D mensaje { get; set; }
        public float timerMensaje { get; set; }
        public bool mostrarMensaje;
        public bool getMostrarMensaje()
        {
            return mostrarMensaje;
        }
          
        public void setMostrarMensaje(bool value)
        {
            mostrarMensaje = value;
            timerMensaje = 0f;
        }

         
        

        private GuiController() { }

        public static GuiController Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new GuiController();
                    instance.mensaje = new TgcText2D();
                    instance.mensaje.Color = Color.BlueViolet;
                    instance.mensaje.Position = new Point(500, 500);
                    instance.mensaje.Size = new Size(500, 200);
                    instance.mensaje.changeFont(new Font("TimesNewRoman", 25, FontStyle.Bold | FontStyle.Italic));
                }
                return instance;
            }
        }

        public D3DDevice D3dDevice { get; set; }

        public float ElapsedTime { get; set; }

        public TgcD3dInput D3dInput { get; set; }

    }
}
