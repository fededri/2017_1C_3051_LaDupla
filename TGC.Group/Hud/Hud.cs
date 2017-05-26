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
using TGC.Group.Camara;

namespace TGC.Group.Hud
{
    public class Hud
    {
        TgcText2D vida;
        public TgcText2D agua;
        TgcText2D energia;
        TgcText2D hambre;
        TgcText2D inventarioString;
        CustomSprite hambreSprite,aguaSprite,vidaSprite,energiaSprite;
        Drawer2D drawer2D;
        public List<ItemContainer> itemsContainerSprite { get; }
        D3DDevice device;
        public List<Item> items { get; }
        String directorio;
        public TgcText2D mensaje { get; set; }
        private const int maxItemsPorSlot = 2;
        private Personaje _personaje;
        public Personaje personaje { get { return _personaje; }
            set {
                _personaje = value;
                cargarAguaSprite();
                cargarHambreSprite();
                cargarVidaSprite();
                cargarEnergiaSprite();
            }
        }



       

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
            items = new List<Item>();
            mensaje = new TgcText2D();
            mensaje.Position = new Point(D3DDevice.Instance.Width / 2, 10);
            mensaje.changeFont(new System.Drawing.Font("ComicSands", 12, FontStyle.Bold));
            mensaje.Color = Color.White;
            mensaje.Size = new Size(200,10);

            device = D3DDevice.Instance;

            //crear Sprite de hud
            hambreSprite = new CustomSprite();
            vidaSprite = new CustomSprite();
            energiaSprite = new CustomSprite();
            aguaSprite = new CustomSprite();       
            
      
            ItemContainer item1 = new ItemContainer(HudDir, 1,this);
            ItemContainer item2 = new ItemContainer(HudDir, 2, this);
            ItemContainer item3 = new ItemContainer(HudDir, 3, this);
            ItemContainer item4 = new ItemContainer(HudDir, 4, this);
            ItemContainer item5 = new ItemContainer(HudDir, 5, this);
            ItemContainer item6 = new ItemContainer(HudDir, 6, this);

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


        #region update atributos
        public void actualizarStatusPersonaje()
        {
            if (personaje == null) return;
            hambre.Text = "Hambre: " + personaje.hambre.ToString();
            agua.Text = "Bebida: " + personaje.sed.ToString();
            vida.Text = "Vida: " + personaje.vida.ToString();
            energia.Text = "Energia: " +personaje.energia.ToString();
            cargarAguaSprite();
            cargarHambreSprite();
            cargarVidaSprite();
            cargarEnergiaSprite();
        }

        public void cargarEnergiaSprite()
        {
            if (energiaSprite == null) throw new Exception("Vidasprite no puede ser nulo al llamar a cargarAguaSprite");

            var energia = personaje.energia;
            var textureSize = new Size(30, 30);
            energiaSprite.Scaling = new Vector2(0.5f, 0.5f);
            energiaSprite.Position = new Vector2(20, 50);

            if (energia == 100)
            {
                energiaSprite.Bitmap = new CustomBitmap(directorio + "energia_llena.png", D3DDevice.Instance.Device);
            }

            else if (energia >= 80)
            {
                energiaSprite.Bitmap = new CustomBitmap(directorio + "energia_80p.png", D3DDevice.Instance.Device);
            }

            else if (energia >= 60)
            {
                energiaSprite.Bitmap = new CustomBitmap(directorio + "energia_60p.png", D3DDevice.Instance.Device);
            }

            else if (energia >= 40)
            {
                energiaSprite.Bitmap = new CustomBitmap(directorio + "energia_40p.png", D3DDevice.Instance.Device);
            }


            else if (energia >= 20) { energiaSprite.Bitmap = new CustomBitmap(directorio + "energia_20p.png", D3DDevice.Instance.Device); }

            else if (energia == 0)
            {
                energiaSprite.Bitmap = new CustomBitmap(directorio + "vacio.png", D3DDevice.Instance.Device);

            }
        }

        public void cargarVidaSprite()
        {
            if (vidaSprite == null) throw new Exception("Vidasprite no puede ser nulo al llamar a cargarAguaSprite");

            var vida = personaje.vida;
            var textureSize = new Size(30, 30);
            vidaSprite.Scaling = new Vector2(0.5f, 0.5f);
            vidaSprite.Position = new Vector2(20, 20);

            if (vida == 100)
            {
                vidaSprite.Bitmap = new CustomBitmap(directorio + "vida_llena.png", D3DDevice.Instance.Device);
            }

            else if (vida >= 80)
            {
                vidaSprite.Bitmap = new CustomBitmap(directorio + "vida_80p.png", D3DDevice.Instance.Device);
            }

            else if (vida >= 60)
            {
                vidaSprite.Bitmap = new CustomBitmap(directorio + "vida_60p.png", D3DDevice.Instance.Device);
            }

            else if (vida >= 40)
            {
                vidaSprite.Bitmap = new CustomBitmap(directorio + "vida_40p.png", D3DDevice.Instance.Device);
            }


            else if (vida >= 20) { vidaSprite.Bitmap = new CustomBitmap(directorio + "vida_20p.png", D3DDevice.Instance.Device); }

            else if (vida == 0)
            {
                vidaSprite.Bitmap = new CustomBitmap(directorio + "vacio.png", D3DDevice.Instance.Device);

            }
        }

        public void cargarHambreSprite()
        {
            if (hambreSprite == null) throw new Exception("AguaSprite no puede ser nulo al llamar a cargarAguaSprite");

            var hambre = personaje.hambre;
            var textureSize = new Size(30, 30);
            hambreSprite.Scaling = new Vector2(0.5f, 0.5f);
            hambreSprite.Position = new Vector2(20, 125);

            if (hambre == 100)
            {
                hambreSprite.Bitmap = new CustomBitmap(directorio + "hambre_llena.png", D3DDevice.Instance.Device);
            }

            else if (hambre >= 80)
            {
                hambreSprite.Bitmap = new CustomBitmap(directorio + "hambre_80p.png", D3DDevice.Instance.Device);
            }

            else if (hambre >= 60)
            {
                hambreSprite.Bitmap = new CustomBitmap(directorio + "hambre_60p.png", D3DDevice.Instance.Device);
            }

            else if (hambre >= 40)
            {
                hambreSprite.Bitmap = new CustomBitmap(directorio + "hambre_40p.png", D3DDevice.Instance.Device);
            }


            else if (hambre >= 20) { hambreSprite.Bitmap = new CustomBitmap(directorio + "hambre_20p.png", D3DDevice.Instance.Device); }

            else if (hambre == 0)
            {
                hambreSprite.Bitmap = new CustomBitmap(directorio + "vacio.png", D3DDevice.Instance.Device);

            }
        }

        public void cargarAguaSprite()
        {
            if (aguaSprite == null) throw new Exception("AguaSprite no puede ser nulo al llamar a cargarAguaSprite");

            var bebida = personaje.sed;
            var textureSize = new Size(30, 30);
            aguaSprite.Scaling = new Vector2(0.5f, 0.5f);
            aguaSprite.Position = new Vector2(20, 90);

            if (bebida == 100)
            {
                aguaSprite.Bitmap = new CustomBitmap(directorio + "sed_llena.png", D3DDevice.Instance.Device);
            }

            else if (bebida >= 80)
            {
                aguaSprite.Bitmap = new CustomBitmap(directorio + "sed_80p.png", D3DDevice.Instance.Device);
            }

            else if (bebida >= 60)
            {
                aguaSprite.Bitmap = new CustomBitmap(directorio + "sed_60p.png", D3DDevice.Instance.Device);
            }

            else if (bebida >= 40)
            {
                aguaSprite.Bitmap = new CustomBitmap(directorio + "sed_40p.png", D3DDevice.Instance.Device);
            }


            else if (bebida >= 20) { aguaSprite.Bitmap = new CustomBitmap(directorio + "sed_20p.png", D3DDevice.Instance.Device); }

            else if (bebida == 0)
            {
                aguaSprite.Bitmap = new CustomBitmap(directorio + "vacio.png", D3DDevice.Instance.Device);

            }
        }
        #endregion



        #region agregarItem
        public ItemContainer getPrimerItemContainerLibre()
        {
            foreach (var item in itemsContainerSprite)
            {
                if (item.estaDisponible)
                {
                    return item;
                }
            }
            throw new Exception("NO hay ningun item container disponible");
        }

        public ItemContainer getItemContainer(Recurso item)
        {
            foreach (var container in itemsContainerSprite)
            {
                if (!container.estaDisponible && container.tipoRecurso == item.tipo)
                {
                    var cantidad = cuantosObjetosDeEsteTipoHay(item.tipo);
                    if (cantidad >= maxItemsPorSlot)
                    {
                        Gui.Instance.mensaje.Text = "No hay mas espacio para este recurso";
                        return null;
                    }
                    else
                    {
                        return container;
                    }
                }
            }
            //si el recurso no lo tiene el jugador entonces busco el primer slot libre
            return getPrimerItemContainerLibre();
        }


        public int cuantosObjetosDeEsteTipoHay(TiposRecursos tipo)
        {
            if (items.Count == 0) return 0;
            foreach (var itemContainer in itemsContainerSprite)
            {
                if (itemContainer.tipoRecurso == tipo)
                {
                    return itemContainer.cantidad;
                }
            }
            return 0;
        }

        public void borrarItemEnItemContainer(ItemContainer ic)
        {
            var index = -1;
            for (int i = 0; i < items.Count; i++)
            {
                if (items[i].ic == ic)
                {
                    index = i;
                    break;
                }
            }

            if (index != -1)
            {
                items.RemoveAt(index);
            }
        }

        public void agregarItem(Recurso recurso)
        {

            ItemContainer itemContainer = getItemContainer(recurso);
            if (itemContainer == null) return;
            Item item = new Item(recurso, directorio, itemContainer);
            itemContainer.tipoRecurso = recurso.tipo;
            itemContainer.recurso = recurso;
            itemContainer.cantidad++;
            if (itemContainer.estaDisponible)
            {
                itemContainer.estaDisponible = false;

            }
            else
            {
                //aumentar el contador de cantidad de items
            }


            items.Add(item);
        }
        #endregion


        public void render()
        {
            drawer2D.BeginDrawSprite();
            drawer2D.DrawSprite(hambreSprite);
            drawer2D.DrawSprite(aguaSprite);
            drawer2D.DrawSprite(vidaSprite);
            drawer2D.DrawSprite(energiaSprite);
            vida.render();
            energia.render();
            agua.render();
            hambre.render();
            
            inventarioString.render();
            foreach(var item  in itemsContainerSprite)
            {
                drawer2D.DrawSprite(item.sprite);
                item.textoContador.render();
            }

            foreach ( var item in items)
            {
                drawer2D.DrawSprite(item.itemSprite);
                
            }
            drawer2D.EndDrawSprite();
         

        }

       public void usarItem(int position)
        {
            itemsContainerSprite.ElementAt(position-1).usarItem(personaje);
        }

        public void dispose()
        {
            vida.Dispose();
            agua.Dispose();
            energia.Dispose();
            hambre.Dispose();
            hambreSprite.Dispose();
            
            foreach (var item in itemsContainerSprite)
            {
                item.sprite.Dispose();
            }

            foreach (var item in items)
            {
                item.itemSprite.Dispose();
            }
            mensaje.Dispose();
        }
    }
}
