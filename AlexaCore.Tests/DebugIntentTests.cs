using AlexaCore.Tests.Function;
using NUnit.Framework;

namespace AlexaCore.Tests
{
    [TestFixture]
    class DebugIntentTests
    {
        [Test]
        public void WhenDebugIntentIsEnabled_ValidTextIsOutput()
        {
            var debugIntentName = new IntentNames().DefaultDebugIntent;

            var response = new TestFunctionTestRunner()
                .RunInitialFunction(debugIntentName)
                .GetOutputSpeechValue();

            Assert.That(response.Contains(debugIntentName), Is.True);
        }
    }
}
