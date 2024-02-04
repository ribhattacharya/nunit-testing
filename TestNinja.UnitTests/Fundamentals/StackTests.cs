using NUnit.Framework;
using TestNinja.Fundamentals;

namespace TestNinja.UnitTests.Fundamentals
{
    [TestFixture]
    public class StackTests
    {
        private Stack<string> _stack;
        
        [SetUp]
        public void Setup()
        {
            _stack = new Stack<string>();
        }
        
        [Test]
        public void Push_ValidObject_PushElementToStack()
        {
            const string toPush = "a";
            
            _stack.Push(toPush);
            
            Assert.That(_stack.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void Push_InvalidObject_ThrowException()
        {
            Assert.That(() => _stack.Push(null), Throws.ArgumentNullException);
        }
        
        [Test]
        public void Pop_EmptyStack_ThrowException()
        {
            Assert.That(() => _stack.Pop(), Throws.InvalidOperationException);
        }
        
        [Test]
        public void Pop_NonEmptyStack_RetrieveLastElement()
        {
            const string toPushA = "a", toPushB = "b";

            _stack.Push(toPushA);
            _stack.Push(toPushB);

            var top = _stack.Pop();
            
            Assert.That(top, Is.EqualTo(toPushB));
            Assert.That(_stack.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void Peek_EmptyStack_ThrowException()
        {
            Assert.That(() => _stack.Peek(), Throws.InvalidOperationException);
        }
        
        [Test]
        public void Peek_NonEmptyStack_RetrieveLastElement()
        {
            const string toPush = "a";
            
            _stack.Push(toPush);
            var top = _stack.Peek();
            
            Assert.That(top, Is.EqualTo(toPush));
            Assert.That(_stack.Count, Is.EqualTo(1));
        }
        
        [Test]
        public void Count_EmptyStack_CountIsZero()
        {
            Assert.That(_stack.Count, Is.EqualTo(0));
        }
        
        [Test]
        public void Count_NonEmptyStack_CountIsOne()
        {
            const string toPush = "a";
            
            _stack.Push(toPush);
            
            Assert.That(_stack.Count, Is.EqualTo(1));
        }
    }
}