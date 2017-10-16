﻿using System;
using NUnit.Framework;
using SnakeGame;
using System.Collections.Generic;

namespace SnakeGameTest
{
    [TestFixture]
    public class GameFieldTest
    {
<<<<<<< HEAD
        //old test for console ui
        [Test]
=======
        /*[Test]
>>>>>>> 4dc72c0953fc22d78da5379359f96d62f134694f
        public void InitFieldTest()
        {
            GameField gameField = new GameField(10, 10);
            
            Assert.AreEqual(gameField.Field.Count, 10);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(gameField.Field[i].Count, 10);
                for (int j = 0; j < 10; j++)
                {
                    Assert.AreEqual(" ", gameField.Field[i][j].Val);
                    Assert.AreEqual(System.ConsoleColor.White, gameField.Field[i][j].Color);
                }
            }
        }

        //old test for console ui
        [Test]
        public void ClearFieldTest()
        {
            GameField gameField = new GameField(10, 10);
            
            foreach(var list in gameField.Field)
            {
                foreach(var cell in list)
                {
                    cell.Val = "a";
                    cell.Color = System.ConsoleColor.Blue;
                }
            }

            gameField.ClearField();

            Assert.AreEqual(gameField.Field.Count, 10);
            for (int i = 0; i < 10; i++)
            {
                Assert.AreEqual(gameField.Field[i].Count, 10);
                for (int j = 0; j < 10; j++)
                {
                    Assert.AreEqual(" ", gameField.Field[i][j].Val);
                    Assert.AreEqual(System.ConsoleColor.White, gameField.Field[i][j].Color);
                }
            }

        }*/

        //Random point test
        [Test]
        public void RandomPointInFieldTest()
        {
            GameField gameField = new GameField(10, 10);

            for(int i = 0; i < 100; i++)
            {
                Point point = gameField.RandomPointInField();

                Assert.That(point.X >= 0 && point.X < 10);
                Assert.That(point.Y >= 0 && point.Y < 10);
            }
        }
<<<<<<< HEAD

        //old test for console UI drawing
        [Test]
=======
        //obsolete
        /*[Test]
>>>>>>> 4dc72c0953fc22d78da5379359f96d62f134694f
        public void DrawTest()
        {
            GameField gameField = new GameField(5, 5);

            using (System.IO.StringWriter stringWriter = new System.IO.StringWriter())
            {
                Console.SetOut(stringWriter);

                gameField.Draw();

                string expected =   "#######\r\n" +
                                    "#     #\r\n" +
                                    "#     #\r\n" +
                                    "#     #\r\n" +
                                    "#     #\r\n" +
                                    "#     #\r\n" +
                                    "#######\r\n";
                
                string consoleOutput = stringWriter.ToString();

                Assert.AreEqual(expected, consoleOutput);
            }
        }*/

    }
}
