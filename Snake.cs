﻿using OpenTK;
using OpenTK.Graphics.OpenGL;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Snake : IMovable, IDrawable
    {
        //variable declarations
        private Direction Dir = Direction.Up;

        private GameField _gameField;

        private static Texture _snakeHeadTex = null;
        private static Texture _snakeBodyTex = null;

        Vector4 _snakeColor = new Vector4(175/255f, 96/255f, 255/255f, 1);

        public Point Head { get; set; }
        private List<Point> _tail = new List<Point>();
        public List<Point> Tail
        {
            get { return _tail; }
        }
    
        //Snake constructor, taking Point and GameField parameters
        public Snake(Point point, GameField gameField)
        {
            Head = new Point(point.X, point.Y);
            SetField(gameField);

            //Set snake head and tail textures if they're null
            if(_snakeHeadTex == null)
            {
               _snakeHeadTex = Texture.LoadFromFile("textures/snakehead.png");
            }
            if(_snakeBodyTex == null)
            {
               _snakeBodyTex = Texture.LoadFromFile("textures/snakebody.png");
            }

        }

        //GameField setter
        public void SetField(GameField aGameField)
        {
            _gameField = aGameField;
        }

        //Eat fruit when snake touches it
        public void Eat(Fruit f)
        {
            _tail.Insert(0, new Point(Head.X,Head.Y));
            f.ResetPosition(_gameField.RandomPointInField());
        }

        //Set snake's movement direction
        public void SetDirection(Direction direction)
        {
            Dir = direction;
        }
        
        //Move the snake
        public void Move()
        {
            _tail.Insert(0, new Point(Head.X, Head.Y));
            _tail.RemoveAt(_tail.Count - 1);

            //When the head and fruit are in the same position, eat the fruit
            if (Head.Equals(_gameField.Fruits[0].Position))
            {
                Eat(_gameField.Fruits[0]);
            }

            //Set the direction of the head
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

            //Wrap snake around if it goes beyond the game field
            if (Head.X > _gameField.Width - 1) Head.X = 0;
            if (Head.X < 0) Head.X = _gameField.Width - 1;
            if (Head.Y > _gameField.Height - 1) Head.Y = 0;
            if (Head.Y < 0) Head.Y = _gameField.Height - 1;
        }

        //Draw the snake's textures
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
            GL.Vertex3(32,  32, -6);
            GL.TexCoord2(1, 0);
            GL.Vertex3( 32, 0, -6);

            GL.End();
            GL.PopMatrix();
            
            GL.BindTexture(TextureTarget.Texture2D, _snakeBodyTex.ID);

            //Snake tail texture
            foreach(Point p in Tail)
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
    }
}
