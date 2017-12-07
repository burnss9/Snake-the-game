using System;
using NUnit.Framework;
using SnakeGame;
using System.Collections.Generic;

namespace SnakeGameTest
{
    [TestFixture]
    public class GameFieldTest
    {

        //Random point test
        [Test]
        public void RandomPointInFieldTest()
        {
            GameField gameField = new GameField(10, 10, null, false);

            for(int i = 0; i < 100; i++)
            {
                Point point = gameField.RandomPointInField();

                Assert.That(point.X >= 0 && point.X < 10);
                Assert.That(point.Y >= 0 && point.Y < 10);
            }
        }

        

    }
}
