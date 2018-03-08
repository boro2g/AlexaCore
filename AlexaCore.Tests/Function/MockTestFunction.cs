using Autofac;

namespace AlexaCore.Tests.Function
{
    class MockTestFunction : TestFunction
    {
        protected override void RegisterDependencies(ContainerBuilder builder, IntentParameters parameters)
        {
            base.RegisterDependencies(builder, parameters);

            builder.Register(a => new TestDataStore("override")).Named<ITestDataStore>("globalItem");
        }
    }
}