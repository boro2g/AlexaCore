using AlexaCore.Tests.Function;
using NUnit.Framework;

namespace AlexaCore.Tests
{
    [TestFixture]
    class IntentParameterTests
    {
        private const string LaunchIntentName = "LaunchIntent";

        [Test]
        public void InputQueue_StringItemAddedCorrectly()
        {
            new TestFunctionTestRunner()
                .RunInitialFunction(LaunchIntentName)
                .VerifySessionInputQueue("string");
        }

        [Test]
        public void InputQueue_StringItemWithTagsAddedCorrectly()
        {
            new TestFunctionTestRunner()
                .RunInitialFunction(LaunchIntentName)
                .VerifySessionInputQueue("stringWithTags")
                .VerifySessionInputQueue("stringWithTags", new[] { "tag1", "tag2" });
        }

        [Test]
        public void CommandQueue_ItemAddedCorrectly()
        {
            new TestFunctionTestRunner()
                .RunInitialFunction(LaunchIntentName)
                .VerifySessionCommandQueue(LaunchIntentName);
        }

        [Test]
        public void CommandQueue_CommandKeyCanBeChanged()
        {
            new TestFunctionTestRunner()
                .RunInitialFunction(LaunchIntentName)
                .RunAgain("CommandQueueNameChangerIntent")
                .VerifySessionCommandQueue("FakeCommandDefinitionName");
        }

        [Test]
        public void ApplicationParameters_ItemAddedCorrectly()
        {
            new TestFunctionTestRunner()
                .RunInitialFunction(LaunchIntentName)
                .VerifySessionApplicationParameters("name", "value");
        }

        [Test]
        public void ApplicationParameters_ItemGetsUpdatedCorrectly()
        {
            new TestFunctionTestRunner()
                .RunInitialFunction(LaunchIntentName)
                .VerifySessionApplicationParameters("name", "value")
                .RunAgain("ParameterUpdaterIntent")
                .VerifySessionApplicationParameters("name", "value updated");
        }
    }
}
