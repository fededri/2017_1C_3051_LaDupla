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
        private float timerClima; //timer, cada X tiempo cambiamos el clima y horario
        int maniana = 0;
        int tarde = 1;
        int noche = 2;
        public int horarioActual { get; set; }
        public bool bloquearAvance { get; set; }


        //agrgar time segundos a timerClima
        public void agregartiempoAtimerClima(float time)
        {
            timerClima += time;
            if(timerClima >= (10)) // 5 minutos
            {
                if (horarioActual == noche) horarioActual = maniana;
                else horarioActual += 1;
                timerClima = 0;
            }
        }

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
                    instance.mensaje.Align = TgcText2D.TextAlign.LEFT;
                    instance.mensaje.Color = Color.BlueViolet;
                    instance.mensaje.Position = new Point(D3DDevice.Instance.Width /2, D3DDevice.Instance.Height /2);
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
