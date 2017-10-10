using Alexa.NET;
using Alexa.NET.Response;
using AlexaCore.Intents;

namespace AlexaCore.Tests.Function
{
    class InitOnlyFunction : AlexaFunction
    {
        protected override IntentFactory IntentFactory()
        {
            return new TestFunctionIntentFactory();
        }

        protected override SkillResponse FunctionInit(AlexaContext alexaContext, IntentParameters parameters)
        {
            return ResponseBuilder.Tell(new PlainTextOutputSpeech {Text = "InitOnly"});
        }
    }
}