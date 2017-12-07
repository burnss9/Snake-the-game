using System;
using SnakeGame;
using NUnit.Framework;

namespace SnakeTest
{
    [TestFixture]
    public class GameTest
    {
        [Test]
        [Ignore("Keep files in use, doesn't let you build/stuff")]
        public void GameRegisterFruitTest()
        {

            Game g = new Game(10, 10, 0, "", false);
            Assert.AreEqual(1, g.gameField.Fruits.Count);

            g.RegisterFruit(new ObjectNetID(0, 0), false);
            //1 from construction, 1 from the above line
            Assert.AreEqual(2, g.gameField.Fruits.Count);

            g.networkManager.setActive(false);
            g.networkManager = null;

            g.Close();
            g = null;
        }


    }
}
