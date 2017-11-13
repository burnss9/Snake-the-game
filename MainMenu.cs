using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SnakeGame
{
    //Beginnings of New UI 
    class MainMenu : IDrawable
    {

        UIComponent SelectedElement;
        UIComponent _join, _host;
        public MainMenu()
        {
            _join = new Button("textures/join.png");
            _host = new Button("textures/host.png");
        }

        public void Draw()
        {
            _host.Draw();
            _join.Draw();
        }

        public void SetField(GameField aGameField)
        {

        }


    }
}
