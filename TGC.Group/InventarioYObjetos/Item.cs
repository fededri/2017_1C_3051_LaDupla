using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Direct3D;
using TGC.Group.Hud;
using TGC.Group.Model;

namespace TGC.Group.InventarioYObjetos
{
    public class Item
    {
        public CustomSprite itemSprite {get;set;}
        public Recurso recurso { get; set; }
        public int cantidad { get; set; }


        public Item(Recurso r, String directorio, ItemContainer itemContainer)
        {
            itemSprite = new CustomSprite();
            recurso = r;
            TiposRecursos tipo = r.tipo;
            switch (tipo)
            {
                case TiposRecursos.Madera:
                    itemSprite.Bitmap = new CustomBitmap(directorio + "wood.png", D3DDevice.Instance.Device);
                    break;

                case TiposRecursos.Piedra:
                    itemSprite.Bitmap = new CustomBitmap(directorio + "roca.png", D3DDevice.Instance.Device);
                    break;

                case TiposRecursos.Comida:
                    itemSprite.Bitmap = new CustomBitmap(directorio + "cherries.png", D3DDevice.Instance.Device);
                    break;

                case TiposRecursos.Bebida:
                    if (((Botella)r).tieneAgua)
                    {
                        itemSprite.Bitmap = new CustomBitmap(directorio + "water_full.png", D3DDevice.Instance.Device);
                    }
                    else
                        itemSprite.Bitmap = new CustomBitmap(directorio + "water_empty.png", D3DDevice.Instance.Device);
                    break;

                case TiposRecursos.Encendedor:

                    break;

                default:
                    itemSprite.Bitmap = new CustomBitmap(directorio + "wood.png", D3DDevice.Instance.Device);
                    break;
            }

            itemSprite.Scaling = new Vector2(1.5f, 1.5f);
            itemSprite.Position = new Vector2(itemContainer.sprite.Position.X + 10,
                                            itemContainer.sprite.Position.Y + 10);


        }

    }
}
