using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Text;
using Microsoft.DirectX;

namespace TGC.Group.Hud
{
    class Hud
    {
        TgcText2D vida;
        TgcText2D agua;
        TgcText2D energia;
        TgcText2D hambre;
        CustomSprite hudSprite;
        Drawer2D drawer2D;
        

        public Hud(String HudDir)
        {
            vida = new TgcText2D();
            agua = new TgcText2D();
            energia = new TgcText2D();
            drawer2D = new Drawer2D();
            hambre = new TgcText2D();
            

            //crear Sprite de hud
            hudSprite = new CustomSprite();
            hudSprite.Bitmap = new CustomBitmap(HudDir + "barras.png", D3DDevice.Instance.Device);
            var textureSize = new Size(30, 30);
            hudSprite.Scaling = new Vector2(0.5f, 0.5f);
            hudSprite.Position = new Vector2(10, 20);

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


        }


        public void render()
        {
            drawer2D.BeginDrawSprite();
            drawer2D.DrawSprite(hudSprite);
            vida.render();
            energia.render();
            agua.render();
            hambre.render();
            drawer2D.EndDrawSprite();
        }

        public void dispose()
        {
            hudSprite.Dispose();

        }

    }
}
