using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using OpenTK.Input;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    class InGameMenu : GameWindow
    {

        private static Texture _texture = null;

        private static Game currentGame = null;
        private static NetworkManager currentNetworkManager = null;

        UIComponent SelectedElement;
        UIComponent _resume, _mainMenu, _exit;

        public InGameMenu(Game current)
        {

            currentGame = current;
            currentNetworkManager = currentGame.networkManager;

            _texture = Texture.LoadFromFile("textures/grass.png");
            _resume = new Button("textures/resume.png", 200, 100);
            _mainMenu = new Button("textures/main_menu.png", 200, 200);
            _exit = new Button("textures/exit.png", 200, 300);
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
            GL.TexCoord2(0, Height / 3);
            GL.Vertex3(0, Height * 32, -8);
            GL.TexCoord2(Width / 3, Height / 3);
            GL.Vertex3(Width * 32, Height * 32, -8);

            GL.TexCoord2(Width / 3, 0);
            GL.Vertex3(Width * 32, 0, -8);

            GL.End();
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
        //Graphics stuff
        protected override void OnRenderFrame(FrameEventArgs e)
        {
            base.OnRenderFrame(e);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            Matrix4 modelview = Matrix4.LookAt(Vector3.Zero, -Vector3.UnitZ, Vector3.UnitY);
            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref modelview);

            //Meat

            this.Draw();
            _resume.Draw();
            _mainMenu.Draw();
            _exit.Draw();


            SwapBuffers();
        }

        protected override void OnMouseDown(MouseButtonEventArgs e)
        {

            base.OnMouseDown(e);
            if (Mouse.X > 199 && Mouse.X < 400)
                if (Mouse.Y > 99 && Mouse.Y < 150)
                {
                    //Resume Game Clicked
                    currentGame.MakeCurrent();
                    this.Close();
                    
                }
                else if (Mouse.Y > 199 && Mouse.Y < 250)
                {
                    //Main Menu Clicked
                    this.Close();

                    currentGame.networkManager.setActive(false);
                    currentGame.networkManager.closeSocket();

                    currentGame.Close();
                    using (MainMenu main = new MainMenu())
                    {
                        main.Run();
                    }


                }
                else if (Mouse.Y > 299 && Mouse.Y < 350)
                {
                    this.Close();
                    currentGame.Close();
                    Environment.Exit(0);
                }
        }

    }
}
