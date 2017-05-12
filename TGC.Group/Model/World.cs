using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.BoundingVolumes;
using TGC.Core.SceneLoader;
using TGC.Core.Terrain;
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
        private TgcMesh palmeraMesh;


        public World(Vector3 terrainCenter, int size, String mediaDir)
        {

            this.size = size;
            this.mediaDir = mediaDir;
            this.terrainTexture = mediaDir + "Texturas\\" + "Pasto.jpg"; ;

            this.terrainHeightmap = mediaDir + "Heighmaps\\Heightmap3.jpg";
            this.currentScaleXZ = (79.5f / 5000) * size;
            this.currentScaleY = 2f;
            this.position = terrainCenter;
            this.terrain = new TgcSimpleTerrain();
            this.refreshTerrain();
            this.terrain.loadTexture(terrainTexture);

            var loader = new TgcSceneLoader();
            var palmeraScene =
                loader.loadSceneFromFile(mediaDir + "MeshCreator\\Meshes\\Vegetacion\\Palmera\\Palmera-TgcScene.xml");
               
           palmeraMesh = palmeraScene.Meshes[0];
            this.objects = new List<Crafteable>();
            crearPalmeras();
        }

        public void refreshTerrain()
        {
            this.terrainPosition.X = this.position.X / this.currentScaleXZ;
            this.terrainPosition.Z = this.position.Z / this.currentScaleXZ;
            this.terrain.loadHeightmap(this.terrainHeightmap, this.currentScaleXZ, this.currentScaleY, this.terrainPosition);
        }

        public void render()
        {
            this.terrain.render();
            if(objects != null && objects.Count > 0 )
            foreach(var crafteable in objects)
                {
                    crafteable.render();
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
            //this.setPosition(this.position + distance);
            //this.objects.ForEach(crafteable => { crafteable.move(distance); });
        }

        public void setPosition(Vector3 position)
        {

            this.position = position;
            this.refreshTerrain();
        }

        public void dispose()
        {
            this.terrain.dispose();
        } 


        public void crearPalmeras()
        {
            float offsetX, offsetZ;
            var random = new Random();

            for (var i = 0; i < 20; i++)
            {
                offsetX = random.Next(-size/2, size/2);
                offsetZ = random.Next(-size/2, size/2);
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
