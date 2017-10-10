using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;

namespace AlexaCore.Tests.Function.Intents
{
    class TestParameterIntent : AlexaIntent
    {
        public TestParameterIntent(IntentParameters parameters) : base(parameters)
        {
        }

        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            if (Parameters is TestIntentParameters testIntentParameters)
            {
                testIntentParameters.TestQueue.Enqueue("first");

                testIntentParameters.TestQueue.Enqueue("second");
            }

            return Tell("");
        }
    }
}
