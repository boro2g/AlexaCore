using Alexa.NET;
using Alexa.NET.Response;
using AlexaCore.Intents;
using Autofac;

namespace AlexaCore.Tests.Function
{
    class InitOnlyFunction : AlexaFunction
    {
        protected override IntentFactory IntentFactory()
        {
            return new TestFunctionIntentFactory();
        }

        protected override SkillResponse FunctionInit(IntentParameters parameters)
        {
            return ResponseBuilder.Tell(new PlainTextOutputSpeech {Text = "InitOnly"});
        }

        protected override void RegisterDependencies(ContainerBuilder builder, IntentParameters parameters)
        {
            builder.Register(a => new TestDependency("init only")).As<ITestDependency>();
        }
    }
}