
using Microsoft.DirectX;
using System.IO;
using System.Collections.Generic;
using TGC.Core.SceneLoader;
using TGC.Core.SkeletalAnimation;
using TGC.Group.Camara;
using TGC.Core.Direct3D;

namespace TGC.Group.InventarioYObjetos
{
    public class ObjectsFactory
    {
        private TgcMesh arbolMesh;
        private TgcMesh pinoMesh;
        private TgcMesh arbustoMesh;
        private TgcMesh piedraMesh;
        private TgcMesh hachaMesh;
        private TgcMesh maderaMesh;
        private TgcMesh fogataMesh;
        private TgcMesh leonMesh;
        private TgcMesh frutaMesh;
        private TgcMesh arbustoFrutaMesh;

        
        int objectId = 0;


        TgcSkeletalLoader skeletalLoader = new TgcSkeletalLoader();
        string mediaDir = ""; //TODO
        string skeletalPath;
        string[] animationsPath;
        Microsoft.DirectX.Direct3D.Device d3dDevice = D3DDevice.Instance.Device;
        DirectoryInfo dirAnim;
        FileInfo[] animFiles;
        string[] animationList;

        public ObjectsFactory(string mediaFolderDirection)
        {
            this.mediaDir = mediaFolderDirection;
            skeletalPath = mediaDir + "SkeletalAnimations\\BasicHuman\\";

            TgcSceneLoader loader = new TgcSceneLoader();
           // Microsoft.DirectX.Direct3D.Device d3dDevice = GuiController.Instance.D3dDevice;

          

            dirAnim = new DirectoryInfo(skeletalPath + "Animations\\");
            animFiles = dirAnim.GetFiles("*-TgcSkeletalAnim.xml", SearchOption.TopDirectoryOnly);
            animationList = new string[animFiles.Length];
            animationsPath = new string[animFiles.Length];

            for (int i = 0; i < animFiles.Length; i++)
            {
                string name = animFiles[i].Name.Replace("-TgcSkeletalAnim.xml", "");
                animationList[i] = name;
                animationsPath[i] = animFiles[i].FullName;
            }
        }

        public Human createHuman(Vector3 position, Vector3 scale)
        {
            Inventory inventory = new Inventory(this, new Vector2(20, 20));
            TgcSkeletalMesh humanMesh;
            humanMesh = skeletalLoader.loadMeshAndAnimationsFromFile(skeletalPath + "WomanJeans-TgcSkeletalMesh.xml", skeletalPath, animationsPath);
            humanMesh.buildSkletonMesh();
            return new Human(inventory, humanMesh, position, scale);
        }

  
 

        public void dispose()
        {
            arbolMesh.dispose();
            piedraMesh.dispose();
            hachaMesh.dispose();
            pinoMesh.dispose();
        }
    }
}
