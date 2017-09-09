using AlexaCore.Tests.Function;
using NUnit.Framework;

namespace AlexaCore.Tests
{
    [TestFixture]
    class IntentFactoryReflectionTests
    {
        [Test]
        public void WhenReflectionIsUsed_CorrectIntentsAreLoaded()
        {
            string helpIntentName = new IntentNames().HelpIntent;

            new TestFunctionTestRunner()
                .RunTest(helpIntentName)
                .VerifyIntentIsLoaded("LaunchIntent")
                .VerifyIntentIsLoaded(helpIntentName);
        }
    }
}
