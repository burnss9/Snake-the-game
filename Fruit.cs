using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace SnakeGame
{
    public class Fruit : IDrawable, ISerializable
    {


        public ObjectNetID objectNetID;


        //Playing field
        private GameField _gameField;

        private bool isDirty = true;

        //Fruit's position
        private Point _position;

        //Fruit's texture
        private static Texture _appleTex = null;

        //Position getter
        public Point Position
        {
            get { return _position; }
        }

        //Fruit constructor
        public Fruit(Point point, GameField gameField, bool loadTexture = true)
        {
            _position = new Point(point.X, point.Y);
            SetField(gameField);

            if(_appleTex == null && loadTexture)
            {
                _appleTex = Texture.LoadFromFile("textures/apple.png");
            }
        }

        //Set the GameField
        public void SetField(GameField aGameField)
        {
            _gameField = aGameField;
        }

        //Reset the Fruit's position
        public void ResetPosition(Point point)
        {
            _position = new Point(point.X, point.Y);
            setNetworkClean(false);
        }

        //Draw the Fruit's texture
        public void Draw()
        {
            GL.BindTexture(TextureTarget.Texture2D, _appleTex.ID);

            GL.PushMatrix();
            GL.Translate(Position.X * Cell.CELL_SIZE, Position.Y * Cell.CELL_SIZE, 0);

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(1f, 1f, 1f, 1f);

            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, -7);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 32, -7);
            GL.TexCoord2(1, 1);
            GL.Vertex3(32, 32, -7);
            GL.TexCoord2(1, 0);
            GL.Vertex3(32, 0, -7);

            GL.End();

            GL.PopMatrix();
        }

        public byte[] Serialize()
        {

            var PositionBytes = BitConverter.GetBytes(_position.X).Concat(BitConverter.GetBytes(_position.Y)).ToArray();

            byte[] Serialized = PositionBytes;

            return Serialized;
        }

        public void Deserialize(byte[] Serialized)
        {
            int readBytes = 0;
            int headX = BitConverter.ToInt32(Serialized, readBytes);
            readBytes += sizeof(Int32);
            int headY = BitConverter.ToInt32(Serialized, readBytes);
            readBytes += sizeof(Int32);

            _position = new Point(headX, headY);
        }

        public bool isNetworkDirty()
        {
            return isDirty;
        }

        public void setNetworkClean(bool clean)
        {
            isDirty = !clean;
        }
    }

}
