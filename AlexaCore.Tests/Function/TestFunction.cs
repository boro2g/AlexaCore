using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;
using Amazon.Lambda.Core;
using Autofac;

namespace AlexaCore.Tests.Function
{
    class TestFunction : AlexaFunction
    {
        protected override IntentFactory IntentFactory()
        {
            return new TestFunctionIntentFactory();
        }

        protected override SkillResponse FunctionInit(IntentParameters parameters)
        {
            return null;
        }

        protected override void RegisterDependencies(ContainerBuilder builder, IntentParameters parameters)
        {
            builder.Register(a => new TestDependency("interface")).As<ITestDependency>();

            builder.Register(a => new TestDependency("concrete"));

            builder.Register(a => new TestDataStore("Function")).Named<ITestDataStore>("globalItem");
        }

        protected override IntentParameters BuildParameters(ILambdaLogger logger, Session session)
        {
            return new TestIntentParameters(logger, session);
        }
    }
}
