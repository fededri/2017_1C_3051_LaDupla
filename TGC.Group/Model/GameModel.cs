using Microsoft.DirectX;
using Microsoft.DirectX.DirectInput;
using System.Drawing;
using TGC.Core.Direct3D;
using TGC.Core.Example;
using TGC.Core.Geometry;
using TGC.Core.Input;
using TGC.Core.SceneLoader;
using TGC.Core.Textures;
using TGC.Core.Utils;
using TGC.Core.Camara;
using TGC.Group.Camara;
using System;
using System.Collections.Generic;

namespace TGC.Group.Model
{
    /// <summary>
    ///     Ejemplo para implementar el TP.
    ///     Inicialmente puede ser renombrado o copiado para hacer más ejemplos chicos, en el caso de copiar para que se
    ///     ejecute el nuevo ejemplo deben cambiar el modelo que instancia GameForm <see cref="Form.GameForm.InitGraphics()" />
    ///     line 97.
    /// </summary>
    public class GameModel : TgcExample
    {
        /// <summary>
        ///     Constructor del juego.
        /// </summary>
        /// <param name="mediaDir">Ruta donde esta la carpeta con los assets</param>
        /// <param name="shadersDir">Ruta donde esta la carpeta con los shaders</param>
        public GameModel(string mediaDir, string shadersDir) : base(mediaDir, shadersDir)
        {
            Category = Game.Default.Category;
            Name = Game.Default.Name;
            Description = Game.Default.Description;
        }

        private TgcPlane suelo;
        //Caja que se muestra en el ejemplo.
        private TgcBox Box { get; set; }

        private TgcMesh palmera;

        //Mesh de TgcLogo.
        private TgcMesh Mesh { get; set; }

        private TgcPlane planoAgua;

        private List<TgcMesh> palmeras;

        //Boleano para ver si dibujamos el boundingbox
        private bool BoundingBox { get; set; }

        /// <summary>
        ///     Se llama una sola vez, al principio cuando se ejecuta el ejemplo.
        ///     Escribir aquí todo el código de inicialización: cargar modelos, texturas, estructuras de optimización, todo
        ///     procesamiento que podemos pre calcular para nuestro juego.
        ///     Borrar el codigo ejemplo no utilizado.
        /// </summary>
        public override void Init()
        {
            //Device de DirectX para crear primitivas.
            var d3dDevice = D3DDevice.Instance.Device;

            var pisoTexture = TgcTexture.createTexture(D3DDevice.Instance.Device, MediaDir + "Texturas\\pasto.jpg");
            var aguaTexture = TgcTexture.createTexture(D3DDevice.Instance.Device, MediaDir + "Texturas\\agua.bmp");
            suelo = new TgcPlane(new Vector3(-100, 0, -50), new Vector3(6000, 0, 5000), TgcPlane.Orientations.XZplane, pisoTexture, 200f, 200f);
            planoAgua = new TgcPlane(new Vector3(-100, 0, -51), new Vector3(6000, 0, -3000), TgcPlane.Orientations.XZplane, aguaTexture, 20f, 20f);
            var loader = new TgcSceneLoader();
            var pinoScene =
                loader.loadSceneFromFile(MediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
            palmera = pinoScene.Meshes[0];
            initPalmeras();

            Camara = new TgcFpsCamera(new Vector3(900f, 400f, 900f), Input);
        }


        //crea las instancias de las palmeras y las ubica de forma random en el espacio
        private void initPalmeras()
        {
            palmeras = new List<TgcMesh>();
            var rows = 30;
            var cols = 5;
            float offsetX, offsetZ;
            var random = new Random();

            for (var i = 0; i < rows; i++)
            {
                offsetX = random.Next(100, 200);
                for (var j = 0; j < cols; j++)
                {
                    offsetZ = random.Next(50, 1000);
                    //Crear instancia de modelo
                    var instance = palmera.createMeshInstance(palmera.Name + i + "_" + j);
                    instance.AutoTransformEnable = true;
                    //Desplazarlo
                    instance.move(i * offsetX, 0, j * offsetZ);
                    //instance.Scale = new Vector3(0.25f, 0.25f, 0.25f);

                    palmeras.Add(instance);
                }
            }
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public override void Update()
        {
            PreUpdate();

            //Capturar Input teclado
            if (Input.keyPressed(Key.F))
            {
                BoundingBox = !BoundingBox;
            }

            //Capturar Input Mouse
            if (Input.buttonUp(TgcD3dInput.MouseButtons.BUTTON_LEFT))
            {
                //Como ejemplo podemos hacer un movimiento simple de la cámara.
              
            }          
        }

        /// <summary>
        ///     Se llama cada vez que hay que refrescar la pantalla.
        ///     Escribir aquí todo el código referido al renderizado.
        ///     Borrar todo lo que no haga falta.
        /// </summary>
        public override void Render()
        {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            PreRender();        

            //Render de BoundingBox, muy útil para debug de colisiones.
            if (BoundingBox)
            {                
                //Box.BoundingBox.render();
               // Mesh.BoundingBox.render();
            }

            suelo.render();
            planoAgua.render();
        
            foreach (var mesh in palmeras)
            {
                mesh.render();
            }
         

            //Finaliza el render y presenta en pantalla, al igual que el preRender se debe para casos puntuales es mejor utilizar a mano las operaciones de EndScene y PresentScene
            PostRender();
        }

        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            suelo.dispose();
            palmera.dispose(); //solo se necesita dispose del pino original
        }
    }
}