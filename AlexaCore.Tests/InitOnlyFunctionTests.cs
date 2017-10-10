using AlexaCore.Tests.Function;
using NUnit.Framework;

namespace AlexaCore.Tests
{
    [TestFixture]
    class InitOnlyFunctionTests
    {
        [Test]
        public void WhenInitOnlyReturnsResponse_FunctionEnds()
        {
            new InitOnlyFunctionTestRunner()
                .RunInitialFunction("LaunchIntent")
                .VerifyOutputSpeechValue("InitOnly");
        }
    }
}
