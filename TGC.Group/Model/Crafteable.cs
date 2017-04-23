using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.SceneLoader;
using TGC.Core.Text;
using TGC.Group.Camara;

namespace TGC.Group.Model
{
    abstract class Crafteable
    {
        public bool destruirse { get; set; }
        protected float probabilidadAcierto = 0.3f;
        protected float probabilidadObtenerMadera = 0.5f;
        public TgcMesh mesh { get; set; }
        public Vector3 Position { get { return mesh.Position; } }
        protected TgcText2D mensaje;
        protected bool mostrarMensaje;
        protected float contadorMensajeVisible;
        protected float timer;
        protected Point screenCenter;


        public  Crafteable()
        {
            screenCenter = new Point(
                D3DDevice.Instance.Device.Viewport.Width / 2,
                D3DDevice.Instance.Device.Viewport.Height / 2);
        }


        public Recurso consumir()
        {
            if(timer < 0.1)
            {
                timer = 0;
                return null;
            }
            Random r = new Random();
            var randomResult = r.NextDouble();
            mostrarMensaje = true;
            contadorMensajeVisible = 0f;
            if (randomResult <= probabilidadAcierto)
            {

                //el objeto se destruira, ahora decido si le doy madera o no al jugador
                destruirse = true;
                var prob = r.NextDouble();
                if (prob <= probabilidadObtenerMadera)
                {
                    GuiController.Instance.mensaje.Text = "Obtuviste Madera!";
                    GuiController.Instance.mensaje.Color = Color.Gold;
                    mostrarMensaje = true;
                    return dameTuRecurso();
                }else 
                {
                
                    GuiController.Instance.mensaje.Text = "No has obtenido Madera!";
                    GuiController.Instance.mensaje.Color = Color.Red;
                    GuiController.Instance.
                    mostrarMensaje = true;
                    return null;
                }
                
            }
            else
            {
                GuiController.Instance.mensaje.Text = "Le erraste feo!";
                GuiController.Instance.mensaje.Color = Color.BlueViolet;
                mostrarMensaje = true;
                //el objeto no se destruira y no devuelve madera
                destruirse = false;
                return null;
            }

        }

        public virtual void update()
        {
            timer += GuiController.Instance.ElapsedTime;
            if (mostrarMensaje)
            {
                GuiController.Instance.setMostrarMensaje(true);
            }
            mostrarMensaje = false;
        }

        public virtual void render()
        {
            mesh.render();
        }

       public abstract Recurso dameTuRecurso();
    }
}
