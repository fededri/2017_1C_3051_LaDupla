using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Core.Text;
using TGC.Group.Hud;
using TGC.Group.Model;

namespace TGC.Group.InventarioYObjetos
{
    //es el sprite negro que contiene a los items en el inventario
    class ItemContainer
    {
        public CustomSprite sprite { get; set; }
        public bool estaDisponible { get; set; }
        public TiposRecursos tipoRecurso { get; set; }
        private int Cantidad;
        public int cantidad
        {
            get { return Cantidad; }
            set
            {
                Cantidad = value;
                textoContador.Text = Cantidad.ToString();
            }
        }
        public TgcText2D textoContador;
        
        public ItemContainer(String hudDir,int position)
        {
            estaDisponible = true;
            sprite = new CustomSprite();
           sprite.Bitmap =  new CustomBitmap(hudDir + "inv_item.png", D3DDevice.Instance.Device);

            switch (position){
                case 1:
                    sprite.Position = new Vector2(D3DDevice.Instance.Width - ((sprite.Bitmap.Width * 2) + 40), D3DDevice.Instance.Height - (sprite.Bitmap.Height + 40));
                    break;

                case 2:
                    sprite.Position = new Vector2(D3DDevice.Instance.Width - (sprite.Bitmap.Width + 20), D3DDevice.Instance.Height - (sprite.Bitmap.Height + 40));
                    break;

                case 3:
                    sprite.Position = new Vector2(D3DDevice.Instance.Width - (sprite.Bitmap.Width + 20), D3DDevice.Instance.Height - (sprite.Bitmap.Height * 2 + 60));
                    break;
                case 4:
                    sprite.Position = new Vector2(D3DDevice.Instance.Width - (sprite.Bitmap.Width * 2 + 40), D3DDevice.Instance.Height - (sprite.Bitmap.Height * 2 + 60));
                    break;
                case 5:
                    sprite.Position = new Vector2(D3DDevice.Instance.Width - (sprite.Bitmap.Width * 2 + 40), D3DDevice.Instance.Height - (sprite.Bitmap.Height * 3 + 80));
                    break;
                case 6:
                    sprite.Position = new Vector2(D3DDevice.Instance.Width - (sprite.Bitmap.Width + 20), D3DDevice.Instance.Height - (sprite.Bitmap.Height * 3 + 80));
                    break;
            }

            textoContador = new TgcText2D();
            textoContador.Align = TgcText2D.TextAlign.CENTER;
            textoContador.Color = Color.White;
            textoContador.Position = new Point((int)sprite.Position.X -70, (int)sprite.Position.Y +62);
            textoContador.Size = new Size(200, 200);
            textoContador.changeFont(new Font("TimesNewRoman", 15, FontStyle.Bold));
            textoContador.Text = "0";

        }

    }
}
