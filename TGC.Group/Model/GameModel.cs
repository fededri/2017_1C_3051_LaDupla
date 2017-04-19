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

        private TgcMesh rocaOriginal;

        private TgcPlane planoAgua;
        private TgcPlane planoTransicionPastoAgua;
        private List<TgcPlane> esquinasArena;

        private TgcTexture arenaTexture;

        private List<TgcMesh> palmeras;
        private List<TgcMesh> rocas;
        private List<TgcPlane> transicionesPastoArena;
        private List<TgcMesh> arbustos;
       
        TgcTexture transicionPastoArenaRightTexture,transicionPastoArenaDownTexture, transicionPastoArenaUpTexture, transicionPastoArenaLeftTexture;
        private const int planoTransicionPastoArenaAncho = 500;
        private const int anchoIsla = 30000;
        private const int altoIsla = 30000;
        private TgcMesh arbusto;

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
             var pisoTexture = TgcTexture.createTexture(D3DDevice.Instance.Device, MediaDir + "Isla\\Textures\\pasto.jpg");
            var aguaTexture = TgcTexture.createTexture(D3DDevice.Instance.Device, MediaDir + "Isla\\Textures\\agua 10.jpg");
            transicionPastoArenaRightTexture = TgcTexture.createTexture(d3dDevice, MediaDir + "\\Isla\\Textures\\TransicionPastoArenaRight.jpg");
            transicionPastoArenaDownTexture = TgcTexture.createTexture(d3dDevice, MediaDir + "\\Isla\\Textures\\TransicionPastoArenaDown.jpg");
            transicionPastoArenaUpTexture = TgcTexture.createTexture(d3dDevice, MediaDir + "\\Isla\\Textures\\TransicionPastoArenaUP.jpg");
            transicionPastoArenaLeftTexture = TgcTexture.createTexture(d3dDevice, MediaDir + "\\Isla\\Textures\\TransicionPastoArenaLeft.jpg");

            arenaTexture = TgcTexture.createTexture(d3dDevice, MediaDir + "Isla\\Textures\\sand.jpg");

            suelo = new TgcPlane(new Vector3(0, 0, 0), new Vector3(anchoIsla, 0, altoIsla), TgcPlane.Orientations.XZplane, pisoTexture, 50f,50f);
            planoTransicionPastoAgua = new TgcPlane(new Vector3(0, 0,0), new Vector3(10, 0, -altoTransicionPastoArena), TgcPlane.Orientations.XZplane, transicionPastoArenaRightTexture, 1f, 1f);
            planoAgua = new TgcPlane(new Vector3(-1000, 0, -altoTransicionPastoArena), new Vector3(anchoIsla + 3000, 0, -3000), TgcPlane.Orientations.XZplane, aguaTexture, 1f, 1f);

          
            var loader = new TgcSceneLoader();
            var palmeraScene =
                loader.loadSceneFromFile(MediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
            var arbustoScene = loader.loadSceneFromFile(MediaDir + "\\MeshCreator\\Meshes\\Vegetacion\\Planta2\\Planta2-TgcScene.xml");
            arbusto = arbustoScene.Meshes[0];
            palmera = palmeraScene.Meshes[0];

     

            var rocaScene = loader.loadSceneFromFile(MediaDir + "MeshCreator\\Meshes\\Vegetacion\\Roca\\Roca-TgcScene.xml");
            rocaOriginal = rocaScene.Meshes[0];
            initPalmeras();
            initRocas();
            initTransicionPastoArena();
            initArbustos();
            Camara = new TgcFpsCamera(new Vector3(30000f, 10f, 0), Input);
        }


        //crea las instancias de las palmeras y las ubica de forma random en el espacio
        private void initPalmeras()
        {
            palmeras = new List<TgcMesh>();        
            float offsetX, offsetZ;
            var random = new Random();

            for(var i = 0; i < 500; i++)
            {
                offsetX = random.Next(100, 29900);
                offsetZ = random.Next(100, 29900);
                var instance = palmera.createMeshInstance(palmera.Name + i);
                instance.AutoTransformEnable = true;
                instance.move(offsetX, 0, offsetZ);
             
               instance.Scale = new Vector3(3f, 5f, 3f);
                palmeras.Add(instance);
            }     
        }


        private void initRocas()
        {
            rocas = new List<TgcMesh>();
            var random = new Random();
            for (var i = 0; i < 100; i++)
            {
                var offsetX = random.Next(1, 28000) + random.Next(100);
                var offsetZ = random.Next(1, 28000) + random.Next(100);
                var instance = rocaOriginal.createMeshInstance(rocaOriginal.Name + "i");

                instance.AutoTransformEnable = true;
                instance.move(offsetX, 0, offsetZ);
                instance.Scale = new Vector3(5f, 2f, 5f);
                rocas.Add(instance);
           
            }

        }


        private void initArbustos()
        {
            arbustos = new List<TgcMesh>();
            var random = new Random();
            for(var i = 0; i<500; i++)
            {
                var offsetX = random.Next(1, 29900);
                var offsetZ = random.Next(300, 29900);
                var instance = arbusto.createMeshInstance(arbusto.Name + "i");
                instance.AutoTransformEnable = true;

                instance.move(offsetX, 0, offsetZ);
                if (random.Next(0, 2) == 1) { instance.Scale = new Vector3(5f, 5f, 5f); }
                else instance.Scale = new Vector3(1f, 1f, 1f);
                arbustos.Add(instance);
            }
        }

        private const int altoTransicionPastoArena = 200;

        private void initTransicionPastoArena()
        {
            transicionesPastoArena = new List<TgcPlane>();

            //x+ z-
            int startXPosition = 0;
            while((startXPosition + planoTransicionPastoArenaAncho) <= (anchoIsla))
            {
                var plane = new TgcPlane(new Vector3(startXPosition, 0, 0), new Vector3(planoTransicionPastoArenaAncho, 0, -altoTransicionPastoArena), TgcPlane.Orientations.XZplane, transicionPastoArenaRightTexture, 2f, 1f);                
                transicionesPastoArena.Add(plane);
                startXPosition += planoTransicionPastoArenaAncho;
            }

            //x fijo z+
            int startZPosition = -85;
            var arena = new TgcPlane(new Vector3(-altoTransicionPastoArena, 0, -altoTransicionPastoArena), new Vector3(200, 0, 115), TgcPlane.Orientations.XZplane, arenaTexture, 1f, 2f);
            esquinasArena = new List<TgcPlane>();
            esquinasArena.Add(arena);
            while ( startZPosition + planoTransicionPastoArenaAncho <= altoIsla)
            {
                var plane = new TgcPlane(new Vector3(-altoTransicionPastoArena, 0, startZPosition), new Vector3(altoTransicionPastoArena, 0, planoTransicionPastoArenaAncho), TgcPlane.Orientations.XZplane, 
                            transicionPastoArenaDownTexture, 1f, 2f);
                transicionesPastoArena.Add(plane);
                startZPosition += planoTransicionPastoArenaAncho;
            }

            //x+ z +
            startXPosition = 0;
            arena = new TgcPlane(new Vector3(-(altoTransicionPastoArena), 0, altoIsla - 85), new Vector3(200, 0, 335), TgcPlane.Orientations.XZplane, arenaTexture, 1f, 2f);
            esquinasArena.Add(arena);
            while ((startXPosition + planoTransicionPastoArenaAncho) < (anchoIsla))
            {
                var plane = new TgcPlane(new Vector3(startXPosition, 0, altoIsla), new Vector3(planoTransicionPastoArenaAncho + 200, 0, altoTransicionPastoArena +50), TgcPlane.Orientations.XZplane, transicionPastoArenaRightTexture, 2f, 1f);
                transicionesPastoArena.Add(plane);
                startXPosition += planoTransicionPastoArenaAncho;
            }

            //x a la derecha fijo z +
            startZPosition = -85;
            arena = new TgcPlane(new Vector3(anchoIsla , 0, altoIsla -85), new Vector3(200, 0, 340), TgcPlane.Orientations.XZplane, arenaTexture, 1f, 2f);
            esquinasArena.Add(arena);
            arena = new TgcPlane(new Vector3(anchoIsla -300, 0, altoIsla), new Vector3(300, 0, 250), TgcPlane.Orientations.XZplane, arenaTexture, 1f, 2f);
            esquinasArena.Add(arena);
            while (startZPosition + planoTransicionPastoArenaAncho <= (altoIsla))
            {
                var plane = new TgcPlane(new Vector3(anchoIsla, 0, startZPosition), new Vector3(altoTransicionPastoArena, 0, planoTransicionPastoArenaAncho), TgcPlane.Orientations.XZplane,
                            transicionPastoArenaUpTexture, 1f, 2f);
                transicionesPastoArena.Add(plane);
                startZPosition += planoTransicionPastoArenaAncho;
            }

            arena = new TgcPlane(new Vector3(anchoIsla, 0,- altoTransicionPastoArena), new Vector3(200, 0, 120), TgcPlane.Orientations.XZplane, arenaTexture, 1f, 2f);
            esquinasArena.Add(arena);

            /* int startZPosition = 0;
             while ((startZPosition + planoTransicionPastoArenaAncho) < 5000)
             {
                 var plane = new TgcPlane(new Vector3(startXPosition, 0, startZPosition), new Vector3(altoTransicionPastoArena, 0, planoTransicionPastoArenaAncho), TgcPlane.Orientations.XZplane, transicionPastoArenaUpTexture, 1f, 1f);
                 transicionesPastoArena.Add(plane);
                 startZPosition += planoTransicionPastoArenaAncho;
             }*/


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
        
            //renderizado de palmeras
            foreach (var mesh in palmeras)
            {
                mesh.render();
            }

            //renderizado de rocas
            foreach  ( var mesh in rocas)
            {
                mesh.render();
            }

            planoTransicionPastoAgua.render();

            foreach ( var mesh in transicionesPastoArena)
            {
                mesh.render();
            }

            foreach ( var mesh in arbustos)
            {
                mesh.render();
            }

            foreach( var arena in esquinasArena)
            {
                arena.render();
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
            palmera.dispose();
            rocaOriginal.dispose();
            transicionPastoArenaRightTexture.dispose();
            transicionPastoArenaDownTexture.dispose();
            transicionPastoArenaLeftTexture.dispose();
            transicionPastoArenaUpTexture.dispose();
            arbusto.dispose();
            arenaTexture.dispose();
        }
    }
}