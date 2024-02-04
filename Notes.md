# NUnit Test in C# <!-- omit from toc -->
([Github project link](https://github.com/ribhattacharya/nunit-testing.git), [Udemy](https://ort.udemy.com/course/unit-testing-csharp/learn/lecture/8997840#overview))

- [Introduction](#introduction)
  - [Benefits of automated testing](#benefits-of-automated-testing)
  - [Types of tests](#types-of-tests)
  - [Test pyramid](#test-pyramid)
  - [The tooling](#the-tooling)
  - [Using NUnit in Visual Studio](#using-nunit-in-visual-studio)
  - [What is test-driven-development (TDD)](#what-is-test-driven-development-tdd)
- [Fundamentals of unit testing](#fundamentals-of-unit-testing)
  - [Characteristics of Good Unit Tests](#characteristics-of-good-unit-tests)
  - [What to Test and What Not to Test](#what-to-test-and-what-not-to-test)
  - [Naming and Organizing Tests](#naming-and-organizing-tests)
  - [Writing a Simple Unit Test](#writing-a-simple-unit-test)
  - [Set Up and Tear Down](#set-up-and-tear-down)
  - [Parameterized Tests](#parameterized-tests)
  - [Ignoring Tests](#ignoring-tests)
  - [Writing Trustworthy Tests](#writing-trustworthy-tests)
- [Core unit testing techniques](#core-unit-testing-techniques)
  - [Strings](#strings)
  - [Arrays/Collections](#arrayscollections)
  - [Return type of methods](#return-type-of-methods)
  - [Void methods](#void-methods)
  - [Methods that throw exceptions](#methods-that-throw-exceptions)
  - [Methods that raise events](#methods-that-raise-events)
  - [Private/protected methods](#privateprotected-methods)
  - [Code coverage](#code-coverage)
- [Breaking external dependencies (Integration tests)](#breaking-external-dependencies-integration-tests)
  - [Refactoring Towards a Loosely-coupled Design](#refactoring-towards-a-loosely-coupled-design)
  - [Dependency Injection via](#dependency-injection-via)
  - [Mocking Frameworks: Moq](#mocking-frameworks-moq)
  - [State-based vs. Interaction Testing](#state-based-vs-interaction-testing)
  - [Testing the Interaction Between Two Objects](#testing-the-interaction-between-two-objects)
  - [Fake as Little As Possible](#fake-as-little-as-possible)


## Introduction
### Benefits of automated testing
1. Test code frequently and in less time
2. Catch bugs before deploying; catching bugs later costs more money!
3. Deploy with confidence
4. Refactor with confidence
5. Focus more on quality

### Types of tests
1. Unit test: Testing unit of an app, **without** external dependencies.
    - Cheap and easy to write
    - Quick to execute
    - Not much confidence
2. Integration test: Testing unit of an app, **with** external dependencies.
    - Longer to write
    - Bit more confidence
3. End-To-End test: Drives an app through its UI.
    - Very slow
    - Very brittle
    - Greatest confidence

### Test pyramid
Should have all 3 kinds of tests in your testing implementation. Actual ratio amongst 3 should be based on your project.
- Favour unit tests to E2E tests.
- Cover unit test gaps with integration tests.
- Use E2E tests sparingly.
   
### The tooling
MSTest, NUnit, etc.


### Using NUnit in Visual Studio
    public class Math
    {
        public int Add(int a, int b)
        { 
            return a + b;
        }
    }

    [TestFixture]               // Required before every class
    public class MathTests
    {
        private Math _math;

        [OneTimeSetUp]          // One time setup, once for each test fixture
        public void OneTimeSetup()
        {
            _math = new Math();
        }
        
        [Test]                  // Required before each test method
        [TestCase(1, 2, 3)]     // Parameterizing tests
        [TestCase(-1, -2, -3)]
        public void Add_WhenCalled_ReturnsSumOfTwoArguments(int x, int y, int z)
        {
            // NUnit assertion; Assert.That is the newer way
            Assert.That(_math.Add(x,y), Is.EqualTo(z));   
        }
    }

### What is test-driven-development (TDD)
1. Write failing test
2. Write simplest code to make the tesst pass.
3. Refactor if necessary

Benefits
- Testable source code
- Full code coverage
- Simple implementation

## Fundamentals of unit testing
### Characteristics of Good Unit Tests
1. Are first class citizens: clean readable and maintianable code
2. No logic (loops, conditionals, etc): Will make it complicated and might have to debug later
3. Isolated: Tests should not call other tests, and should not assume states
4. Not too specific/general: Low code coverage and low confidence

### What to Test and What Not to Test
Functions are of two types
1. Query
2. Commands

Don't test
1. Language features
2. 3rd party code

Tests are generally organized into
- Arrange
- Act
- Assert

### Naming and Organizing Tests
- Each project `MyProject` has its tests in another sub-project `MyProject.UnitTests`.
- Each class `MyClass` (in `MyProject`) has a corressponding class (in `MyProject.UnitTests`) containing its tests `MyClassTests`.
- Test methods are named as `[NameOfMethodToTest]_[Scenario]_[ExpectedBehaviour]`.
- If a method has many logical branches, might as well combine all of those into a single test `class`. So a single method has a `class` of tests.

### Writing a Simple Unit Test
    public class Math
    {
        public int Add(int a, int b)
        { 
            return a + b;
        }
        
        public int Max(int a, int b)
        {
            return (a > b) ? a : b;
        }

        public IEnumerable<int> GetOddNumbers(int limit)
        {
            for (var i = 0; i <= limit; i++)
                if (i % 2 != 0)
                    yield return i; 
        }
    } 

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
            Assert.That(_math.GetOddNumbers(limit), Is.EqualTo(oddNumbers));
        }
    }

### Set Up and Tear Down
    [Setup]         // Run for every test
    [OneTimeSetup]  // Run for each test fixture
    
    [TearDown]         // Run for every test
    [OneTimeTearDown]  // Run for each test fixture

### Parameterized Tests
    [Test]
    [TestCase(1, 2, 3)]
    [TestCase(-1, -2, -3)]
    public void Add_WhenCalled_ReturnsSumOfTwoArguments(int x, int y, int z)
    {
        Assert.That(_math.Add(x,y), Is.EqualTo(z));
    }
Can only use **constants** for the parameterized arguments (in `TestCase()`). 

### Ignoring Tests
    [Ignore("reason to ignore")] // Ignores test and prints the reason

### Writing Trustworthy Tests
1. Use TDD
2. Change the line which is responsible for the test. If the test result does not change, the test is wrong.

## Core unit testing techniques
### Strings
    
        Assert.That(formatAsBold, Is.EqualTo($"hello{content}world"));
        Assert.That(formatAsBold, Does.StartWith("hello"));
        Assert.That(formatAsBold, Does.EndWith("world"));
        Assert.That(formatAsBold, Does.Contain(content));

### Arrays/Collections

        Assert.That(result, Does.Contain(5));
        Assert.That(result, Is.EquivalentTo(new [] {1, 5, 3}));
        Assert.That(result, Is.Not.Empty);
        Assert.That(result, Is.Ordered);
        Assert.That(result, Is.Unique);

### Return type of methods

        // Exactly this type
        Assert.That(result, Is.TypeOf<NotFound>());
        
        // This type or its derivatives
        Assert.That(result, Is.InstanceOf<Ok>());

### Void methods
Check some state (`LastError`) being changed inside the `void` method.
    
    public class ErrorLogger
    {
        public string LastError { get; set; }

        public event EventHandler<Guid> ErrorLogged; 
        
        public void Log(string error)
        {
            if (String.IsNullOrWhiteSpace(error))
                throw new ArgumentNullException();
                
            LastError = error; 
            
            // Write the log to a storage
            // ...

            ErrorLogged?.Invoke(this, Guid.NewGuid());
        }
    }

    [Test]
    public void Log_CalledWithoutNullOrWhiteSpace_LogsError()
    {
        const string log = "hello world";
        var errorLogger = new ErrorLogger();
        
        errorLogger.Log(log);
        
        Assert.That(errorLogger.LastError, Is.EqualTo(log));
    } 

### Methods that throw exceptions
    [Test]
    [TestCase(null)]
    [TestCase("")]
    [TestCase(" ")]
    public void Log_CalledWithNullOrWhiteSpace_ThrowsException(string error)
    {
        var errorLogger = new ErrorLogger();
        
        Assert.That(() => errorLogger.Log(error), Throws.ArgumentNullException);
    }

### Methods that raise events
    [Test]
    public void Log_ValidError_LogsEvent()
    {
        var errorLogger = new ErrorLogger();
        var id = Guid.Empty;

        errorLogger.ErrorLogged += (sender, args) => { id = args; }; 
        
        errorLogger.Log("a");
        
        Assert.That(id, Is.Not.EqualTo(Guid.Empty));
    }

### Private/protected methods
- Do not test these since they are implementation details.
- Can change from time to time.
- Only need to test the public API.

### Code coverage
How many of the execution paths have been covered?

## Breaking external dependencies (Integration tests)
### Refactoring Towards a Loosely-coupled Design
- Extract interfaces from classes and test using fake objects that implement the interface.
- Then use **dependency injection** to ease testing.

### Dependency Injection via
1. Method Parameters
  - Can invoke real and fake objects at will.
  - But ends up changing the method signature, will require refactoring across the project
2. Properties
   - Add a settable-property in the class.
   - Function signature does not change.
   - Need to set the property whenever new objects are to be injected.
3. Constructor (poor man's dependency injection)
   - Overload ctor to take in a default value (production code) or a fake value as argument (in the overloaded ctor) for testing.
   - No code is broken, since default ctor still works.
   - Can just pass in additional fake object as argument during testing.
4. Frameworks
   - Systematic way to create DI.
   - Recommemded for enterprise applications.

### Mocking Frameworks: Moq
For creating dynamic mock objects. Very useful since we don't have to create seperate mock objects for every little test.


### State-based vs. Interaction Testing
- State-based: Check the state that was changed using that method.
- Interaction: Check the interaction b/w two classes/entities.
  
### Testing the Interaction Between Two Objects
    [TestFixture]
    public class OrderServiceTests
    {
        [Test]
        public void PlaceOrder_OrderPlaced_OrderIsStored()
        {
            var storage = new Mock<IStorage>();
            var orderService = new OrderService(storage.Object);

            var order = new Order();
            orderService.PlaceOrder(order);
            
            // To test interaction b/w two objects
            storage.Verify(s=> s.Store(order));
        }
    }

### Fake as Little As Possible
Drawbacks of Mocks:
- Explosion of interfaces
- Explosion of ctor params
- Fat tests
- Fragile tests

