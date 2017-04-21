using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using TGC.Core.Text;

namespace TGC.Group.InventarioYObjetos
{
    class Palmera
    {
        public bool destruirse { get; set; }
        float probabilidadAcierto = 0.3f;
        float probabilidadObtenerMadera = 0.5f;
        public TgcMesh mesh { get; set; }
        public Vector3 Position { get { return mesh.Position; } }
        TgcText2D mensaje;

        public Palmera(TgcMesh mesh)
        {
            this.mesh = mesh;
            this.destruirse = false;
        }


        public Madera consumir()
        {
            mensaje = new TgcText2D();
            Random r = new Random();
            var  randomResult = r.NextDouble();
            if(randomResult <= probabilidadAcierto)
            {
                //el objeto se destruira, ahora decido si le doy madera o no al jugador
                destruirse = true;
                var prob = r.NextDouble();
                if (prob <= probabilidadObtenerMadera)
                {
                    return new Madera();
                }
                else return null;
            } else
            {
                mensaje = new TgcText2D();
                mensaje.Text = "Le erraste feo!";
                mensaje.Color = Color.BlueViolet;
                mensaje.Position = new Point(500, 500);
                mensaje.Size = new Size(500, 200);
                mensaje.changeFont(new Font("TimesNewRoman", 25, FontStyle.Bold | FontStyle.Italic));                
                //el objeto no se destruira y no devuelve madera
                destruirse = false;
                return null;
            }

          
        }


       public void render()
        {
            mesh.render();
        }

      public bool esConsumible()
        {
            return true;
        }


    }
}
