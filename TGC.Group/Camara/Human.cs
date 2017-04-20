﻿using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Input;
using TGC.Core.SkeletalAnimation;
using TGC.Core.Utils;
using TGC.Group.InventarioYObjetos;

namespace TGC.Group.Camara
{
    public class Human
    {

        public int type = 0; //For future
        public string description = "Human";
        public int status = 1; //1: New. 2: Used; 3: inInventory; 4: Usign; 5: Disabled;
        public int health;// = 100;
        public int agua;
        public int suenio;
        public float minimumDistance = 100; //Default
        private TgcSkeletalMesh mesh;
        public Inventory inventory;      
        //private DateTime tActual;
        //private DateTime tAnterior;
        private float tTranscurridoVida = 0;
        private float tTranscurridoAgua = 0;
        private float tTranscurridoSuenio = 0;

        private String animation = "Walk";
    
        public bool muerto = false;
        Size screenSize; //todo
        Size textureSizeGameOver;
     


     
        public Human(Inventory inventory, TgcSkeletalMesh mesh, Vector3 position, Vector3 scale)
        {
            this.inventory = inventory;
            inventory.setDueño(this);
            this.mesh = mesh;
            this.mesh.Position = position;
            this.mesh.Scale = scale;
            this.health = 101;//101;
            this.agua = 101;// 101;
            this.suenio = -1;
          
        }

        private Vector3 positionBS(Vector3 position)
        {
            return new Vector3(position.X, position.Y + 20, position.Z);
        }


        #region Movimientos //aplica para el personaje en 3era persona

        TgcD3dInput input;
       // ObjectsFactory objectsFactory;
        string animationCaminar = "Walk";
        float velocidadCaminar = 150f;
        //float velocidadRotacion = 100f;
        //Calcular proxima posicion de personaje segun Input
        float moveForward = 0f;
        float rotate = 0;
        bool moving = false;
        bool rotating = false;
        float jump = 0;
        public const float DEFAULT_ROTATION_SPEED = 2f;

        public void movete(Key input, float rotAngle, float elapsedTimeSec) //heading = rotAngle
        {
            //this.move(

            //this.setPosition(vec);
            //Adelante
            if (input == Key.W)
            {
                moveForward = -velocidadCaminar;
                moving = true;
                //cam.getMovementDirection(input);
            }

            //Atras
            if (input == Key.S)
            {
                moveForward = velocidadCaminar;
                moving = true;
            }
         
            //Jump
            if (input == Key.Space)
            {
                jump = 50;
                moving = true;
            }

      
            rotAngle *= DEFAULT_ROTATION_SPEED * elapsedTimeSec;
            this.rotateY(rotAngle);
     //       GuiController.Instance.ThirdPersonCamera.rotateY(rotAngle);
     


            //Vector de movimiento
            Vector3 movementVector = Vector3.Empty;
            if (moving)
            {
                //Aplicar movimiento, desplazarse en base a la rotacion actual del personaje
                movementVector = new Vector3(
                    FastMath.Sin(this.getRotation().Y) * moveForward,
                    jump,
                    FastMath.Cos(this.getRotation().Y) * moveForward);
                this.move(movementVector);

                //this.playAnimation(animationCaminar, true);
                //this.updateAnimation();
                moving = false;
            }

        }

        public void recuperarVida(int v)
        {
            this.health = health + v;
        }
        #endregion Movimientos

        public void recalcularStats(float elapsedTime)
        {
            tTranscurridoVida += elapsedTime;
            tTranscurridoAgua += elapsedTime;
            tTranscurridoSuenio += elapsedTime;

            if (tTranscurridoVida > 6)
            {
                this.health = health - 2;
                tTranscurridoVida = 0;
           
            }
            if (tTranscurridoAgua > 12)
            {
                this.agua = this.agua - 2;
                tTranscurridoAgua = 0;
            }
            if (tTranscurridoSuenio > 30)
            {
                this.suenio = this.suenio + 3;
                tTranscurridoSuenio = 0;
            }

            if (this.agua < 1 || this.suenio > 99 || this.health < 1) this.muerto = true;
        }

        public void morite()
        {
            //GuiController.Instance.Drawer2D.beginDrawSprite();
       
            //GuiController.Instance.Drawer2D.endDrawSprite();
            //TODO
        }

        public void meshRender()
        {
            this.mesh.render();
        }


        public void leaveObject()
        {
           //TODO
           
        }

        public void render() //hay otro "renderMesh" para el mesh
        {
            if (this.muerto)
            {          
                this.morite();
            }
        }

        public void store()
        {
          
        }

        public void move(Vector3 movement)
        {
            this.mesh.move(movement);
            //this.playAnimation(animation, true);
            //this.updateAnimation();

        }

        public void scale(Vector3 scale)
        {
            this.mesh.Scale = scale;
        }

        public Vector3 getPosition()
        {
            return this.mesh.Position;
        }

        public void setPosition(Vector3 position)
        {
            this.mesh.Position = position;
        }

        public virtual float getMinimumDistance()
        {
            return this.minimumDistance;
        }

        public void rotateX(float angle)
        {
            this.mesh.rotateX(angle);
        }

        public void rotateY(float angle)
        {
            this.mesh.rotateY(angle);
        }

        public void rotateZ(float angle)
        {
            this.mesh.rotateZ(angle);
        }

        public Vector3 getRotation()
        {
            return this.mesh.Rotation;
        }


        public void playAnimation(string animation, bool playLoop)
        {
            this.mesh.playAnimation(animation, playLoop);
        }
        public void updateAnimation()
        {
  
        }
        public void dispose()
        {
            this.mesh.dispose();
        

        }     
         
        public void Render()
        {
      
        }

        public int getHealth()
        {
            return health;
        }
        public void causarDaño(int daño)
        {
            health = health - daño;
        }
    }
}
