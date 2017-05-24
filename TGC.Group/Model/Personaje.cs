using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.SceneLoader;
using TGC.Core.Utils;
using TGC.Group.Camara;
using TGC.Group.Hud;

namespace TGC.Group.Model
{
    public class Personaje
    {
        public int vida { get; set; }
        public int hambre { get; set; }
        public int sed { get; set; }
        public int energia { get; set; }
        private List<Recurso> recursos;
        public Hud.Hud hud;
        public String mediaDir { get; set; }
        private Vector3 _userPosition;
        public TgcFpsCamera cam { get; set; }    
        public CustomSprite hachaSprite;
        public Drawer2D drawer2D;

        public Vector3 userPosition {
            get { return _userPosition; }
            set {                
                    _userPosition = value;  
            } }

        public Personaje(Hud.Hud hud, String dir)
        {
            recursos = new List<Recurso>();
            this.hud = hud;
            this.mediaDir = dir;
            drawer2D = new Drawer2D();
            cargarHacha();
        }      
        
       
    
        public bool inventarioLleno()
        {
            var contadorSlotsLlenos = 0;
            foreach(var container in hud.itemsContainerSprite)
            {
                if (!container.estaDisponible) contadorSlotsLlenos++;
            }

            return contadorSlotsLlenos >= 6;
        }


        public void cargarHacha() 
        {
            hachaSprite = new CustomSprite();
            hachaSprite.Bitmap = new CustomBitmap(mediaDir + "MeshCreator\\Meshes\\Armas\\Hacha\\Textures\\axe1.png", D3DDevice.Instance.Device);
            var textureSize = new Size(30, 30);
            var width = D3DDevice.Instance.Width;
            var height = D3DDevice.Instance.Height;
            int p = (int) (height * 0.2);
            hachaSprite.Scaling = new Vector2(0.7f, 0.7f);
            hachaSprite.Position = new Vector2(width / 2, (height/2) +p);           

        }


        public void render()
        {
            drawer2D.BeginDrawSprite();
            drawer2D.DrawSprite(hachaSprite);

            drawer2D.EndDrawSprite();
        }

        public void quitarRecurso(Recurso recurso)
        {
            var index = -1;

            for(int i =0; i< recursos.Count;i++)
            {
                if(recursos[i] == recurso)
                {
                    index = i;
                }
            }

            if(index != -1)
            {
                recursos.RemoveAt(index);
            }
        }

        public void agregarRecurso(Recurso recurso)
        {
            if (inventarioLleno()) throw new Exception("Se quiso agregar un recurso pero el inventario esta lleno");
            recursos.Add(recurso);
            hud.agregarItem(recurso);
        }

    }
}
