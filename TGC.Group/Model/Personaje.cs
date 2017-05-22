using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using TGC.Group.Camara;

namespace TGC.Group.Model
{
    public class Personaje
    {
        int vida { get; set; }
        int hambre { get; set; }
        int sed { get; set; }
        int energia { get; set; }
        private List<Recurso> recursos;
        public Hud.Hud hud;
        private TgcMesh hachaInstance;
        private TgcMesh hacha;
        public String mediaDir { get; set; }
        private Vector3 _userPosition;
        public Vector3 lookAt;
        public TgcFpsCamera cam { get; set; }
        public Vector3 previousRotated { get; set; }
        public Vector3 userPosition {
            get { return _userPosition; }
            set {if(value != _userPosition)
                {
                    _userPosition = value;
                    updateHacha();
                }
              
            } }

        public Personaje(Hud.Hud hud, String dir)
        {
            recursos = new List<Recurso>();
            this.hud = hud;
            this.mediaDir = dir;
            cargarHacha();
        }


        public void update()
        {
            if (previousRotated == cam.rotated) return;
            _userPosition = cam.positionEye;

            previousRotated = cam.rotated;
            

        }

        private void updateHacha()
        {

            Vector3 moveVector = cam.rotated;
            hachaInstance.Position = new Vector3(userPosition.X, userPosition.Y - 50, userPosition.Z);

            if (cam.LookAt.X >0 )
            {
                moveVector.X += 50;
            }else if(cam.LookAt.X <0)
            {
                moveVector.X += -50;
            }

             if (cam.LookAt.Z > 0)
             { 
                 moveVector.Z += -50;
             }
             else moveVector.Z += 50;


            hachaInstance.move(moveVector);
           
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
            var loader = new TgcSceneLoader();
            var hachaScene = loader.loadSceneFromFile(mediaDir + "MeshCreator\\Meshes\\Armas\\Hacha\\Hacha-TgcScene.xml");
            hacha = hachaScene.Meshes[0];
            var instance = hacha.createMeshInstance(hacha.Name);
            instance.AutoTransformEnable = true;
            instance.Scale = new Vector3(5f, 3f, 5f);
            instance.move(-30, 150f, 10);            
            hachaInstance = instance;
            hachaInstance.rotateY((float)Math.PI);

        }


        public void render()
        {
            hachaInstance.render();
        }

        public void agregarRecurso(Recurso recurso)
        {
            if (inventarioLleno()) throw new Exception("Se quiso agregar un recurso pero el inventario esta lleno");
            recursos.Add(recurso);
            hud.agregarItem(recurso);
        }

    }
}
