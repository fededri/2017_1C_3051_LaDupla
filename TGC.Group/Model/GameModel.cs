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

        private List<TgcPlane> esquinasArena;

        private TgcTexture arenaTexture;

     
        private List<TgcPlane> transicionesPastoArena;
        private List<TgcMesh> arbustos;
        private List<TgcPlane> planosAgua;
        private TgcSkyBox skyBox;
        private float MAX_DIST_A_OBJ_CONSUMIBLE = 300f;
       
        
        private const int planoTransicionPastoArenaAncho = 500;
        private const int anchoIsla = 30000;
        private const int altoIsla = 30000;
        private TgcTexture aguaTexture;
        private List<Crafteable> objetosABorrar;
        private List<Crafteable> objetos;
        private TgcFpsCamera cam;
        private Hud.Hud hud;
        private Personaje personaje;
        private List<MySimpleTerrain> terrains;
        private  World[][] worlds;
        private World[][] savedWorlds;
        private int worldSize = 7500;
        private World currentWorld;
        private TgcBoxDebug box;
        private int flag = 0;
        int sectorToRender;

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
          
            var loader = new TgcSceneLoader();    
            var botellaAgua = new Botella() ;
            botellaAgua.tieneAgua = false;
            personaje.agregarRecurso(botellaAgua);          
                    
            loadWorld();

            box = new TgcBoxDebug();
            
            cam = new TgcFpsCamera(new Vector3(0, 150f, 0), Input);
            cam.currentworld = currentWorld;
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
            savedWorlds = new World[3][];
            savedWorlds[0] = new World[3];
            savedWorlds[1] = new World[3];
            savedWorlds[2] = new World[3];

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


            currentWorld = worlds[1][1];

          
        }

      

        #region inits
      


       


     

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


        public World calculateCurrentWorld(Vector3 position)
        {
            for(int i = 0; i < 3;i++)
            {
                for ( int j = 0; j < 3; j++)
                {
                    if (estaDentroDelWorld(worldSize, worlds[i][j],position)){
                        return worlds[i][j];
                    }
                }
            }

           return currentWorld;
        }
        private bool estaDentroDelWorld(int worldSize, World world, Vector3 position)
        {
            /*  return (position.X <= (world.position.X + worldSize) && position.X >= (world.position.X + worldSize)
                  && position.Z <= (world.position.Z + worldSize) && position.Z >= (world.position.Z + worldSize)
                  );*/
            float limiteSuperiorX, limiteInferiorX, limiteSuperiorZ, limiteInferiorZ;
            limiteSuperiorX = world.position.X + (worldSize / 2);
            limiteInferiorX = world.position.X - (worldSize / 2);
            limiteSuperiorZ = world.position.Z + (worldSize / 2);


            return (position.X >= world.position.X  && position.X <= (world.position.X + worldSize)
                && position.Z <= world.position.Z && position.Z >= (world.position.Z - worldSize));
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
            //currentWorld = calculateCurrentWorld(cam.positionEye);
            cam.currentworld = currentWorld;

            box.PMin = new Vector3(-7500, 0, 0);
            box.PMax = new Vector3(0, 100, 0);
            box.Thickness = 5f;
            box.Color = Color.Red;
            box.updateValues();
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

            if(currentWorld != null)
            worlds[0][0].update(Camara.Position, cam);
            worlds[0][1].update(Camara.Position, cam);
            worlds[0][2].update(Camara.Position, cam);
            worlds[1][0].update(Camara.Position, cam);
            worlds[1][1].update(Camara.Position, cam);
            worlds[1][2].update(Camara.Position, cam);
            worlds[2][0].update(Camara.Position, cam);
            worlds[2][1].update(Camara.Position, cam);
            worlds[2][2].update(Camara.Position, cam);
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


        public void refreshWorlds()
        {
            Vector3 logicPosition = cam.positionEye - currentWorld.position;

            int size = worldSize / 2;
            if (logicPosition.X > size)
            {
                Vector3 newPosition = cam.positionEye;
                newPosition.X = -size;

                copyWorlds();
                for (int i = 0; i <= 2; i++)
                {
                    savedWorlds[i][0].move(new Vector3(size * 6, 0, 0));
                    worlds[i][2] = savedWorlds[i][0];
                    worlds[i][1] = savedWorlds[i][2];
                    worlds[i][0] = savedWorlds[i][1];
                }
            }
            if (logicPosition.X < -size)
            {
                flag = 1;
                Vector3 newPosition = cam.positionEye;
                newPosition.X = size;
                copyWorlds();
                for (int i = 0; i <= 2; i++)
                {
                    savedWorlds[i][2].move(new Vector3(size * -6, 0, 0));
                    worlds[i][2] = savedWorlds[i][1];
                    worlds[i][1] = savedWorlds[i][0];
                    worlds[i][0] = savedWorlds[i][2];
                }
            }
            if (logicPosition.Z > size)
            {
                flag = 1;
                Vector3 newPosition = cam.positionEye;
                newPosition.Z = -size;
                copyWorlds();

                for (int i = 0; i <= 2; i++)
                {
                    savedWorlds[2][i].move(new Vector3(0, 0, size * 6));
                    worlds[0][i] = savedWorlds[2][i];
                    worlds[1][i] = savedWorlds[0][i];
                    worlds[2][i] = savedWorlds[1][i];
                }
            }
            if (logicPosition.Z < -size)
            {
                flag = 1;
                Vector3 newPosition = cam.positionEye;
                newPosition.Z = size;
                copyWorlds();
                for (int i = 0; i <= 2; i++)
                {
                    savedWorlds[0][i].move(new Vector3(0, 0, size * -6));
                    worlds[1][i] = savedWorlds[2][i];
                    worlds[2][i] = savedWorlds[0][i];
                    worlds[0][i] = savedWorlds[1][i];
                }
            }

            currentWorld = worlds[1][1];
            currentWorld.refresh();
        }


        public void copyWorlds()
        {
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    savedWorlds[i][j] = worlds[i][j];
                }
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
            refreshWorlds();
            renderWorlds();


            hud.render();
            box.render();
            
        
          
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

        public void renderWorlds()
        {
            for (int i = 0; i <= 2; i++)
            {
                for (int j = 0; j <= 2; j++)
                {
                    worlds[i][j].rendered = false;
                }

            }

            int constante = -200;
            Vector3 viewDir = new Vector3(cam.directionView.X, 0, cam.directionView.Z);
            Vector3 logicPosition = cam.positionEye - currentWorld.position;
            Vector3 personajePosition = cam.positionEye;
            worlds[1][1].render();       

            if ((viewDir.X > constante && viewDir.Z > constante) || sectorToRender == 1)
            {
                sectorToRender = 1;
                worlds[0][1].render();
                worlds[0][2].render();
                worlds[1][2].render();
            }

            if ((viewDir.X < -constante && viewDir.Z > constante) || sectorToRender == 2)
            {
                sectorToRender = 2;
                worlds[0][1].render();
                worlds[0][0].render();
                worlds[1][0].render();
            }

            if ((viewDir.X > constante && viewDir.Z < -constante) || sectorToRender == 3)
            {
                sectorToRender = 3;
                worlds[1][2].render();
                worlds[2][1].render();
                worlds[2][2].render();
            }

            if ((viewDir.X < -constante && viewDir.Z < -constante) || sectorToRender == 4)
            {
                sectorToRender = 4;
                worlds[1][0].render();
                worlds[2][0].render();
                worlds[2][1].render();
            }
        }

        /// <summary>
        ///     Se llama cuando termina la ejecución del ejemplo.
        ///     Hacer Dispose() de todos los objetos creados.
        ///     Es muy importante liberar los recursos, sobretodo los gráficos ya que quedan bloqueados en el device de video.
        /// </summary>
        public override void Dispose()
        {
            worlds[0][0].dispose();
            worlds[0][1].dispose();
            worlds[0][2].dispose();
            worlds[1][0].dispose();
            worlds[1][1].dispose();
            worlds[1][2].dispose();
            worlds[2][0].dispose();
            worlds[2][1].dispose();
            worlds[2][2].dispose();
            skyBox.dispose();
            hud.dispose();
        }
    }
}