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
        CustomSprite hudSprite;
        Drawer2D drawer2D;

        public Hud(String HudDir)
        {
            vida = new TgcText2D();
            agua = new TgcText2D();
            energia = new TgcText2D();
            drawer2D = new Drawer2D();

            Size tamanioTexto = new Size(40, 60);
            vida.Size = tamanioTexto;
            vida.changeFont(new System.Drawing.Font("ComicSands", 16, FontStyle.Bold));

            vida.Color = Color.Red;
            agua.Size = tamanioTexto;
            agua.changeFont(new System.Drawing.Font("ComicSands", 16, FontStyle.Bold));
            agua.Color = Color.Cyan;
            energia.Size = tamanioTexto;
            energia.changeFont(new System.Drawing.Font("ComicSands", 16, FontStyle.Bold));
            energia.Color = Color.Yellow;

            //crear Sprite de hud
            hudSprite = new CustomSprite();
            hudSprite.Bitmap = new CustomBitmap(HudDir + "barras.png", D3DDevice.Instance.Device);
            var textureSize = new Size(30,30);
            hudSprite.Scaling = new Vector2(0.5f, 0.5f);
            
            hudSprite.Position = new Vector2(10, 20);


        }


        public void render()
        {
            drawer2D.BeginDrawSprite();
            drawer2D.DrawSprite(hudSprite);

            drawer2D.EndDrawSprite();
        }

        public void dispose()
        {
            hudSprite.Dispose();

        }

    }
}
