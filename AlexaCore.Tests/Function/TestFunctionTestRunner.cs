using AlexaCore.Testing;

namespace AlexaCore.Tests.Function
{
    class TestFunctionTestRunner : AlexaCoreTestRunner
    {
        public override AlexaFunction BuildFunction()
        {
            return new TestFunction();
        }
    }
}
