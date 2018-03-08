using AlexaCore.Testing;

namespace AlexaCore.Tests.Function
{
    class TestFunctionTestRunner : AlexaCoreTestRunner<TestFunctionTestRunner>
    {
        private bool _performRegisterTypes;

        public override AlexaFunction BuildFunction()
        {
            return SelectFunction(_performRegisterTypes);
        }

        public TestFunctionTestRunner PerformRegisterTypes(bool value)
        {
            _performRegisterTypes = value;

            UpdateFunction(SelectFunction(_performRegisterTypes));

            return this;
        }

        public TestFunctionTestRunner DoSomethingSpecificToThisImplementation()
        {
            return this;
        }

        private AlexaFunction SelectFunction(bool selectMockFunction)
        {
            return selectMockFunction ? new MockTestFunction() : new TestFunction();
        }
    }
}
