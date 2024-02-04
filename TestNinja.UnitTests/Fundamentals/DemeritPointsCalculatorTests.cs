using System;
using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests.Fundamentals
{
    [TestFixture]
    public class DemeritPointsCalculatorTests
    {
        [Test]
        [TestCase(-1)]
        [TestCase(301)]
        public void CalculateDemeritPoints_SpeedOutOfRange_ThrowException(int speed)
        {
            var demeritPointsCalculator = new DemeritPointsCalculator();
            
            Assert.That(() => demeritPointsCalculator.CalculateDemeritPoints(speed), 
                Throws.Exception.TypeOf<ArgumentOutOfRangeException>());
        }
        
        [Test]
        [TestCase(0, 0)]
        [TestCase(64, 0)]
        [TestCase(65, 0)]
        [TestCase(66, 0)]
        [TestCase(70, 1)]
        [TestCase(75, 2)]
        [TestCase(300, 47)]
        public void CalculateDemeritPoints_SpeedInRange_ReturnDemeritPoints(int speed, 
            int expectedDemeritPoints)
        {
            var demeritPointsCalculator = new DemeritPointsCalculator();
            var actualDemeritPoints = demeritPointsCalculator.CalculateDemeritPoints(speed);
            
            Assert.That(actualDemeritPoints, Is.EqualTo(expectedDemeritPoints));
        }
    }
}