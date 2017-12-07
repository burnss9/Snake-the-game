using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Snake : IMovable, IDrawable, ISerializable
    {
        public ObjectNetID objectNetID;

        //Snake Direction
        public Direction Dir = Direction.Stop;
        protected double TimeSinceLastMove = 0;
        protected double MoveDelay = 0.2;//lower to make snake faster
        protected int Score = 0;


        //The playing field
        protected GameField _gameField;

        bool _networkDirty = false;

        //Texture for the Snake's head and body
        private static Texture _snakeHeadTex = null;
        private static Texture _snakeBodyTex = null;

        //color of the Snake
        Vector4 _snakeColor = new Vector4(175 / 255f, 96 / 255f, 255 / 255f, 1);
        public Vector4 SnakeColor { get { return _snakeColor; } }

        //Get and Set the Snake's Head
        public Point Head { get; set; }

        //List of points representing the Snake's tail
        private List<Point> _tail = new List<Point>();

        //Tail getter
        public List<Point> Tail
        {
            get { return _tail; }
        }

        //Snake constructor
        public Snake(Point point, GameField gameField, bool loadTexture = true)
        {
            Head = new Point(point.X, point.Y);
            SetField(gameField);

            //set head and body textures
            if (_snakeHeadTex == null && loadTexture)
            {
                _snakeHeadTex = Texture.LoadFromFile("textures/snakehead.png");
            }
            if (_snakeBodyTex == null && loadTexture)
            {
                _snakeBodyTex = Texture.LoadFromFile("textures/snakebody.png");
            }
        }

        //Set the GameField
        public void SetField(GameField aGameField)
        {
            _gameField = aGameField;
        }

        public void SetColor(byte R, byte G, byte B)
        {
            _snakeColor = new Vector4(R / 255f, G / 255f, B / 255f, 1);
        }

        //Eat the fruit
        public virtual void Eat(Fruit f)
        {
            Score++;

            //add to snake and change fruit's position
            _tail.Insert(0, new Point(Head.X, Head.Y));
            f.ResetPosition(_gameField.RandomPointInField());
        }

        //Set Snake's Direction
        public void SetDirection(Direction direction)
        {
            if (Dir == Direction.Down && direction == Direction.Up)
            {

            }
            else if (Dir == Direction.Up && direction == Direction.Down)
            {


            }
            else if (Dir == Direction.Right && direction == Direction.Left)
            {


            }
            else if (Dir == Direction.Left && direction == Direction.Right)
            {


            }
            else
            {

                Dir = direction;
                if (_gameField?.game?.networkManager?.isServer == null ? false : _gameField.game.networkManager.isServer)
                {
                    _networkDirty = true;
                }
                else if (_gameField?.game?.networkManager?.isServer == null ? false : !_gameField.game.networkManager.isServer)
                {
                    byte[] msg = Encoding.ASCII.GetBytes("Turn:").Concat(BitConverter.GetBytes((Int16)direction)).ToArray();
                    _gameField.game.networkManager.Host.Send(msg);
                }
            }


        }

        //Move the Snake
        public virtual void Move(bool forceUpdate = false)
        {

            if (TimeSinceLastMove < MoveDelay && !forceUpdate)
            {
                return;
            }

            TimeSinceLastMove = 0;
            Serialize();

            _tail.Insert(0, new Point(Head.X, Head.Y));
            _tail.RemoveAt(_tail.Count - 1);

            //Eat the fruit if Snake's head and fruit have the same position
            if (_gameField.Fruits.Count > 0 && Head.Equals(_gameField.Fruits[0].Position))
            {
                Eat(_gameField.Fruits[0]);
            }

            //Sets the direction of the Snake's Head
            switch (Dir)
            {
                case Direction.Left:
                    Head.X--;
                    break;
                case Direction.Right:
                    Head.X++;
                    break;
                case Direction.Up:
                    Head.Y--;
                    break;
                case Direction.Down:
                    Head.Y++;
                    break;
                default:
                    break;
            }
            wrap(forceUpdate);
        }

        internal void Tick(double deltaTime)
        {
            TimeSinceLastMove += deltaTime;
            Move();
        }

        //if snake goes outside play area wrap coordinates
        private void wrap(bool forceCheck = false)
        {
            if (Head.X > _gameField.Width - 1) Head.X = 0;
            if (Head.X < 0) Head.X = _gameField.Width - 1;
            if (Head.Y > _gameField.Height - 1) Head.Y = 0;
            if (Head.Y < 0) Head.Y = _gameField.Height - 1;

            foreach (Snake snake in _gameField.Snakes)
            {
                foreach (Point p in snake.Tail)
                {
                    if (Head.Equals(p))
                    {

                        //respawn for now 
                        if ((_gameField?.game?.networkManager?.isServer == null ? false : _gameField.game.networkManager.isServer) || forceCheck)
                        {

                            Respawn();
                            _networkDirty = true;


                        }
                        else if (_gameField?.game?.networkManager?.isServer == null ? false : !_gameField.game.networkManager.isServer)
                        {
                            //client prediction potentially unecessary but leaving this in for the future
                        }


                    }
                }
            }
        }

        public void Respawn()
        {

            _tail = new List<Point>();

            Dir = Direction.Stop;

            Head = _gameField.RandomPointInField();

        }

        //Draw Snake's textures
        public void Draw()
        {

            GL.BindTexture(TextureTarget.Texture2D, _snakeHeadTex.ID);

            GL.PushMatrix();
            GL.Translate(Head.X * Cell.CELL_SIZE, Head.Y * Cell.CELL_SIZE, 0);

            GL.Begin(PrimitiveType.Quads);
            GL.Color4(_snakeColor);


            GL.TexCoord2(0, 0);
            GL.Vertex3(0, 0, -6);
            GL.TexCoord2(0, 1);
            GL.Vertex3(0, 32, -6);
            GL.TexCoord2(1, 1);
            GL.Vertex3(32, 32, -6);
            GL.TexCoord2(1, 0);
            GL.Vertex3(32, 0, -6);

            GL.End();
            GL.PopMatrix();

            GL.BindTexture(TextureTarget.Texture2D, _snakeBodyTex.ID);

            foreach (Point p in Tail)
            {
                GL.PushMatrix();
                GL.Translate(p.X * Cell.CELL_SIZE, p.Y * Cell.CELL_SIZE, 0);

                GL.Begin(PrimitiveType.Quads);

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

        }

        public byte[] Serialize()
        {

            var DirectionBytes = BitConverter.GetBytes((short)Dir);

            var HeadBytes = BitConverter.GetBytes(Head.X).Concat(BitConverter.GetBytes(Head.Y)).ToArray();

            var ColorBytes = BitConverter.GetBytes(_snakeColor.X).Concat(BitConverter.GetBytes(_snakeColor.Y)).Concat(BitConverter.GetBytes(_snakeColor.Z)).Concat(BitConverter.GetBytes(_snakeColor.W)).ToArray();

            var TailCountBytes = BitConverter.GetBytes(Tail.Count());

            IEnumerable<byte> TailBytes = new byte[0];
            foreach (Point p in Tail)
            {
                TailBytes = TailBytes.Concat(BitConverter.GetBytes(p.X).Concat(BitConverter.GetBytes(p.Y)));
            }

            byte[] Serialized = DirectionBytes.Concat(HeadBytes).Concat(ColorBytes).Concat(TailCountBytes).Concat(TailBytes).ToArray();

            return Serialized;

        }

        public void Deserialize(byte[] Serialized)
        {
            int readBytes = 0;

            Dir = (Direction)BitConverter.ToInt16(Serialized, 0);
            readBytes += sizeof(Int16);

            int headX = BitConverter.ToInt32(Serialized, readBytes);
            readBytes += sizeof(Int32);
            int headY = BitConverter.ToInt32(Serialized, readBytes);
            readBytes += sizeof(Int32);

            Head = new Point(headX, headY);


            float colorX = BitConverter.ToSingle(Serialized, readBytes);
            readBytes += sizeof(float);
            float colorY = BitConverter.ToSingle(Serialized, readBytes);
            readBytes += sizeof(float);
            float colorZ = BitConverter.ToSingle(Serialized, readBytes);
            readBytes += sizeof(float);
            float colorW = BitConverter.ToSingle(Serialized, readBytes);
            readBytes += sizeof(float);

            _snakeColor = new Vector4(colorX, colorY, colorZ, colorW);

            int TailCount = BitConverter.ToInt32(Serialized, readBytes);
            readBytes += sizeof(Int32);

            List<Point> newTail = new List<Point>();

            for (int i = 0; i < TailCount; i++)
            {
                int tailX = BitConverter.ToInt32(Serialized, readBytes);
                readBytes += sizeof(Int32);
                int tailY = BitConverter.ToInt32(Serialized, readBytes);
                readBytes += sizeof(Int32);

                newTail.Add(new Point(tailX, tailY));

            }

            _tail = newTail;

        }

        public bool isNetworkDirty()
        {
            return _networkDirty;
        }

        public void setNetworkClean(bool clean)
        {
            _networkDirty = !clean;
        }
    }
}
