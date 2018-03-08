using AlexaCore.Testing;

namespace AlexaCore.Tests.Function
{
    class TestFunctionTestRunner : AlexaCoreTestRunner<TestFunctionTestRunner>
    {
        private bool _performRegisterTypes;

        public override AlexaFunction BuildFunction()
        {
            _performRegisterTypes = false;

            return new TestFunction();
        }

        public TestFunctionTestRunner PerformRegisterTypes(bool value)
        {
            _performRegisterTypes = value;

            return this;
        }

        public TestFunctionTestRunner DoSomethingSpecificToThisImplementation()
        {
            return this;
        }

        protected override void RegisterTypes()
        {
            if (_performRegisterTypes)
            {
                //AlexaContext.Container.RegisterType("globalItem", () => new TestDataStore("override"), true);
            }
        }
    }
}
