using AlexaCore.Tests.Function;
using NUnit.Framework;

namespace AlexaCore.Tests
{
    [TestFixture]
    class IntentParameterTests
    {
        [Test]
        public void InputQueue_StringItemAddedCorrectly()
        {
            new TestFunctionTestRunner()
                .RunTest("LaunchIntent")
                .VerifySessionInputQueue("string");
        }

        [Test]
        public void InputQueue_StringItemWithTagsAddedCorrectly()
        {
            new TestFunctionTestRunner()
                .RunTest("LaunchIntent")
                .VerifySessionInputQueue("stringWithTags")
                .VerifySessionInputQueue("stringWithTags", new[] { "tag1", "tag2" });
        }

        [Test]
        public void CommandQueue_ItemAddedCorrectly()
        {
            new TestFunctionTestRunner()
                .RunTest("LaunchIntent")
                .VerifySessionCommandQueue("LaunchIntent");
        }

        [Test]
        public void ApplicationParameters_ItemAddedCorrectly()
        {
            new TestFunctionTestRunner()
                .RunTest("LaunchIntent")
                .VerifySessionApplicationParameters("name", "value");
        }
    }
}
