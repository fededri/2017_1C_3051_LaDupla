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
    class Gui
    {
        private static Gui instance;
        public  TgcText2D mensaje { get; set; }
        public float timerMensaje { get; set; }
        public bool mostrarMensaje;
        private float timerHorario; //timer, cada X tiempo cambiamos el clima y horario       
        public int horarioActual { get; set; }
        public bool bloquearAvance { get; set; }
        public int horaDelDia { get; set; }


        //agrgar time segundos a timerClima
        public void agregarTiempoHorario(float time)
        {

            if(timerHorario > 10)
            {
                timerHorario = 0;
                if (horaDelDia < 3)
                {
                    horaDelDia++;
                }
                else
                {
                    horaDelDia = 0; 
                }
            }else
            timerHorario += time;
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

         
        

        private Gui() { }

        public static Gui Instance
        {
            get
            {
                if (instance == null)
                {
                    instance = new Gui();
                    instance.mensaje = new TgcText2D();
                    instance.mensaje.Align = TgcText2D.TextAlign.LEFT;
                    instance.mensaje.Color = Color.BlueViolet;
                    instance.mensaje.Position = new Point(D3DDevice.Instance.Width /2, D3DDevice.Instance.Height /2);
                    instance.mensaje.Size = new Size(500, 200);
                    instance.mensaje.changeFont(new Font("TimesNewRoman", 25, FontStyle.Bold));
                }
                return instance;
            }
        }

        public D3DDevice D3dDevice { get; set; }

        public float ElapsedTime { get; set; }

        public TgcD3dInput D3dInput { get; set; }

    }
}
