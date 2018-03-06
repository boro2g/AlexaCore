namespace AlexaCore.Tests.Function
{
    internal interface ITestDependency
    {
        string GetData();
    }

    class TestDependency : ITestDependency
    {
        private readonly string _name;

        public TestDependency(string name)
        {
            _name = name;
        }

        public string GetData()
        {
            return _name;
        }
    }
}