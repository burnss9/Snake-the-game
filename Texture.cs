using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK.Graphics.OpenGL;
using System.Drawing;
using System.Drawing.Imaging;

namespace SnakeGame
{
    class Texture
    {
        //Texture's ID, width, and height
        private int id;
        private int width, height;

        //Getters for the above variables
        public int ID { get { return id; } }
        public int Width { get { return width; } }
        public int Height { get { return height; } }

        //Texture constructor
        public Texture(int id, int width, int height)
        {
            this.id = id;
            this.width = width;
            this.height = height;
        }

        //Load a texture from the path of a local file
        public static Texture LoadFromFile(string path)
        {
            Bitmap b = new Bitmap(path);
            int tid = GL.GenTexture();
            GL.BindTexture(TextureTarget.Texture2D, tid);

            BitmapData data = b.LockBits(new Rectangle(0, 0, b.Width, b.Height), ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb); 
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, b.Width, b.Height, 0, OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, data.Scan0);
            if (b != null)
            {
                b.UnlockBits(data);
            }

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter, (int)TextureMinFilter.Linear);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter, (int)TextureMagFilter.Linear);

            return new Texture(tid, b.Width, b.Height);

        }


    }
}
