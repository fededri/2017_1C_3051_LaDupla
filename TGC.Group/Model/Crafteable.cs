﻿using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
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
        protected float probabilidadObtenerRecurso = 0.5f;
        public TgcMesh mesh { get; set; }
        public Vector3 Position { get { return mesh.Position; } }
        protected TgcText2D mensaje;
        protected bool mostrarMensaje;
        protected float contadorMensajeVisible;
        protected float timer;
        protected Point screenCenter;
        public TgcBoundingCylinder cilindro { get; set; }
        public TgcBoundingSphere esfera { get; set; }
        public Vector3 position { get; set; }


        public abstract string nombreRecurso();

        public  Crafteable()
        {
            screenCenter = new Point(
                D3DDevice.Instance.Device.Viewport.Width / 2,
                D3DDevice.Instance.Device.Viewport.Height / 2);
        }

        public void move(Vector3 movement)
        {
           this.mesh.Position = mesh.Position + movement;
            if(cilindro != null) cilindro.Center = mesh.Position;
            if (esfera != null) esfera = new TgcBoundingSphere(mesh.Position, 180);
        }

        public Recurso consumir(bool inventarioLleno)
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
                if (prob <= probabilidadObtenerRecurso)
                {
                    if (!inventarioLleno)
                    {
                        Gui.Instance.mensaje.Text = "Obtuviste " + nombreRecurso();
                    }
                    else Gui.Instance.mensaje.Text = "Inventario lleno!";
                    Gui.Instance.mensaje.Color = Color.Gold;
                    mostrarMensaje = true;
                    return dameTuRecurso();
                }else 
                {
                
                    Gui.Instance.mensaje.Text = "No has obtenido " + nombreRecurso();
                    Gui.Instance.mensaje.Color = Color.Red;
                    Gui.Instance.
                    mostrarMensaje = true;
                    return null;
                }
                
            }
            else
            {
                Gui.Instance.mensaje.Text = "Le erraste!";
                Gui.Instance.mensaje.Color = Color.BlueViolet;
                mostrarMensaje = true;
                //el objeto no se destruira y no devuelve madera
                destruirse = false;
                return null;
            }

        }

        public virtual void update()
        {
            timer += Gui.Instance.ElapsedTime;
            if (mostrarMensaje)
            {
                Gui.Instance.setMostrarMensaje(true);
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
