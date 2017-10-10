using System.Linq;
using AlexaCore.Tests.Function;
using NUnit.Framework;

namespace AlexaCore.Tests
{
    [TestFixture]
    class CustomIntentParametersTests
    {
        [Test]
        public void WhenCustomParametersAreUsed_CustomQueuesAreAvailable()
        {
            var parameters = new TestFunctionTestRunner()
                .RunInitialFunction("LaunchIntent")
                .Parameters() as TestIntentParameters;

            var testQueue = parameters.TestQueue;

            Assert.That(testQueue, Is.Not.Null);
        }

        [Test]
        public void WhenCustomParameterQueueIsUsed_QueueContainsValidEntries()
        {
            var parameters = new TestFunctionTestRunner()
                .RunInitialFunction("TestParameterIntent")
                .RunAgain("TestParameterIntent")
                .Parameters() as TestIntentParameters;

            var testQueue = parameters.TestQueue;

            Assert.That(testQueue.Entries().Count(), Is.EqualTo(4));

            Assert.That(testQueue.Entries(), Contains.Item("first"));

            Assert.That(testQueue.Entries(), Contains.Item("second"));
        }
    }
}
