using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Text;
using Microsoft.DirectX;
using Microsoft.DirectX.Direct3D;
using TGC.Group.Model;
using TGC.Group.InventarioYObjetos;

namespace TGC.Group.Hud
{
    class Hud
    {
        TgcText2D vida;
        TgcText2D agua;
        TgcText2D energia;
        TgcText2D hambre;
        TgcText2D inventarioString;
        CustomSprite hudSprite;
        Drawer2D drawer2D;
        List<ItemContainer> itemsContainerSprite;
        D3DDevice device;
        List<CustomSprite> items; 
        String directorio;
        public TgcText2D mensaje { get; set; }
        public bool mostrarMensaje { get; set; }

      

        public Hud(String HudDir)
        {
            directorio = HudDir;
            vida = new TgcText2D();
            agua = new TgcText2D();
            energia = new TgcText2D();
            drawer2D = new Drawer2D();
            hambre = new TgcText2D();
            inventarioString = new TgcText2D();
            itemsContainerSprite = new List<ItemContainer>();
            items = new List<CustomSprite>();
            mensaje = new TgcText2D();
            mensaje.Position = new Point(D3DDevice.Instance.Width / 2, 10);
            mensaje.changeFont(new System.Drawing.Font("ComicSands", 12, FontStyle.Bold));
            mensaje.Color = Color.White;
            mensaje.Size = new Size(200,10);

            device = D3DDevice.Instance;

            //crear Sprite de hud
            hudSprite = new CustomSprite();
            hudSprite.Bitmap = new CustomBitmap(HudDir + "barras.png", D3DDevice.Instance.Device);
            var textureSize = new Size(30, 30);
            hudSprite.Scaling = new Vector2(0.5f, 0.5f);
            hudSprite.Position = new Vector2(10, 20);
      
            ItemContainer item1 = new ItemContainer(HudDir, 1);
            ItemContainer item2 = new ItemContainer(HudDir, 2);
            ItemContainer item3 = new ItemContainer(HudDir, 3);
            ItemContainer item4 = new ItemContainer(HudDir, 4);
            ItemContainer item5 = new ItemContainer(HudDir, 5);
            ItemContainer item6 = new ItemContainer(HudDir, 6);

            itemsContainerSprite.Add(item1);
            itemsContainerSprite.Add(item2);
            itemsContainerSprite.Add(item3);
            itemsContainerSprite.Add(item4);
            itemsContainerSprite.Add(item5);
            itemsContainerSprite.Add(item6);

        

            //textos
            Size tamanioTexto = new Size(200, 10);
            vida.Size = tamanioTexto;
            vida.Align = TgcText2D.TextAlign.RIGHT;           
            vida.changeFont(new System.Drawing.Font("ComicSands", 12, FontStyle.Bold));
            vida.Position = new Point(60, 20);
            vida.Color = Color.White;
            vida.Text = "Vida: 100";

            energia.Size = tamanioTexto;
            energia.Align = TgcText2D.TextAlign.RIGHT;
            energia.changeFont(new System.Drawing.Font("ComicSands", 12, FontStyle.Bold));
            energia.Position = new Point(60, 50);
            energia.Text = "Energia: 100";
            energia.Color = Color.White;

            agua.Size = tamanioTexto;
            agua.Align = TgcText2D.TextAlign.RIGHT;
            agua.Color = Color.White;
            agua.Text = "Agua: 100";
            agua.Position = new Point(60, 90);
            agua.changeFont(new System.Drawing.Font("ComicSands", 12, FontStyle.Bold));

            hambre.Size = tamanioTexto;
            hambre.Align = TgcText2D.TextAlign.RIGHT;
            hambre.Color = Color.White;
            hambre.Text = "Hambre: 100";
            hambre.Position = new Point(60, 125);
            hambre.changeFont(new System.Drawing.Font("ComicSands", 12, FontStyle.Bold));

            inventarioString.Size = tamanioTexto;
            inventarioString.Color = Color.White;
            inventarioString.Align = TgcText2D.TextAlign.CENTER;
            inventarioString.Text = "Inventario";
            inventarioString.Position = new Point((int)itemsContainerSprite.ElementAt(4).sprite.Position.X - 20,(int)itemsContainerSprite.ElementAt(5).sprite.Position.Y - 40);
            inventarioString.changeFont(new System.Drawing.Font("ComicSands", 12, FontStyle.Bold));

        }

        public ItemContainer getPrimerItemContainerLibre()
        {
            foreach ( var item in itemsContainerSprite)
            {
                if (item.estaDisponible)
                {
                    return item;
                }
            }
            throw new Exception("NO hay ningun item container disponible");
        }

        public void agregarItem(Recurso item)
        {            
            CustomSprite itemSprite = new CustomSprite();
            ItemContainer itemContainer = getPrimerItemContainerLibre();
            itemContainer.estaDisponible = false;
            //TODO elegir que grafico renderizar
            itemSprite.Bitmap = new CustomBitmap(directorio + "wood.png", D3DDevice.Instance.Device);
            itemSprite.Position = itemContainer.sprite.Position;
            items.Add(itemSprite);
        }

        public void render()
        {
            drawer2D.BeginDrawSprite();
            drawer2D.DrawSprite(hudSprite);
            vida.render();
            energia.render();
            agua.render();
            hambre.render();
            inventarioString.render();
            foreach(var item  in itemsContainerSprite)
            {
                drawer2D.DrawSprite(item.sprite);
            }

            foreach ( var item in items)
            {
                drawer2D.DrawSprite(item);
            }
            drawer2D.EndDrawSprite();
            if (mostrarMensaje)
            {
                mostrarMensaje = false;
                mensaje.render();
            }

        }


        public void dispose()
        {
            vida.Dispose();
            agua.Dispose();
            energia.Dispose();
            hambre.Dispose();
            foreach (var item in itemsContainerSprite)
            {
                item.sprite.Dispose();
            }

            foreach (var item in items)
            {
                item.Dispose();
            }
            mensaje.Dispose();
            hudSprite.Dispose();
        }
    }
}
