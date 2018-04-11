using AlexaCore.Tests.Function;
using NUnit.Framework;

namespace AlexaCore.Tests
{
    [TestFixture]
    class AllIntentTests
    {
        [Test]
        public void WhenAllIntentsAreRun_TestCompletes()
        {
            new TestFunctionTestRunner()
                .RunInitialFunction("LaunchIntent")
                .RunAllIntents();
        }

        [Test]
        public void WhenAllIntentsAreRunWithRandomSlots_TestCompletes()
        {
            new TestFunctionTestRunner()
                .RunInitialFunction("LaunchIntent")
                .RunAllIntents();
        }
    }
}