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
        List<CustomSprite> inventario;
        D3DDevice device;
        List<CustomSprite> items;
        String directorio;
        

        public Hud(String HudDir)
        {
            directorio = HudDir;
            vida = new TgcText2D();
            agua = new TgcText2D();
            energia = new TgcText2D();
            drawer2D = new Drawer2D();
            hambre = new TgcText2D();
            inventarioString = new TgcText2D();
            inventario = new List<CustomSprite>();
            items = new List<CustomSprite>();


            device = D3DDevice.Instance;

            //crear Sprite de hud
            hudSprite = new CustomSprite();
            hudSprite.Bitmap = new CustomBitmap(HudDir + "barras.png", D3DDevice.Instance.Device);
            var textureSize = new Size(30, 30);
            hudSprite.Scaling = new Vector2(0.5f, 0.5f);
            hudSprite.Position = new Vector2(10, 20);


            //items
            //primera fila
            CustomSprite item1 = new CustomSprite();
            item1.Bitmap = new CustomBitmap(HudDir + "inv_item.png", device.Device);
            item1.Position = new Vector2(device.Width - ((item1.Bitmap.Width*2)+40),device.Height - (item1.Bitmap.Height+40));

            CustomSprite item2 = new CustomSprite();
            item2.Bitmap = new CustomBitmap(HudDir + "inv_item.png", device.Device);
            item2.Position = new Vector2(device.Width - (item2.Bitmap.Width + 20), device.Height - (item2.Bitmap.Height+40));

            //segunda fila
            CustomSprite item3 = new CustomSprite();
            item3.Bitmap = new CustomBitmap(HudDir + "inv_item.png", device.Device);
            item3.Position = new Vector2(device.Width - (item3.Bitmap.Width + 20), device.Height - (item3.Bitmap.Height*2 + 60));

            CustomSprite item4 = new CustomSprite();
            item4.Bitmap = new CustomBitmap(HudDir + "inv_item.png", device.Device);
            item4.Position = new Vector2(device.Width - (item4.Bitmap.Width*2 + 40), device.Height - (item4.Bitmap.Height*2 + 60));

            //tercera fila
            CustomSprite item5 = new CustomSprite();
            item5.Bitmap = new CustomBitmap(HudDir + "inv_item.png", device.Device);
            item5.Position = new Vector2(device.Width - (item5.Bitmap.Width*2 + 40), device.Height - (item5.Bitmap.Height*3 + 80));

            CustomSprite item6 = new CustomSprite();
            item6.Bitmap = new CustomBitmap(HudDir + "inv_item.png", device.Device);
            item6.Position = new Vector2(device.Width - (item6.Bitmap.Width + 20), device.Height - (item6.Bitmap.Height * 3 + 80));

            inventario.Add(item1);
            inventario.Add(item2);
            inventario.Add(item3);
            inventario.Add(item4);
            inventario.Add(item5);
            inventario.Add(item6);

            CustomSprite itemSprite = new CustomSprite();
            itemSprite.Bitmap = new CustomBitmap(directorio + "wood.png", D3DDevice.Instance.Device);
            itemSprite.Position = item1.Position;
            itemSprite.Scaling = new Vector2(1.5f, 1.5f);
            items.Add(itemSprite);

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
            inventarioString.Position = new Point((int)item5.Position.X - 10,(int) item6.Position.Y - 40);
            inventarioString.changeFont(new System.Drawing.Font("ComicSands", 12, FontStyle.Bold));

        }


        public void agregarItem(Recurso item)
        {
            
            CustomSprite itemSprite = new CustomSprite();
            CustomSprite slotInventario =  inventario.ElementAt(0);
            itemSprite.Bitmap = new CustomBitmap(directorio + "wood.png", D3DDevice.Instance.Device);
            itemSprite.Position = slotInventario.Position;
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
            foreach(var item  in inventario)
            {
                drawer2D.DrawSprite(item);
            }

            foreach ( var item in items)
            {
                drawer2D.DrawSprite(item);
            }
            drawer2D.EndDrawSprite();
        }

        public void dispose()
        {
            hudSprite.Dispose();

        }

    }
}
