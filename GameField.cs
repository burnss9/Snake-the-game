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

        public int Width { get; set; }
        public int Height { get; set; }

        private static Texture _texture = null;
        private int _textureSize = 32;
        public int TextureSize { get { return _textureSize; } }

        private Game _game;
        public Game game { get { return _game; } }

        private List<List<Cell>> _field = new List<List<Cell>>();
        public List<List<Cell>> Field
        {
            get { return _field; }
        }

        private List<Fruit> _fruits = new List<Fruit>();
        public List<Fruit> Fruits
        {
            get { return _fruits; }
        }


        private List<Snake> _snakes = new List<Snake>();
        public List<Snake> Snakes
        {
            get { return _snakes; }
        }

        public GameField(int aWidth, int aHeight, Game game)
        {
            Width = aWidth;
            Height = aHeight;
            InitField();

            _game = game;

            if (_texture == null)
            {
                _texture = Texture.LoadFromFile("textures/grass.png");
            }
            
            _fruits.Add(new Fruit(new Point(_rand.Next(aWidth), _rand.Next(aHeight)), this));
        }

        private void InitField()
        {
            for (int i = 0; i < Height; i++)
            {
                List<Cell> lRow = new List<Cell>();
                for (int j = 0; j < Width; j++)
                {
                    Cell lCell = new Cell(" ", ConsoleColor.White);
                    lRow.Add(lCell);
                }
                _field.Add(lRow);
            }
        }

        public void ClearField()
        {
            for (int i = 0; i < Height; i++)
            {
                //List<string> lRow = new List<string>();
                for (int j = 0; j < Width; j++)
                {
                    _field[i][j].Val = " ";
                    _field[i][j].Color = ConsoleColor.White;
                }
            }
        }

        public Point RandomPointInField()
        {
            return new Point(_rand.Next(Width), _rand.Next(Height));
        }
        

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
