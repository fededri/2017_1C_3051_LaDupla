using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Group.Hud;

namespace TGC.Group.InventarioYObjetos
{
    //es el sprite negro que contiene a los items en el inventario
    class ItemContainer
    {
        public CustomSprite sprite { get; set; }
        public bool estaDisponible { get; set; }

        
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

        }

    }
}
