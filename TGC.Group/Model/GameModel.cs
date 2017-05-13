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
using TGC.Core.Terrain;
using TGC.Group.InventarioYObjetos;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using Microsoft.DirectX.Direct3D;
using TGC.Group.Shaders;

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

        private TgcMesh palmeraMesh;

        //Mesh de TgcLogo.
        private TgcMesh Mesh { get; set; }

        private TgcMesh rocaOriginal;

        private TgcPlane planoAgua;
        private TgcPlane planoTransicionPastoAgua;
        private List<TgcPlane> esquinasArena;

        private TgcTexture arenaTexture;

     
        private List<TgcPlane> transicionesPastoArena;
        private List<TgcMesh> arbustos;
        private List<TgcPlane> planosAgua;
        private TgcSkyBox skyBox;
        private float MAX_DIST_A_OBJ_CONSUMIBLE = 300f;
       
        TgcTexture transicionPastoArenaRightTexture,transicionPastoArenaDownTexture, transicionPastoArenaUpTexture, transicionPastoArenaLeftTexture;
        private const int planoTransicionPastoArenaAncho = 500;
        private const int anchoIsla = 30000;
        private const int altoIsla = 30000;
        private TgcMesh arbustoMesh;
        private TgcTexture aguaTexture;
        private List<Crafteable> objetosABorrar;
        private List<Crafteable> objetos;
        private TgcFpsCamera cam;
        private Hud.Hud hud;
        private Personaje personaje;
        private List<MySimpleTerrain> terrains;
        private MySimpleTerrain currentTerrain;
        private World world;
        private  World[][] worlds;
        private int worldSize = 7500;


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
            skyBox = new TgcSkyBox();
            skyBox.Center = new Vector3(0, 0, 0);
            skyBox.Size = new Vector3(20000, 10000,20000);
            hud = new Hud.Hud(MediaDir + "Hud\\");
            personaje = new Personaje(hud);

            objetosABorrar = new List<Crafteable>();
            objetos = new List<Crafteable>();
            terrains = new List<MySimpleTerrain>();

            var texturesPath = MediaDir + "Texturas\\Quake\\SkyBox LostAtSeaDay\\";
            //Configurar las texturas para cada una de las 6 caras
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "lostatseaday_up.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "lostatseaday_dn.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "lostatseaday_lf.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "lostatseaday_rt.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "lostatseaday_bk.jpg");
            skyBox.setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "lostatseaday_ft.jpg");
            skyBox.SkyEpsilon = 20f;
            skyBox.Init();

            //Device de DirectX para crear primitivas.
            var d3dDevice = D3DDevice.Instance.Device;
             var pisoTexture = TgcTexture.createTexture(D3DDevice.Instance.Device, MediaDir + "Isla\\Textures\\pasto.jpg");
            aguaTexture = TgcTexture.createTexture(D3DDevice.Instance.Device, MediaDir + "Isla\\Textures\\agua 10.jpg");
            transicionPastoArenaRightTexture = TgcTexture.createTexture(d3dDevice, MediaDir + "\\Isla\\Textures\\TransicionPastoArenaRight.jpg");
            transicionPastoArenaDownTexture = TgcTexture.createTexture(d3dDevice, MediaDir + "\\Isla\\Textures\\TransicionPastoArenaDown.jpg");
            transicionPastoArenaUpTexture = TgcTexture.createTexture(d3dDevice, MediaDir + "\\Isla\\Textures\\TransicionPastoArenaUP.jpg");
            transicionPastoArenaLeftTexture = TgcTexture.createTexture(d3dDevice, MediaDir + "\\Isla\\Textures\\TransicionPastoArenaLeft.jpg");
            
            arenaTexture = TgcTexture.createTexture(d3dDevice, MediaDir + "Isla\\Textures\\sand.jpg");

            suelo = new TgcPlane(new Vector3(0, 0, 0), new Vector3(anchoIsla, 0, altoIsla), TgcPlane.Orientations.XZplane, pisoTexture, 50f,50f);
            planoTransicionPastoAgua = new TgcPlane(new Vector3(0, 0,0), new Vector3(10, 0, -altoTransicionPastoArena), TgcPlane.Orientations.XZplane, transicionPastoArenaRightTexture, 1f, 1f);

         
            var loader = new TgcSceneLoader();
            var palmeraScene =
                loader.loadSceneFromFile(MediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
            var arbustoScene = loader.loadSceneFromFile(MediaDir + "\\MeshCreator\\Meshes\\Vegetacion\\Planta2\\Planta2-TgcScene.xml");
            arbustoMesh = arbustoScene.Meshes[0];
            palmeraMesh = palmeraScene.Meshes[0];

            var botellaAgua = new Botella() ;
            botellaAgua.tieneAgua = false;
            personaje.agregarRecurso(botellaAgua);     

            var rocaScene = loader.loadSceneFromFile(MediaDir + "MeshCreator\\Meshes\\Vegetacion\\Roca\\Roca-TgcScene.xml");
            rocaOriginal = rocaScene.Meshes[0];


            //loadTerrains();
            loadWorld();
            //initPalmeras();
            //initRocas();
            //initTransicionPastoArena();
            //initArbustos();
            //initAgua();
            
            cam = new TgcFpsCamera(new Vector3(0, 100f, 0), Input);
            Camara = cam;

            GuiController.Instance.D3dInput = Input;           
            ///Camara.SetCamera(human.getPosition(), human.getPosition() + new Vector3(50f, 0, 0));
        

        }

        private void loadWorld()
        {
            worlds = new World[3][];
            worlds[0] = new World[3];
            worlds[1] = new World[3];
            worlds[2] = new World[3];

            int off = 0;

            worlds[0][0] = new World(new Vector3(-(worldSize - off), 0, worldSize - off), worldSize,MediaDir);
            worlds[0][1] = new World(new Vector3(0, 0, worldSize - off), worldSize, MediaDir);
            worlds[0][2] = new World(new Vector3(worldSize - off, 0, worldSize - off), worldSize, MediaDir);
            worlds[1][1] = new World(new Vector3(0, 0, 0), worldSize - off, MediaDir);
            worlds[1][0] = new World(new Vector3(-(worldSize- off), 0, 0), worldSize, MediaDir);
            worlds[1][2] = new World(new Vector3(worldSize- off, 0, 0), worldSize, MediaDir);
            worlds[2][0] = new World(new Vector3(-(worldSize - off), 0, -worldSize), worldSize, MediaDir);
            worlds[2][1] = new World(new Vector3(0, 0, -(worldSize- off)), worldSize, MediaDir);
            worlds[2][2] = new World(new Vector3(worldSize  - off, 0, -(worldSize- off)), worldSize, MediaDir);



        }

        private void loadTerrains()
        {
            currentTerrain = new MySimpleTerrain();
            currentTerrain.loadHeightmap(MediaDir + "Heighmaps\\Heightmap3.jpg", 45.0550f, 0.5950f,
           new Vector3(0, 0, 0));          
            currentTerrain.loadTexture(MediaDir + "Heighmaps\\grass.jpg");

          /* var terrain = new MySimpleTerrain();
            terrain.loadHeightmap(MediaDir + "Heighmaps\\Heightmap2.jpg", 100f, 1f,
           new Vector3(0, 0, 0));
            terrain.loadTexture(MediaDir + "Heighmaps\\grass.jpg");
            terrains.Add(terrain);

            var instance = palmeraMesh.createMeshInstance(palmeraMesh.Name);
            instance.AutoTransformEnable = true;
            instance.move(3000, currentTerrain.CalcularAltura(0, 0), -3000);

            instance.Scale = new Vector3(3f, 5f, 3f);
  
            var palmera = new Palmera(instance);
            objetos.Add(palmera);*/

            
              int zCounter = 0;
               for(int i = 0;i < 1; i++)
               {
                   int xCounter = 0;
                   for (int j = 0; j < 2; j++)
                   {
                       var terrain = new MySimpleTerrain();                    
                       terrain.loadHeightmap(MediaDir + "Heighmaps\\Heightmap2.jpg", 100f, 1f,
                      new Vector3(xCounter, 0, zCounter));
                       xCounter += 101;
                       terrain.loadTexture(MediaDir + "Heighmaps\\grass.jpg");
                       terrains.Add(terrain);
                    initPalmeras(terrain,j,i);
                    initRocas(terrain,j,i);
                    initArbustos(terrain,j,i);
                }
                zCounter += 62;
            }

        }

        #region inits
        //crea las instancias de las palmeras y las ubica de forma random en el espacio
        private void initPalmeras(MySimpleTerrain terrain,int iteracionJ, int iteracionI)
        {
            Vector3 center = new Vector3(terrain.center.X + (6000 * iteracionJ), terrain.center.Y, terrain.center.Z + (6000 * iteracionI));
            float scaleXZ = terrain.scaleXZ;

            int limiteSupX = (int)center.X + 2500;
            int limiteSupZ = (int)center.Z + 2500;

            int limiteInfX = (int)center.X - 2500;
            int limiteInfZ = (int)center.Z - 2500;

            float offsetX, offsetZ;
            var random = new Random();

            for(var i = 0; i < 20; i++)
            {
                offsetX = random.Next(limiteInfX, limiteSupX);
                offsetZ = random.Next(limiteInfZ, limiteSupZ);
                var instance = palmeraMesh.createMeshInstance(palmeraMesh.Name + i);    
                instance.move(offsetX,terrain.CalcularAltura(offsetX,offsetZ), offsetZ);
             
               instance.Scale = new Vector3(3f, 5f, 3f);
                var collisionableCylinder = new TgcBoundingCylinder(new Vector3(offsetX,0,offsetZ),60,800);
                collisionableCylinder.setRenderColor(Color.Green);
                var palmera = new Palmera(instance);
                palmera.cilindro = collisionableCylinder;
                objetos.Add(palmera);
            }     
        }


        private void initRocas(MySimpleTerrain terrain, int iteracionJ, int iteracionI)
        {
            Vector3 center = new Vector3(terrain.center.X + (6000*iteracionJ),terrain.center.Y,terrain.center.Z +(6000*iteracionI));

            float scaleXZ = terrain.scaleXZ;

            int limiteSupX = (int)center.X + 2500;
            int limiteSupZ = (int)center.Z + 2500;

            int limiteInfX = (int)center.X - 2500;
            int limiteInfZ = (int)center.Z - 2500;

            var random = new Random();
            for (var i = 0; i < 10; i++)
            {
                var offsetX = random.Next(limiteInfX,limiteSupX) + random.Next(100);
                var offsetZ = random.Next(limiteInfZ, limiteSupZ) + random.Next(100);
                var instance = rocaOriginal.createMeshInstance(rocaOriginal.Name + "i");

                instance.AutoTransformEnable = true;
                instance.move(offsetX, terrain.CalcularAltura(offsetX, offsetZ), offsetZ);
                instance.Scale = new Vector3(5f, 2f, 5f);
                var esfera = new TgcBoundingSphere(new Vector3(offsetX,terrain.CalcularAltura(offsetX,offsetZ),offsetZ), 120);
                    
                Roca roca = new Roca(instance);
                roca.esfera = esfera;
                objetos.Add(roca);
           
            }

        }


        private void initArbustos(MySimpleTerrain terrain, int iteracionJ, int iteracionI)
        {
            Vector3 center = new Vector3(terrain.center.X + (6000 * iteracionJ), terrain.center.Y, terrain.center.Z + (6000 * iteracionI));
            float scaleXZ = terrain.scaleXZ;

            int limiteSupX = (int)center.X + 2500;
            int limiteSupZ = (int)center.Z + 2500;

            int limiteInfX = (int)center.X - 2500;
            int limiteInfZ = (int)center.Z - 2500;

            arbustos = new List<TgcMesh>();
            var random = new Random();
            for(var i = 0; i<10; i++)
            {
                var offsetX = random.Next(limiteInfX,limiteSupX);
                var offsetZ = random.Next(limiteInfZ,limiteSupZ);
                var instance = arbustoMesh.createMeshInstance(arbustoMesh.Name + "i");
                instance.AutoTransformEnable = true;

                instance.move(offsetX, terrain.CalcularAltura(offsetX, offsetZ), offsetZ);
                if (random.Next(0, 2) == 1)
                {
                    instance.Scale = new Vector3(5f, 5f, 5f);                
                    Arbusto arbustoObj = new Arbusto(instance);
                    objetos.Add(arbustoObj);
                }
                else
                {
                    instance.Scale = new Vector3(1f, 1f, 1f);
                    Arbusto arbustoObj = new Arbusto(instance);
                    objetos.Add(arbustoObj);
                }
               
            }
        }

        private void initAgua()
        {
            planosAgua = new List<TgcPlane>();

            var plano = new TgcPlane(new Vector3(-planoTransicionPastoArenaAncho, 0, -altoTransicionPastoArena), new Vector3(anchoIsla + 40000, 0, -10000),
                TgcPlane.Orientations.XZplane, aguaTexture, 1f, 1f);
            planosAgua.Add(plano);

            plano = new TgcPlane(new Vector3(-altoTransicionPastoArena, 0,-altoTransicionPastoArena), new Vector3(-10000, 0, altoIsla+10000),
                TgcPlane.Orientations.XZplane, aguaTexture, 1f, 1f);

            planosAgua.Add(plano);

            plano = new TgcPlane(new Vector3(-altoTransicionPastoArena, 0, altoIsla + altoTransicionPastoArena + 50), new Vector3(40000, 0, 40000),
             TgcPlane.Orientations.XZplane, aguaTexture, 1f, 1f);

            planosAgua.Add(plano);

            plano = new TgcPlane(new Vector3(anchoIsla + altoTransicionPastoArena, 0, -altoTransicionPastoArena), new Vector3(40000, 0, 40000),
            TgcPlane.Orientations.XZplane, aguaTexture, 1f, 1f);

            planosAgua.Add(plano);


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

        


        }

        #endregion inits


        //debe chequear si hay un objeto cercano en frente del usuario
        public void checkearObjetoMasCercano(Vector3 position)
        {       
           foreach(var objeto in objetos)
            {
                var distancia = Vector3.Length(objeto.Position - position);
                if (distancia <= MAX_DIST_A_OBJ_CONSUMIBLE && distancia > 0)
                {
                    //consumir palmera
                    bool inventarioLleno = personaje.inventarioLleno();
                   var result = objeto.consumir(inventarioLleno);
                    if (objeto.destruirse) objetosABorrar.Add(objeto);
                    if(result !=null)
                    {
                        //agrego objeto al personaje si tiene espacio
                        if (!inventarioLleno)
                            personaje.agregarRecurso(result);                   
                                               
                    }
                }
            }          
            
        }


        //si el usuario se desplaza y llega a un punto que no hay mas terreno creo un nuevo terrain
       //el terrain tiene 3000 puntos a cada lado desde su centro (cuadrado)
        public void CheckTerrenoSegunPos(Vector3 position)
        {
            foreach(var terrain in terrains)
            {
                Vector3 terrainCentro = new Vector3(terrain.center.X,0,terrain.center.Z);
                                
                    var distancia = Vector3.Length(position - terrainCentro);
                    if(distancia <= 3000)
                    {
                    //estoy en este heightmap
                    if(distancia >= 2900)
                    {
                        var nTerrain = new MySimpleTerrain();
                        nTerrain.loadHeightmap(MediaDir + "Heighmaps\\Heightmap2.jpg", 100f, 1f,
                       new Vector3(terrain.center.X + 20,0,terrain.center.Z + 20));
                        nTerrain.loadTexture(MediaDir + "Heighmaps\\grass.jpg");
                        terrains.Add(nTerrain);
                        break;
                    }
                    
                    }



               }

               
            }
        

        public bool isMyPositionInsideTerrain(MySimpleTerrain terrain, Vector3 position)
        {
            Vector3 terrainCentro = terrain.center;
            return ((position.X <= terrainCentro.X + 3000 && position.Z <= terrainCentro.Z + 3000) ||
                    (position.X >= terrainCentro.X - 3000 && position.Z <= terrainCentro.Z + 3000) ||
                    (position.X <= terrainCentro.X + 3000 && position.Z >= terrainCentro.Z - 3000) ||
                    (position.X >= terrainCentro.X - 3000 && position.Z >= terrainCentro.Z - 3000));
        }

        /// <summary>
        ///     Se llama en cada frame.
        ///     Se debe escribir toda la lógica de computo del modelo, así como también verificar entradas del usuario y reacciones
        ///     ante ellas.
        /// </summary>
        public override void Update()
        {
            PreUpdate();

            GuiController.Instance.ElapsedTime = ElapsedTime;
          
            
            //Capturar Input teclado
            if (Input.keyPressed(Key.F))
            {
                BoundingBox = !BoundingBox;
            }

            if (Input.keyPressed(Key.U))
            {
                checkearObjetoMasCercano(Camara.Position);
            }

          //  updateObjetos();

            D3DDevice.Instance.Device.Transform.Projection =
              Matrix.PerspectiveFovLH(D3DDevice.Instance.FieldOfView,
                  D3DDevice.Instance.AspectRatio,
                  D3DDevice.Instance.ZNearPlaneDistance,
                  D3DDevice.Instance.ZFarPlaneDistance *2f);

            if(cam!= null)
            skyBox.Center = cam.positionEye;

            GuiController.Instance.agregartiempoAtimerClima(ElapsedTime);

            switch (GuiController.Instance.horarioActual)
            {
                case 0:
                    skyBox.Color = Color.Cyan;
                   // skyBox.Init();
                    break;

                case 1:

                    skyBox.Color = Color.OrangeRed;
                   // skyBox.Init();
                    break;

                case 2:
                    skyBox.Color = Color.DarkTurquoise;
                   // skyBox.Init();
                    break;
            }
                     
            foreach (var obj in objetos)
            {
                if(obj.cilindro != null)
                {
                  
                    if (TgcCollisionUtils.testPointCylinder(Camara.Position, obj.cilindro))
                    {
                       obj.cilindro.setRenderColor(Color.Blue);
                        cam.colision = true; 
               
                    }
                } else if(obj.esfera != null)
                {
                    if (TgcCollisionUtils.testPointSphere(obj.esfera, Camara.Position))
                    {
                        cam.colision = true;
                        break;
                    
                    }
                }
            }
            Camara.UpdateCamera(ElapsedTime);
            //CheckTerrenoSegunPos(cam.positionEye);
            //mostrar posicion actual
            string pos = "(" + cam.positionEye.X + ";" + cam.positionEye.Y + ";" + cam.positionEye.Z;
            GuiController.Instance.mensaje.Text = pos;
            GuiController.Instance.mostrarMensaje = true;
            GuiController.Instance.timerMensaje = 0;
        }



        public void updateObjetos()
        {
            foreach(var objeto in objetos)
            {
                objeto.update();
            }
        }

   
        public override void Render()
        {
            //Inicio el render de la escena, para ejemplos simples. Cuando tenemos postprocesado o shaders es mejor realizar las operaciones según nuestra conveniencia.
            PreRender();
            D3DDevice.Instance.Device.Clear(ClearFlags.Target | ClearFlags.ZBuffer, Color.Black, 1.0f, 0);
            //suelo.render();
            /*foreach(var terrain in terrains)
           {
               terrain.render();
           }*/
            worlds[0][0].render();
            worlds[0][1].render();
            worlds[0][1].render();
            worlds[1][0].render();
            worlds[1][1].render();
            worlds[1][2].render();
            worlds[2][0].render();
            worlds[2][1].render();
            worlds[2][2].render();


            hud.render();
            
        
          
            foreach (var objeto in objetos)
            {
                objeto.render();
                if (BoundingBox)
                    objeto.mesh.BoundingBox.render();
        
           }

            //borrado de objetos
            foreach (var crafteable in objetosABorrar)
            {
                if (crafteable.destruirse)
                {
                    objetos.Remove(crafteable);
                }
            }
            objetosABorrar.Clear();

      
          /*  foreach ( var mesh in arbustos)
            {
                mesh.render();
                if (BoundingBox)
                    mesh.BoundingBox.render();
            }*/

            
           

            if (GuiController.Instance.mostrarMensaje && GuiController.Instance.timerMensaje < 0.8)
            {
                GuiController.Instance.mensaje.render();
                GuiController.Instance.timerMensaje += ElapsedTime;
                
            }

            skyBox.render();



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
            palmeraMesh.dispose();
            rocaOriginal.dispose();
            transicionPastoArenaRightTexture.dispose();
            transicionPastoArenaDownTexture.dispose();
            transicionPastoArenaLeftTexture.dispose();
            transicionPastoArenaUpTexture.dispose();
            arbustoMesh.dispose();
            arenaTexture.dispose();
            skyBox.dispose();
            hud.dispose();
        }
    }
}