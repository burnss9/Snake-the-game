using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Point
    {
        //Point's X and Y coordinates
        public int X { get; set; }
        public int Y { get; set; }

        //Point constructor
        public Point(int aX, int aY)
        {
            X = aX;
            Y = aY;
        }

        //Point.Equals method, checks if this Point is equal to some other Point
        public override bool Equals(object o)
        {
            Point p = (Point)o;
            return X == p.X && Y == p.Y;
        }

    }
}
