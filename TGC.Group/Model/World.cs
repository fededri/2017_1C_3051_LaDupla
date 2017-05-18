using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.Collision;
using TGC.Core.SceneLoader;
using TGC.Core.Terrain;
using TGC.Group.Camara;
using TGC.Group.InventarioYObjetos;
using TGC.Group.Shaders;

namespace TGC.Group.Model
{
    public class World
    {
        private TgcSimpleTerrain terrain;
        float currentScaleXZ;
        float currentScaleY;
        public Vector3 position;
        int size;
        public Vector3 terrainPosition;
        string terrainTexture;
        public string terrainHeightmap;
        public bool rendered = false;
        Random rnd = new Random();
        String mediaDir;
        List<Crafteable> objects;
        private TgcMesh palmeraMesh,rocaMesh,arbustoMesh;
        public float worldSize;

        public World(Vector3 terrainCenter, int size, String mediaDir)
        {

            this.size = size;
            this.mediaDir = mediaDir;
            this.terrainTexture = mediaDir + "Texturas\\" + "Pasto.jpg"; ;

            this.terrainHeightmap = mediaDir + "Heighmaps\\Heightmap3.jpg";
            this.currentScaleXZ = (79.5f / 5000) * size;
                       
           
            this.currentScaleY = 1f;
            this.position = terrainCenter;
            this.terrain = new TgcSimpleTerrain();
            this.refreshTerrain();
            this.terrain.loadTexture(terrainTexture);
            this.objetosABorrar = new List<Crafteable>();

            var loader = new TgcSceneLoader();
            var palmeraScene =
                loader.loadSceneFromFile(mediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
            var rocaScene = loader.loadSceneFromFile(mediaDir + "MeshCreator\\Meshes\\Vegetacion\\Roca\\Roca-TgcScene.xml");
            var arbustoScene = loader.loadSceneFromFile(mediaDir + "\\MeshCreator\\Meshes\\Vegetacion\\Planta2\\Planta2-TgcScene.xml");

            arbustoMesh = arbustoScene.Meshes[0];
            rocaMesh = rocaScene.Meshes[0];
            palmeraMesh = palmeraScene.Meshes[0];

          

            this.objects = new List<Crafteable>();
            crearPalmeras();
            crearArbustos();
            crearRocas();
        }

        public void refreshTerrain()
        {
            this.terrainPosition.X = this.position.X / this.currentScaleXZ;
            this.terrainPosition.Z = this.position.Z / this.currentScaleXZ;
            this.terrain.loadHeightmap(this.terrainHeightmap, this.currentScaleXZ, this.currentScaleY, this.terrainPosition);
        }

        public void refresh()
        {
            this.objects.RemoveAll(crafteable => crafteable == null);
        }


        public void update(Vector3 cameraPosition, TgcFpsCamera cam)
        {
            foreach (var obj in objects)
            {
                obj.update();
                if (obj.cilindro != null)
                {

                    if (TgcCollisionUtils.testPointCylinder(cameraPosition, obj.cilindro))
                    {
                       obj.cilindro.setRenderColor(Color.Blue);
                        cam.colision = true;

                    }
                }
                else if (obj.esfera != null)
                {
                    if (TgcCollisionUtils.testPointSphere(obj.esfera, cameraPosition))
                    {
                        cam.colision = true;
                        break;

                    }
                }
            }
        }


        public void render(TgcFrustum frustum)
        {
            if (!rendered)
            {
                foreach (var crafteable in objetosABorrar)
                {
                    if (crafteable.destruirse)
                    {
                        objects.Remove(crafteable);
                    }
                }

             

                this.terrain.render();
                if (objects != null && objects.Count > 0)
                    foreach (var crafteable in objects)
                    {
                        TgcCollisionUtils.FrustumResult frustumResult = TgcCollisionUtils.classifyFrustumAABB(frustum, crafteable.mesh.BoundingBox);
                        if (frustumResult.Equals(TgcCollisionUtils.FrustumResult.INTERSECT)
                            || frustumResult.Equals(TgcCollisionUtils.FrustumResult.INSIDE))
                        {
                            crafteable.render();
                        };
                        //crafteable.render();
                    }
                this.rendered = true;
            }
         
        }     

     

        public float calcularAltura(float x, float z)
        {
            float largo = this.currentScaleXZ * 64;
            float pos_i = 64f * (0.5f + (x - this.position.X) / largo);
            float pos_j = 64f * (0.5f + (z - this.position.Z) / largo);

            int pi = (int)pos_i;
            float fracc_i = pos_i - pi;
            int pj = (int)pos_j;
            float fracc_j = pos_j - pj;

            if (pi < 0)
                pi = 0;
            else
                if (pi > 63)
                pi = 63;

            if (pj < 0)
                pj = 0;
            else
                if (pj > 63)
                pj = 63;

            int pi1 = pi + 1;
            int pj1 = pj + 1;
            if (pi1 > 63)
                pi1 = 63;
            if (pj1 > 63)
                pj1 = 63;

            // 2x2 percent closest filtering usual: 
            float H0 = this.terrain.HeightmapData[pi, pj] * currentScaleY;
            float H1 = this.terrain.HeightmapData[pi1, pj] * currentScaleY;
            float H2 = this.terrain.HeightmapData[pi, pj1] * currentScaleY;
            float H3 = this.terrain.HeightmapData[pi1, pj1] * currentScaleY;
            float H = (H0 * (1 - fracc_i) + H1 * fracc_i) * (1 - fracc_j) +
                      (H2 * (1 - fracc_i) + H3 * fracc_i) * fracc_j;

            return H;
        }

        public void move(Vector3 distance)
        {
            this.setPosition(this.position + distance);
            this.objects.ForEach(crafteable => { crafteable.move(distance); });
        }

        private List<Crafteable> objetosABorrar;

        public void checkearObjetoMasCercano(Vector3 position, float maximaDistanciaAObjConsumible, Personaje personaje)
        {
            foreach (var objeto in objects)
            {
                var distancia = Vector3.Length(objeto.Position - position);
                if (distancia <= maximaDistanciaAObjConsumible && distancia > 0)
                {
                    //consumir
                    bool inventarioLleno = personaje.inventarioLleno();
                    var result = objeto.consumir(inventarioLleno);
                    if (objeto.destruirse) objetosABorrar.Add(objeto);
                    if (result != null)
                    {
                        //agrego objeto al personaje si tiene espacio
                        if (!inventarioLleno)
                            personaje.agregarRecurso(result);

                    }
                }
            }
        }

        public void setPosition(Vector3 position)
        {

            this.position = position;
            this.refreshTerrain();
        }

        public void dispose()
        {
            this.terrain.dispose();
            rocaMesh.dispose();
            arbustoMesh.dispose();
            palmeraMesh.dispose();
        } 


        public void crearArbustos()
        {
            var random = new Random();

            int a = size / 2;
            for (var i = 0; i < 10; i++)
            {             
               var  offsetX = random.Next((int)position.X - (size - a), (int)position.X + (size - a));
               var  offsetZ = random.Next((int)position.Z - (size - a), (int)position.Z + (size - a));
                var instance = arbustoMesh.createMeshInstance(arbustoMesh.Name + i);
                instance.AutoTransformEnable = true;

                instance.move(offsetX, calcularAltura(offsetX, offsetZ), offsetZ);
                if (random.Next(0, 2) == 1)
                {
                    instance.Scale = new Vector3(5f, 5f, 5f);
                    Arbusto arbustoObj = new Arbusto(instance);
                    objects.Add(arbustoObj);
                }
                else
                {
                    instance.Scale = new Vector3(1f, 1f, 1f);
                    Arbusto arbustoObj = new Arbusto(instance);
                    objects.Add(arbustoObj);
                }

            }
        }

        public void crearRocas()
        {
            var random = new Random();
            int offsetX, offsetZ;
            int a = (size / 2) + 400;
            for (var i = 0; i < 10; i++)
            {
                offsetX = random.Next((int)position.X - (size - a), (int)position.X + (size - a));
                offsetZ = random.Next((int)position.Z - (size - a), (int)position.Z + (size - a));
                var instance = rocaMesh.createMeshInstance(rocaMesh.Name + "i");

                instance.AutoTransformEnable = true;
                instance.move(offsetX, calcularAltura(offsetX, offsetZ), offsetZ);
                instance.Scale = new Vector3(5f, 3f, 5f);
                var esfera = new TgcBoundingSphere(new Vector3(offsetX, calcularAltura(offsetX, offsetZ), offsetZ), 180);

                Roca roca = new Roca(instance);
                roca.esfera = esfera;
                objects.Add(roca);

            }
        }


        public void crearPalmeras()
        {
            float offsetX, offsetZ;
            var random = new Random();
            int a = (size / 2) - 400;
           

            for (var i = 0; i < 20; i++)
            {
                offsetX = random.Next((int)position.X, (int) position.X + (size - a));
                offsetZ = random.Next((int)position.Z - (size - a), (int)position.Z );
                var instance = palmeraMesh.createMeshInstance(palmeraMesh.Name + i);
                instance.move(offsetX, calcularAltura(offsetX, offsetZ), offsetZ);
                instance.AutoTransformEnable = true;
                instance.Scale = new Vector3(3f, 5f, 3f);
                var collisionableCylinder = new TgcBoundingCylinder(new Vector3(offsetX, 0, offsetZ), 60, 800);
                collisionableCylinder.setRenderColor(Color.Green);
                var palmera = new Palmera(instance);
                palmera.cilindro = collisionableCylinder;
                objects.Add(palmera);
            }
        }
     
    }
}
