using AlexaCore.Intents;

namespace AlexaCore.Tests.Function
{
    class TestFunction : AlexaFunction
    {
        protected override IntentFactory IntentFactory()
        {
            return new TestFunctionIntentFactory();
        }

        protected override void FunctionInit(AlexaContext alexaContext, IntentParameters parameters)
        {
            AlexaContext.Container.RegisterType("globalItem", () => new TestDataStore("Function"));
        }
    }
}
