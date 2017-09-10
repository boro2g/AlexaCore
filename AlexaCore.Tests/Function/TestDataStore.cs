namespace AlexaCore.Tests.Function
{
    interface ITestDataStore
    {
        string GetData();

        string Name { get; }
    }

    class TestDataStore : ITestDataStore
    {
        public string Name { get; }

        public TestDataStore(string name)
        {
            Name = name;
        }

        public string GetData()
        {
            return "hi";
        }
    }
}