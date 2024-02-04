using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests.Fundamentals
{
    [TestFixture]
    public class FizzBuzzTests
    {
        [Test]
        [TestCase(15, "FizzBuzz")]
        [TestCase(6, "Fizz")]
        [TestCase(10, "Buzz")]
        [TestCase(11, "11")]
        public void GetOutput_DivBy3And5_Return(int n, string expectedResult)
        {
            var actualResult = FizzBuzz.GetOutput(n);

            Assert.That(actualResult, Is.EqualTo(expectedResult));
        }
    }
}