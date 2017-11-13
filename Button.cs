using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    class Button : UIComponent
    {
        private bool host;
        protected Texture _butTex= null;
        public Button(String path)
        {
            if (_butTex == null)
                _butTex = Texture.LoadFromFile(path);
        }
        public override void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, _butTex.ID);

            GL.PushMatrix();
            GL.Translate(0,0, 0);

            GL.Begin(PrimitiveType.Quads);


            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, -6);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 50, -6);
            GL.TexCoord2(1, 1);
            GL.Vertex3(200, 50, -6);
            GL.TexCoord2(1, 0);
            GL.Vertex3(200, 0, -6);

            GL.End();
            GL.PopMatrix();
        }

        public override void OnClick()
        {
            
        }

        public override void SetField(GameField aGameField)
        {
            throw new NotImplementedException();
        }
    }
}
