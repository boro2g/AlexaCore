using AlexaCore.Tests.Function;
using NUnit.Framework;

namespace AlexaCore.Tests
{
    [TestFixture]
    class TestRunnerTypeTests
    {
        [Test]
        public void WhenTestRunnerIsCast_ImplementationSpecificMethodsAreAvailable()
        {
            var testRunner = new TestFunctionTestRunner()
                .RunInitialFunction("DefaultDebugIntent")
                .DoSomethingSpecificToThisImplementation();

            Assert.That(testRunner.GetType(), Is.EqualTo(typeof(TestFunctionTestRunner)));
        }
    }
}
