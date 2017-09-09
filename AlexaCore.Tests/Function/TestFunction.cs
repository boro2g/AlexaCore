using AlexaCore.Intents;

namespace AlexaCore.Tests.Function
{
    class TestFunction : AlexaFunction
    {
        protected override IntentFactory IntentFactory()
        {
            return new TestFunctionIntentFactory();
        }
    }
}
