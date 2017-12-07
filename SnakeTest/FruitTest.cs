using System;
using SnakeGame;
using NUnit.Framework;

namespace SnakeTest
{
    [TestFixture]
    public class FruitTest
    {
        [Test]
        public void FruitResetPositionTest()
        {

            Fruit f = new Fruit(new Point(10, 10), null, false);

            f.ResetPosition(new Point(5, 5));

            Assert.IsTrue(f.isNetworkDirty());
            Assert.AreEqual(new Point(5, 5), f.Position);
        }
        

        [Test]
        public void FruitSerializeTest()
        {
            Fruit f = new Fruit(new Point(10, 10), null, false);
            var bytes = f.Serialize();

            Fruit f2 = new Fruit(new Point(1, 1), null, false);
            f2.Deserialize(bytes);

            Assert.AreEqual(f.Position, f2.Position);
            
        }
    }
}
