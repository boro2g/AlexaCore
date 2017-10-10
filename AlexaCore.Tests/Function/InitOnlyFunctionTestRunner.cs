using AlexaCore.Testing;

namespace AlexaCore.Tests.Function
{
    class InitOnlyFunctionTestRunner : AlexaCoreTestRunner<InitOnlyFunctionTestRunner>
    {
        public override AlexaFunction BuildFunction()
        {
            return new InitOnlyFunction();
        }
    }
}