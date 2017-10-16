﻿using System;
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
            Snake s = new Snake(new Point(10, 10), new GameField(20, 20));
            s.SetDirection(Direction.Up);
            s.Move();

            Assert.AreEqual(9, s.Head.Y);
            Assert.AreEqual(10, s.Head.X);

        }

        //Affirm that SetDirection moves the Snake downwards by increasing its Y value
        [Test]
        public void SnakeMoveDownTest()
        {
            Snake s = new Snake(new Point(10, 10), new GameField(20, 20));
            s.SetDirection(Direction.Down);
            s.Move();

            Assert.AreEqual(11, s.Head.Y);
            Assert.AreEqual(10, s.Head.X);
        }

        //Affirm that SetDirection moves the Snake to the right by increasing its X value
        [Test]
        public void SnakeMoveRightTest()
        {
            Snake s = new Snake(new Point(10, 10), new GameField(20, 20));
            s.SetDirection(Direction.Right);
            s.Move();

            Assert.AreEqual(11, s.Head.X);
            Assert.AreEqual(10, s.Head.Y);
        }

        //Affirm that SetDirection moves the Snake to the left by decreasing its X value
        [Test]
        public void SnakeMoveLeftTest()
        {
            Snake s = new Snake(new Point(10, 10), new GameField(20, 20));
            s.SetDirection(Direction.Left);
            s.Move();

            Assert.AreEqual(9, s.Head.X);
            Assert.AreEqual(10, s.Head.Y);
        }

        
        [Test]
        public void SnakeEatTest()
        {
<<<<<<< HEAD

            //Create a GameField and spawn a Fruit at 11,10
            GameField field = new GameField(20, 20);
            field.Fruits[0].ResetPosition(new Point(11, 10));

            //Create a Snake at 10,10
            Snake s = new Snake(new Point(10, 10), field);

            //Set the Snake's Direction to Right, and move twice
=======
            GameField field = null;
            try
            {
                 field = new GameField(20, 20);
            }
            catch (System.ArgumentException ex) { Console.Write("fuck, that wasn't supposed to happen");}

            field.Fruits[0].ResetPosition(new Point(11, 10));
            Snake s = new Snake(new Point(10, 10), field);
>>>>>>> 4dc72c0953fc22d78da5379359f96d62f134694f
            s.SetDirection(Direction.Right);
            s.Move();
            s.Move();

            //Make sure Snake doesn't have 0 Tails
            Assert.AreNotEqual(0, s.Tail.Count);
            Assert.IsFalse(new Point(11, 10).Equals(field.Fruits[0].Position));
        }

    }
}
