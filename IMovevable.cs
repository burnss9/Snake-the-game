using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    //Enums for Snake direction
    public enum Direction { Stop = 0, Left, Right, Up, Down };

    //IMovable interface
    public interface IMovable
    {
        void SetDirection(Direction direction);
        void Move(bool forceUpdate = false);
    }
}
