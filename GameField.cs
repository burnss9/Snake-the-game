using System;
using System.Collections.Generic;

using OpenTK;
using OpenTK.Graphics.OpenGL;

using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class GameField
    {

        private Random _rand = new Random();

        //GameField's width and height
        public int Width { get; set; }
        public int Height { get; set; }

        //GameField's texture
        private static Texture _texture = null;

        //Texture size and its getter
        private int _textureSize = 32;
        public int TextureSize { get { return _textureSize; } }
        
        private Game _game;
        public Game game { get { return _game; } }
        

        //List of Fruits and its getter
        private List<Fruit> _fruits = new List<Fruit>();
        public List<Fruit> Fruits
        {
            get { return _fruits; }
        }

        //List of Snakes and its getter
        private List<Snake> _snakes = new List<Snake>();
        public List<Snake> Snakes
        {
            get { return _snakes; }
        }

        //GameField constructor
        public GameField(int aWidth, int aHeight, Game game, bool loadTexture = true)
    
        {
            Width = aWidth;
            Height = aHeight;
            
            _game = game;

            if (_texture == null)
            //Set GameField's texture
            if(_texture == null && loadTexture)
            {
               _texture = Texture.LoadFromFile("textures/grass.png");
            }
            
        }
        

        //Generate a random point in the GameField
        public Point RandomPointInField()
        {
            return new Point(_rand.Next(Width), _rand.Next(Height));
        }
        
        //Draw GameField's texture
        public void Draw()
        {

            GL.BindTexture(TextureTarget.Texture2D, _texture.ID);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapS, (int)TextureWrapMode.Repeat);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureWrapT, (int)TextureWrapMode.Repeat);


            GL.Begin(PrimitiveType.Quads);
            GL.Color4(1f, 1f, 1f, 1f);
            
            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, -8);
            GL.TexCoord2(0, Height/3);
            GL.Vertex3(0, Height*32, -8);
            GL.TexCoord2(Width/3, Height/3);
            GL.Vertex3(Width * 32, Height * 32, -8);

            GL.TexCoord2(Width/3, 0);
            GL.Vertex3(Width * 32, 0, -8);

            GL.End();

        }
    }
}
