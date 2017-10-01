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
            //either register explicitly 
            //return new List<AlexaIntent> {new HelpIntent(intentParameters), new LaunchIntent(intentParameters)};

            //or use reflection to find all your intents based of a set of source assemblies
            return IntentFinder.FindIntents(new[] { typeof(LaunchIntent).GetTypeInfo().Assembly },
                intentParameters).ToList();
        }

        public override AlexaIntent LaunchIntent(IntentParameters intentParameters)
        {
            return new LaunchIntent(intentParameters);
        }

        public override AlexaIntent HelpIntent(IntentParameters intentParameters)
        {
            return new HelpIntent(intentParameters);
        }

        public override bool IncludeDefaultDebugIntent()
        {
            return true;
        }
    }
}