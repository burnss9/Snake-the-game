using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    public class Cell
    {
        //old console ui code
        public static readonly int CELL_SIZE = 32;

        public string Val { get; set; }
        public ConsoleColor Color { get; set; }

        public Cell(string aVal, ConsoleColor aColor)
        {
            Val = aVal;
            Color = aColor;
        }
    }
}
