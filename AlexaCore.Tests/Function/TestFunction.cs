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

        protected override SkillResponse FunctionInit(AlexaContext alexaContext, IntentParameters parameters)
        {
            AlexaContext.Container.RegisterType("globalItem", () => new TestDataStore("Function"));

            AlexaContext.Container.RegisterType(() => new TestDependency("concrete"));

            AlexaContext.Container.RegisterType<ITestDependency>(() => new TestDependency("interface"));

            return null;
        }

        protected override void RegisterDependencies(ContainerBuilder builder)
        {
            builder.Register(a => new TestDependency("bob")).As<ITestDependency>();
        }

        protected override IntentParameters BuildParameters(ILambdaLogger logger, Session session)
        {
            return new TestIntentParameters(logger, session);
        }
    }
}
