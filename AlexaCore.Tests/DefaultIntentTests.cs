using Alexa.NET.Request.Type;
using AlexaCore.Tests.Function;
using NUnit.Framework;

namespace AlexaCore.Tests
{
    [TestFixture]
    public class DefaultIntentTests
    {
        [Test]
        public void WhenIntentNameIsMissing_HelpIntentIsUsed()
        {
            new TestFunctionTestRunner()
                .RunInitialFunction("MissingIntent")
                .VerifyOutputSpeechValue("HelpIntent");
        }

        [Test]
        public void WhenLaunchRequestIsSent_LaunchIntentIsUsed()
        {
            new TestFunctionTestRunner()
                .RunInitialFunction("IntentName", request: new LaunchRequest())
                .VerifyOutputSpeechValue("LaunchIntent");
        }
    }
}
