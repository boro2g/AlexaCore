using Alexa.NET.Request;
using Amazon.Lambda.Core;

namespace AlexaCore.Tests.Function
{
    class TestIntentParameters : IntentParameters
    {
        public const string TestKey = "TestKey";

        public TestIntentParameters(ILambdaLogger logger, Session inputSession) : base(logger, inputSession)
        {
        }

        protected override void AddParameters(ILambdaLogger logger, Session inputSession)
        {
            ParameterQueues[TestKey] = new PersistentQueue<string>(logger, inputSession, TestKey);
        }

        public PersistentQueue<string> TestQueue => GetParameter<PersistentQueue<string>>(TestKey);
    }
}
