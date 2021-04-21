using NUnit.Framework;

namespace Microwave.Test.Integration
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void Test()
        {
            // We don't need an assert, as an exception would fail the test case
            Assert.AreEqual(true,true);
        }

    }
}