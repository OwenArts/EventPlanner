using NUnit.Framework;
using Sandbox;

namespace EventPlanner;

public class UnittestTest
    {
        [Test]
        public void PassingTest()
        {
            Assert.That(4, Calculator.Add(2,2));
        }

        [Test]
        public void FailingTest()
        {
            Assert.That(true, 1, 2);
        }
    }
}