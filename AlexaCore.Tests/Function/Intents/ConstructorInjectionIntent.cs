using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;

namespace AlexaCore.Tests.Function.Intents
{
    class ConstructorInjectionIntent : AlexaIntent
    {
        private readonly ITestDependency _testDependency;

        public ConstructorInjectionIntent(ITestDependency testDependency)
        {
            _testDependency = testDependency;
        }

        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            return Tell(_testDependency.GetData());
        }
    }
}
