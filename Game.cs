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
        private bool _gameOver;
        private int difficulty;//0 for normal mode, 1 for hard

        //Game constructor
        public Game(int aWidth, int aHeight, int difficulty) : base(32 * aWidth, 32 * aHeight)
        {
            this.difficulty = difficulty;
            _gameField = new GameField(aWidth, aHeight);

            if (difficulty == 0)
                _gameField.Snakes.Add(new Snake(new Point(aWidth / 2, aHeight / 2), _gameField));
            else _gameField.Snakes.Add(new SnakeHard(new Point(aWidth / 2, aHeight / 2), _gameField));


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

                foreach(Snake s in _gameField.Snakes)
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
            _gameField.ClearField();


            foreach (Snake s in _gameField.Snakes)
            {
                s.Move();
            }

        }

        //Keyboard input, WASD moves Snake up, down, left, and right
        protected override void OnKeyDown(KeyboardKeyEventArgs e)
        {
            base.OnKeyDown(e);

            if(e.Key == Key.W)
            {
                foreach(Snake s in _gameField.Snakes)
                {
                    s.SetDirection(Direction.Up);
                }
            }
            if (e.Key == Key.A)
            {
                foreach (Snake s in _gameField.Snakes)
                {
                    s.SetDirection(Direction.Left);
                }
            }
            if (e.Key == Key.S)
            {
                foreach (Snake s in _gameField.Snakes)
                {
                    s.SetDirection(Direction.Down);
                }
            }
            if (e.Key == Key.D)
            {
                foreach (Snake s in _gameField.Snakes)
                {
                    s.SetDirection(Direction.Right);
                }
            }

        }

    }


}
