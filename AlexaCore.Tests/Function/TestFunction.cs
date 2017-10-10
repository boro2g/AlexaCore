using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;
using Amazon.Lambda.Core;

namespace AlexaCore.Tests.Function
{
    class TestFunction : AlexaFunction
    {
        protected override IntentFactory IntentFactory()
        {
            return new TestFunctionIntentFactory();
        }

        protected override SkillResponse FunctionInit(AlexaContext alexaContext, IntentParameters parameters)
        {
            AlexaContext.Container.RegisterType("globalItem", () => new TestDataStore("Function"));

            return null;
        }

        protected override IntentParameters BuildParameters(ILambdaLogger logger, Session session)
        {
            return new TestIntentParameters(logger, session);
        }
    }
}
