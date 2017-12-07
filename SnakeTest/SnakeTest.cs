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

            Fruit f = new Fruit(field.RandomPointInField(), field, false);

            field.Fruits.Add(f);

            field.Fruits[0].ResetPosition(new Point(11, 10));

            //Create a Snake at 10,10
            Snake s = new Snake(new Point(10, 10), field, false);

            //Set the Snake's Direction to Right, and move twice
            s.SetDirection(Direction.Right);
            s.Move(true);
            s.Move(true);

            //Make sure Snake doesn't have 0 Tails
            Assert.AreEqual(1, s.Tail.Count);
            Assert.IsFalse(new Point(11, 10).Equals(field.Fruits[0].Position));
        }


        [Test]
        public void SnakeDieTest()
        {
            //Create a GameField and spawn a Fruit at 11,10
            GameField field = new GameField(20, 20, null, false);

            Fruit f = new Fruit(field.RandomPointInField(), field, false);

            field.Fruits.Add(f);

            field.Fruits[0].ResetPosition(new Point(11, 10));

            //Create a Snake at 10,10
            Snake s = new Snake(new Point(10, 10), field, false);

            field.Snakes.Add(s);

            //Set the Snake's Direction to Right, and move twice
            s.SetDirection(Direction.Right);
            s.Move(true);
            s.Move(true);
            field.Fruits[0].ResetPosition(new Point(12, 10));
            s.Move(true);
            field.Fruits[0].ResetPosition(new Point(13, 10));
            s.Move(true);
            field.Fruits[0].ResetPosition(new Point(14, 10));
            s.Move(true);
            field.Fruits[0].ResetPosition(new Point(15, 10));
            s.Move(true);
            field.Fruits[0].ResetPosition(new Point(16, 10));
            s.Move(true);
            field.Fruits[0].ResetPosition(new Point(17, 10));

            s.Move(true);
            s.SetDirection(Direction.Up);
            s.Move(true);
            s.SetDirection(Direction.Left);
            s.Move(true);
            s.SetDirection(Direction.Down);
            s.Move(true);

            //Make sure Snake died
            Assert.AreEqual(0, s.Tail.Count);
        }

        [Test]
        public void SnakeSerializeTest()
        {
            //Create a GameField and spawn a Fruit at 11,10
            GameField field = new GameField(20, 20, null, false);

            Fruit f = new Fruit(field.RandomPointInField(), field, false);

            field.Fruits.Add(f);

            field.Fruits[0].ResetPosition(new Point(11, 10));

            //Create a Snake at 10,10
            Snake s = new Snake(new Point(10, 10), field, false);

            field.Snakes.Add(s);

            //Set the Snake's Direction to Right, and move twice
            s.SetDirection(Direction.Right);
            s.Move(true);
            s.Move(true);
            field.Fruits[0].ResetPosition(new Point(12, 10));
            s.Move(true);
            field.Fruits[0].ResetPosition(new Point(13, 10));
            s.Move(true);
            field.Fruits[0].ResetPosition(new Point(14, 10));
            s.Move(true);
            s.SetDirection(Direction.Up);
            s.Move(true);
            s.SetDirection(Direction.Left);
            s.Move(true);

            var bytes = s.Serialize();

            Snake s2 = new Snake(new Point(0, 0), field, false);
            s2.Deserialize(bytes);

            //Make sure s and s2 are equal
            Assert.AreEqual(s.Tail.Count, s2.Tail.Count);
            Assert.AreEqual(s.Head, s2.Head);
            Assert.AreEqual(s.Dir, s2.Dir);
            Assert.AreEqual(s.SnakeColor, s2.SnakeColor);

            for (int i = 0; i < s.Tail.Count; i++)
            {
                Assert.AreEqual(s.Tail[i], s2.Tail[i]);
            }

        }


        [Test]
        public void SnakeRespawnTest()
        {
            //Create a GameField and spawn a Fruit at 11,10
            GameField field = new GameField(20, 20, null, false);

            Fruit f = new Fruit(field.RandomPointInField(), field, false);

            field.Fruits.Add(f);

            field.Fruits[0].ResetPosition(new Point(11, 10));

            //Create a Snake at 10,10
            Snake s = new Snake(new Point(10, 10), field, false);

            field.Snakes.Add(s);

            s.SetDirection(Direction.Right);
            s.Move(true);
            s.Move(true);
            field.Fruits[0].ResetPosition(new Point(12, 10));
            s.Move(true);

            s.Respawn();

            Assert.AreNotEqual(new Point(10, 10), s.Head);
            Assert.AreEqual(0, s.Tail.Count);

        }

        [Test]
        public void SnakeSetDirectionTest()
        {
            //Create a GameField and spawn a Fruit at 11,10
            GameField field = new GameField(20, 20, null, false);

            Fruit f = new Fruit(field.RandomPointInField(), field, false);

            field.Fruits.Add(f);

            field.Fruits[0].ResetPosition(new Point(11, 10));

            //Create a Snake at 10,10
            Snake s = new Snake(new Point(10, 10), field, false);

            field.Snakes.Add(s);

            s.SetDirection(Direction.Right);
            s.Move(true);
            s.Move(true);
            field.Fruits[0].ResetPosition(new Point(12, 10));
            s.Move(true);
            field.Fruits[0].ResetPosition(new Point(13, 10));
            s.Move(true);


            s.SetDirection(Direction.Right);
            s.Move(true);
            s.SetDirection(Direction.Left);
            s.Move(true);
            Assert.AreNotEqual(Direction.Left, s.Dir);

            s.SetDirection(Direction.Up);
            s.Move(true);
            s.SetDirection(Direction.Down);
            s.Move(true);
            Assert.AreNotEqual(Direction.Down, 
                s.Dir);

            s.SetDirection(Direction.Left);
            s.Move(true);
            s.SetDirection(Direction.Right);
            s.Move(true);
            Assert.AreNotEqual(Direction.Right, s.Dir);

            s.SetDirection(Direction.Down);
            s.Move(true);
            s.SetDirection(Direction.Up);
            s.Move(true);
            Assert.AreNotEqual(Direction.Up, s.Dir);

        }


    }
}
