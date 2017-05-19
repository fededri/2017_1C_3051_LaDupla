using Microsoft.DirectX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TGC.Core.Terrain;
using System.Drawing;
using Microsoft.DirectX.Direct3D;
using TGC.Core.SceneLoader;

namespace TGC.Group.Model
{
    public class LDSkyBox : TgcSkyBox
    {      

            public void init(String MediaDir)
            {
                Size = new Vector3(18000, 18000, 18000);
                var texturesPath = MediaDir + "Texturas\\Quake\\SkyBox LostAtSeaDay\\";
                setFaceTexture(TgcSkyBox.SkyFaces.Up, texturesPath + "lostatseaday_up.jpg");
                setFaceTexture(TgcSkyBox.SkyFaces.Down, texturesPath + "lostatseaday_dn.jpg");
                setFaceTexture(TgcSkyBox.SkyFaces.Left, texturesPath + "lostatseaday_lf.jpg");
                setFaceTexture(TgcSkyBox.SkyFaces.Right, texturesPath + "lostatseaday_rt.jpg");
                setFaceTexture(TgcSkyBox.SkyFaces.Front, texturesPath + "lostatseaday_bk.jpg");
                setFaceTexture(TgcSkyBox.SkyFaces.Back, texturesPath + "lostatseaday_ft.jpg");
                SkyEpsilon = 1f;
            //this.AlphaBlendEnable = true;

            base.Init();

            }


            //public TgcMesh[] faces;

            public void horario(string horario)
            {
                switch (horario)
                {
                    case "maniana": this.Color = Color.Coral; break;
                    case "dia": this.Color = Color.Transparent; break;
                    case "tarde": this.Color = Color.OrangeRed; break;
                    case "noche": this.Color = Color.DarkGray; break;
                }
            }


            public void cambiarHorario()
            {
                this.Color = Color.Red;
                //this.updateValues();

                string horarioActual = Color.ToString();

                switch (horarioActual)
                {
                    case "Coral": this.horario("dia"); break;
                    case "Transparent": this.horario("tarde"); break;
                    case "DarkGoldenrod": this.horario("noche"); break;
                    case "DarkBlue": this.horario("maniana"); break;
                }


            }


         public void update(Vector3 pos)
        {
            foreach (TgcMesh face in base.Faces)
            {
                face.AutoTransformEnable = false;
                face.Transform = Matrix.Translation(pos.X, pos.Y, pos.Z);               

            }
        }

            public void renderLdSkyBox()
            {
                foreach (TgcMesh face in base.Faces)
                {
                    face.render();
                }
            }


        }
    }



