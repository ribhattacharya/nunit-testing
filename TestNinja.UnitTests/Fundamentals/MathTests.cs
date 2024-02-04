using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests.Fundamentals
{
    [TestFixture]
    public class MathTests
    {
        private Math _math;

        [SetUp]
        public void Setup()
        {
            _math = new Math();
        }
        
        [Test]
        [TestCase(1, 2, 3)]
        [TestCase(-1, -2, -3)]
        public void Add_WhenCalled_ReturnsSumOfTwoArguments(int x, int y, int z)
        {
            Assert.That(_math.Add(x,y), Is.EqualTo(z));
        }
        
        [Test]
        [TestCase(1, 2, 2)]
        [TestCase(-1, -2, -1)]
        [TestCase(3, 3, 3)]
        public void Max_WhenCalled_ReturnsMaxOfTwoArguments(int x, int y, int z)
        {
            Assert.That(_math.Max(x,y), Is.EqualTo(z));
        }

        [Test]
        [TestCase(5, new [] {1, 3, 5})]
        [TestCase(8, new [] {1, 3, 5, 7})]
        [TestCase(1, new [] {1})]
        public void GetOddNumbers(int limit, int[] oddNumbers)
        {
            var result = _math.GetOddNumbers(limit);
            Assert.That(result, Is.EqualTo(oddNumbers));
        }
    }
}