using System;
using SnakeGame;
using NUnit.Framework;

namespace SnakeGameTest
{
    [TestFixture]
    public class SnakeTest
    {
        //Affirm that SetDirection moves the Snake upwards by decreasing its Y value
        [Test]
        public void SnakeMoveUpTest()
        {
            Snake s = new Snake(new Point(10, 10), new GameField(20, 20, null, false), false);
            s.SetDirection(Direction.Up);
            s.Move(true);

            Assert.AreEqual(9, s.Head.Y);
            Assert.AreEqual(10, s.Head.X);

        }

        //Affirm that SetDirection moves the Snake downwards by increasing its Y value
        [Test]
        public void SnakeMoveDownTest()
        {
            Snake s = new Snake(new Point(10, 10), new GameField(20, 20, null, false), false);
            s.SetDirection(Direction.Down);
            s.Move(true);

            Assert.AreEqual(11, s.Head.Y);
            Assert.AreEqual(10, s.Head.X);
        }

        //Affirm that SetDirection moves the Snake to the right by increasing its X value
        [Test]
        public void SnakeMoveRightTest()
        {
            Snake s = new Snake(new Point(10, 10), new GameField(20, 20, null, false), false);
            s.SetDirection(Direction.Right);
            s.Move(true);

            Assert.AreEqual(11, s.Head.X);
            Assert.AreEqual(10, s.Head.Y);
        }

        //Affirm that SetDirection moves the Snake to the left by decreasing its X value
        [Test]
        public void SnakeMoveLeftTest()
        {
            Snake s = new Snake(new Point(10, 10), new GameField(20, 20, null, false), false);
            s.SetDirection(Direction.Left);
            s.Move(true);

            Assert.AreEqual(9, s.Head.X);
            Assert.AreEqual(10, s.Head.Y);
        }

        
        [Test]
        public void SnakeEatTest()
        {
            //Create a GameField and spawn a Fruit at 11,10
            GameField field = new GameField(20, 20, null, false);
            field.Fruits[0].ResetPosition(new Point(11, 10));

            //Create a Snake at 10,10
            Snake s = new Snake(new Point(10, 10), field, false);

            //Set the Snake's Direction to Right, and move twice
            s.SetDirection(Direction.Right);
            s.Move(true);
            s.Move(true);

            //Make sure Snake doesn't have 0 Tails
            Assert.AreNotEqual(0, s.Tail.Count);
            Assert.IsFalse(new Point(11, 10).Equals(field.Fruits[0].Position));
        }

    }
}
