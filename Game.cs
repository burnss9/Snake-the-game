using System;
using OpenTK;
using OpenTK.Graphics.OpenGL;
using OpenTK.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics;
using System.Drawing;

namespace SnakeGame
{
    public class Game : GameWindow
    {

        private GameField _gameField;
        public GameField gameField { get { return _gameField; } }

        private bool _gameOver;
        private int difficulty;//0 for normal mode, 1 for hard
        
        public NetworkManager networkManager;
        public bool hostGame = true;
        
        //Game constructor
        public Game(int aWidth, int aHeight, int difficulty, string ip = "") : base(32 * aWidth, 32 * aHeight)
        {
            this.difficulty = difficulty;
            _gameField = new GameField(aWidth, aHeight, this);

            hostGame = ip == "" ? true : false;

            networkManager = new NetworkManager(this, hostGame, 10, ip);

            if (hostGame)
            {
                networkManager.RegisterPlayer(null);
                networkManager.RegisterFruit();
            }
            _gameOver = false;
        }

        //Enable startup stuff when the game loads
        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            GL.ClearColor(0.1f, 0.2f, 0.5f, 0.0f);
            GL.Enable(EnableCap.DepthTest);

            GL.Enable(EnableCap.Texture2D);
            GL.Enable(EnableCap.AlphaTest);
            GL.AlphaFunc(AlphaFunction.Gequal, 0.5f);
        }

        //Window resizing to match user dimensions
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            GL.Viewport(ClientRectangle.X, ClientRectangle.Y, ClientRectangle.Width, ClientRectangle.Height);

            Matrix4 orth = Matrix4.CreateOrthographicOffCenter(0, Width, Height, 0, 1.0f, 64.0f);
            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref orth);
        }

        //Graphics stuff
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            //Meat
            {
                //Draw the GameField, Snake, and Fruit
                _gameField.Draw();

                foreach (Snake s in _gameField.Snakes)
                {
                    s.Draw();
                }

                foreach (Fruit f in _gameField.Fruits)
                {
                    f.Draw();
                }
                

            }

            SwapBuffers();
        }

        //Move the Snake each frame update
        protected override void OnUpdateFrame(FrameEventArgs e)
        {
            base.OnUpdateFrame(e);

            foreach (Snake s in _gameField.Snakes)
            {
                s.Tick(e.Time);
            }

            networkManager.NetworkTick();

        }
        
        byte[] serialized = null;
        //Keyboard input, WASD moves Snake up, down, left, and right
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            Snake turningSnake = null;
            foreach (Snake s in _gameField.Snakes)
            {
                if (s.objectNetID.OwnerID == networkManager.LocalClientID)
                {
                    turningSnake = s;
                    break;
                }
            }

            if (e.Key == Key.W)
            {
                if (turningSnake != null)
                {
                    turningSnake.SetDirection(Direction.Up);
                }
            }
            if (e.Key == Key.A)
            {
                if(turningSnake != null)
                {
                    turningSnake.SetDirection(Direction.Left);
                }
            }
            if (e.Key == Key.S)
            {
                if(turningSnake != null)
                {
                    turningSnake.SetDirection(Direction.Down);
                }
            }
            if (e.Key == Key.D)
            {
                if(turningSnake != null)
                {
                    turningSnake.SetDirection(Direction.Right);
                }
            }
            if(e.Key == Key.Escape)
            {
                using (InGameMenu inGame = new InGameMenu(this))
                    inGame.Run(120, 60);
            }
            if(e.Key == Key.Comma)
            {
                serialized = turningSnake.Serialize();
            }
            if (e.Key == Key.Period && serialized != null) 
            {
                turningSnake.Deserialize(serialized);
            }

        }

        public Fruit RegisterFruit(ObjectNetID objectNetID)
        {

            Fruit f = new Fruit(_gameField.RandomPointInField(), _gameField);

            _gameField.Fruits.Add(f);

            return f;
        }

        public Snake RegisterSnake(ObjectNetID ID)
        {

            Snake snake = new Snake(_gameField.RandomPointInField(), _gameField)
            {
                objectNetID = ID
            };

            Random r = new Random();

            snake.SetColor((byte)r.Next(256), (byte)r.Next(256), (byte)r.Next(256));

            _gameField.Snakes.Add(snake);
            return snake;
        }

    }


}
