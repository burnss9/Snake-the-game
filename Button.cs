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
        private int _x, _y;
        public Button(String path, int x, int y)
        {
            if (_butTex == null)
                _butTex = Texture.LoadFromFile(path);
            _x = x;
            _y = y;
        }
        public override void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, _butTex.ID);

            GL.PushMatrix();
            GL.Translate(0,0, 0);

            GL.Begin(PrimitiveType.Quads);


            GL.TexCoord2(0, 0);
            GL.Vertex3(_x, _y, -6);
            GL.TexCoord2(0, 1);
            GL.Vertex3(_x, _y+50, -6);
            GL.TexCoord2(1, 1);
            GL.Vertex3(_x+200,_y+ 50, -6);
            GL.TexCoord2(1, 0);
            GL.Vertex3(_x+200, _y, -6);

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
