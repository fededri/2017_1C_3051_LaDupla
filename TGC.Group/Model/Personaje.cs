using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.SceneLoader;
using TGC.Core.Utils;
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
        private float previousRot;
        public float previousOffsetZ, previousOffsetX;
        private float elapsedTime;
        public Vector3 userPosition {
            get { return _userPosition; }
            set {
                
                    _userPosition = value;
                    updateHacha();              
            } }

        public Personaje(Hud.Hud hud, String dir)
        {
            recursos = new List<Recurso>();
            this.hud = hud;
            this.mediaDir = dir;
            cargarHacha();
        }
       
        public void update(float t)
        {
            elapsedTime = t;
            userPosition = cam.positionEye;
            previousRotated = cam.rotated;
            

        }

        private void updateHacha()
        {

            Vector3 moveVector = new Vector3(0, 0, 0);
            var cosenoAlpha = Vector3.Dot(userPosition, cam.rotated) / ((Vector3.Length(userPosition) * Vector3.Length(cam.rotated)));
            var alpha = FastMath.Acos(cosenoAlpha);

            Console.WriteLine("Se roto en un agulo de: " + alpha);
            var rotation = Matrix.RotationYawPitchRoll(alpha, 0, 0);

            /*
            var translation = Matrix.Translation(moveVector);
            var movimientoFinal = translation;

            var vec4 = Vector3.Transform(hachaInstance.Position, movimientoFinal);*/

            float offsetX = 0, offsetZ = 0;

            var substract = Vector3.Subtract(cam.LookAt, userPosition);

            if (substract.X > 0)
            {
                offsetX = 10;
                // if (previousOffsetX < 0)
                //   hachaInstance.rotateY((float)Math.PI);
            }
            else
            {
                offsetX = -10;
                /* if (previousOffsetX > 0)
                     hachaInstance.rotateY((float)Math.PI);*/
            }

            if (substract.Z > 0)
            {
                offsetZ = 10;
                /*  if(previousOffsetZ < 0)
                  hachaInstance.rotateY((float)Math.PI);*/

            }
            else
            {
                offsetZ = -10;
                /* if(previousOffsetZ > 0)
                 hachaInstance.rotateY(-(float)Math.PI);*/

            }
            previousOffsetZ = offsetZ;
            previousOffsetX = offsetX;

            hachaInstance.Position = new Vector3(cam.LookAt.X + offsetX, userPosition.Y - 50, cam.LookAt.Z + offsetZ);

            
            if( previousRot != cam.rightleftRot && previousRot != cam.leftrightRot)
            {
                if (cam.rightleftRot > 0 && cam.leftrightRot > 0) return;
                double rot = 0;
                if(cam.rightleftRot > 0 || 1==1)
                {
                    rot = cam.rightleftRot * elapsedTime;
                    previousRot = cam.rightleftRot;                  
                } else if(cam.leftrightRot > 0)
                {
                    rot = cam.leftrightRot * elapsedTime;
                    previousRot = cam.leftrightRot;
                }
              
                
               // hachaInstance.rotateY((float)rot);  
            }
            

                     
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
            instance.Scale = new Vector3(3f, 3f, 3f);
            instance.move(-30, 150f, 10);            
            hachaInstance = instance;
            hachaInstance.rotateY((float)(FastMath.PI));

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
