using Microsoft.DirectX;
using System.Drawing;
using Microsoft.DirectX.DirectInput;
using TGC.Group.InventarioYObjetos;
using TGC.Group.Camara;
using TGC.Core.Direct3D;
using TGC.Core.Input;
using TGC.Core.Utils;

namespace TGC.Group.InventarioYObjetos
{


    public class Inventory
    {
        public int items;
        //TgcText2d[] texts;
        //TgcSprite[] img;
        bool[] selections;
        //TgcText2d title;
        //TextCreator textCreator = new TextCreator("Arial", 16, new Size(300, 16));
        Point position;
        ObjectsFactory objectsFactory;
        Human dueño;
        public bool hachaEquipada = false;

        public Inventory(ObjectsFactory objectsFactory, Vector2 position)
        {
            int inventorySize = 8; //Por el momento, no hay mas teclas de numeros..
            this.objectsFactory = objectsFactory;
            this.setPosition(position);
          /*  this.items = new Crafteable[inventorySize];
            this.texts = new TgcText2d[inventorySize];
            this.img = new TgcSprite[inventorySize];
            this.selections = new bool[inventorySize];
            this.title = textCreator.createText("Inventario");*/
        }

        public void addItem(int index)
        {
           
            
        }


        public void render()
        {
            int i = 0;
            Point position = this.position;
            Point posIMG = this.position;
            posIMG.Y += 60;
            posIMG.X = 100;
           // this.title.render();
           // for (i = 0; i < this.items.Length; i++) {
            
            //}
        }

        public void setDueño(Human human)
        {
            dueño = human;
        }

        public void setPosition(Vector2 position)
        {
            int x = (int)FastMath.Floor(position.X);
            int y = (int)FastMath.Floor(position.Y);
            this.position = new Point(x, y);
        }

        public void update(TgcD3dInput input) {
            Microsoft.DirectX.Direct3D.Device d3dDevice = D3DDevice.Instance.Device;         


            if (input.keyDown(Key.D0))
            {
                this.unselectAll();
            }

            if (input.keyDown(Key.D1))
            {
                this.selectItem(0);
            }
            if (input.keyDown(Key.D2))
            {
                this.selectItem(1);
            }
            if (input.keyDown(Key.D3))
            {
                this.selectItem(2);
            }
            if (input.keyDown(Key.D4))
            {
                this.selectItem(3);
            }
            if (input.keyDown(Key.D5))
            {
                this.selectItem(4);
            }
            if (input.keyDown(Key.D6))
            {
                this.selectItem(5);
            }
            if (input.keyDown(Key.D7))
            {
                this.selectItem(6);
            }
            if (input.keyDown(Key.D8))
            {
                this.selectItem(7);
            }
            if (input.keyDown(Key.D9))
            {
                this.selectItem(8);
            }
            if (input.keyDown(Key.C))
            {
                if (checkCombinationPosible()) { 
                    this.doCombine();
                }
                else
                {
                    bool item = checkConsumible();

                    //consumir
                }
            }
        }
        
        private void selectItem(int index)
        {
            int[] selectedIndexes = new int[2];
            selectedIndexes = this.getSelectedIndexes();

           /* if (this.items[index] != null && selectedIndexes[1] == -1) { 
                this.selections[index] = true;
                this.texts[index].Color = Color.Blue;
                this.togglePossibleCombinationText();
            }*/
        }

        private void unselectAll()
        {         
         
        }

        private void unselect(int index)
        {
           
        }

        private bool checkConsumible()
        {
            return true;
        }

        private bool checkCombinationPosible()
        {
            return true;
        }

    

        private void combine(int firstIndex, int secondIndex)
        {
            
            
        }

        private void doCombine()
        {
          

        }

        private void doConsumir(int item)
        {
            
        }

        public void dropObject(int index)
        {
          
         }

       

        public int[] getSelectedIndexes()
        {
            int firstIndex = -1;
            int secondIndex = -1;
            for (int i = 0; i < selections.Length; i++)
            {
                if (selections[i] && firstIndex == -1)
                {
                    firstIndex = i;
                }
                else if (selections[i] && secondIndex == -1)
                {
                    secondIndex = i;
                }
            }

            int[] indexes = new int[2];

            indexes[0] = firstIndex;
            indexes[1] = secondIndex;
            return indexes;
        }

        public int findFreeIndex()
        {
            return 0;
        }

    }
}
