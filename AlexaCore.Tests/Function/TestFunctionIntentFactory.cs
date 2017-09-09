using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlexaCore.Intents;
using AlexaCore.Tests.Function.Intents;

namespace AlexaCore.Tests.Function
{
    class TestFunctionIntentFactory : IntentFactory
    {
        protected override List<AlexaIntent> ApplicationIntents(IntentParameters intentParameters)
        {
            return IntentFinder.FindIntents(new[] { typeof(LaunchIntent).GetTypeInfo().Assembly },
                intentParameters).ToList();
        }

        public override AlexaIntent LaunchIntent(IntentParameters intentParameters)
        {
            return new LaunchIntent(intentParameters);
        }
    }
}