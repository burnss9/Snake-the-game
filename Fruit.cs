﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using OpenTK.Graphics.OpenGL;

namespace SnakeGame
{
    public class Fruit : IDrawable
    {
        private GameField _gameField;
        private Point _position;

        private static Texture _appleTex = null;

        public Point Position
        {
            get { return _position; }
        }

        //Fruit constuctor that takes Point and GameField parameters
        public Fruit(Point point, GameField gameField)
        {
            _position = new Point(point.X, point.Y);
            SetField(gameField);

            if(_appleTex == null)
            {
                _appleTex = Texture.LoadFromFile("textures/apple.png");
            }
        }

        //Set the game field
        public void SetField(GameField aGameField)
        {
            _gameField = aGameField;
        }

        //return the game field
        public GameField getField()
        {
            return _gameField;
        }

        //Reset the position of the fruit
        public void ResetPosition(Point point)
        {
            _position = new Point(point.X, point.Y);
        }

        //Draw the fruit texture
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
    }
}
