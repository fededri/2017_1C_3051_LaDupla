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
       
        private List<Recurso> recursos;
        public Hud.Hud hud;
        public String mediaDir { get; set; }
        private Vector3 _userPosition;
        public TgcFpsCamera cam { get; set; }
        public CustomSprite hachaSprite;
        public Drawer2D drawer2D;
        private float TIEMPO_REDUCIR_HAMBRE = 10, TIEMPO_REDUCIR_BEBIDA = 20, TIEMPO_REDUCIR_VIDA = 5; //segundos
        private float TIEMPO_AUMENTAR_ENERGIA = 5;
        private float _timerEnergia;

        private int _hambre, _sed,_vida,_energia;


        public float timerEnergia { get { return _timerEnergia; }
        set { _timerEnergia = value;
                if(timerEnergia >= TIEMPO_AUMENTAR_ENERGIA)
                {
                    if(sed > 0 && hambre > 0 && energia != 100)
                    energia += 5;

                    _timerEnergia = 0;
                }
            }
        }

        public int vida { get { return _vida; }
        set { _vida = value;
                hud.actualizarStatusPersonaje();
            }
        }

        public int energia
        {
            get { return _energia; }
            set
            {
                _energia = value;
                if (_energia < 0) _energia = 0;
                hud.actualizarStatusPersonaje();
            }
        }

        public int hambre { get { return _hambre; }
            set {
                _hambre = value;
                hud.actualizarStatusPersonaje();
            }

        }
        public int sed { get { return _sed; }
            set {
                _sed = value;
                hud.actualizarStatusPersonaje();
                    
                }
        }
       

        private float _timerSed, _timerHambre, _timerVida;


        public float timerVida { get { return _timerVida; } set { _timerVida = value;
                if(timerVida >= TIEMPO_REDUCIR_VIDA)
                {
                    if (vida >= 10) vida -= 10;
                    else vida = 0; //PERSONAJE MUERTO!!
                    //TODO reiniciar juego
                    _timerVida = 0;
                }
               } }

        public float timerSed {get { return _timerSed; } set { _timerSed = value;
                if (timerSed >= TIEMPO_REDUCIR_BEBIDA)
                {
                    if (sed >= 10) sed -= 10;
                    else sed = 0;                 

                    _timerSed = 0;
                }
            }
        }


        public float timerHambre
        {
            get { return _timerHambre; }
            set
            {
                _timerHambre = value;
                if (timerHambre >= TIEMPO_REDUCIR_HAMBRE)
                {
                    if (hambre >= 10) hambre -= 10;
                    else hambre = 0;

                    _timerHambre = 0;

                }
            }
        }


      

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
            vida = 110;
            sed = 110;
            hambre = 110;
            energia = 100;
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


        #region agregarYRemoverRecursos
        public void quitarRecurso(Recurso recurso)
        {
            var index = -1;

            for (int i = 0; i < recursos.Count; i++)
            {
                if (recursos[i] == recurso)
                {
                    index = i;
                }
            }

            if (index != -1)
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
        #endregion



    }
}
