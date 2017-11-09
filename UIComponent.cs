using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    //New UI IDrawable Interface
    abstract class UIComponent : IDrawable
    {

        public Texture _texture = null;
        

        public abstract void Draw();
        public abstract void SetField(GameField aGameField);

        public abstract void OnClick();


    }
}
