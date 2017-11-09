using System;
using SnakeGame;
using NUnit.Framework;

namespace SnakeTest
{
    [TestFixture]
    public class PointTest
    {
        //Create 2 Points at 0,0 and make sure they are both at 0,0, and that they'e not the same object
        [Test]
        public void TestPointEqual()
        {
            Point p = new Point(0, 0);
            Point p2 = new Point(0, 0);

            Assert.IsTrue(p.Equals(p2));
            Assert.AreNotSame(p, p2);
        }

        //Create 4 Points at different positions and make sure they aren't in the same position
        [Test]
        public void TestPointNotEqual()
        {
            Point p = new Point(0, 0);
            Point p2 = new Point(0, 1);
            Point p3 = new Point(1, 0);
            Point p4 = new Point(1, 1);

            Assert.IsFalse(p.Equals(p2));
            Assert.IsFalse(p.Equals(p3));
            Assert.IsFalse(p.Equals(p4));
            
        }
    }
}
