using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlexaCore.Intents;
using AlexaCore.LambdaFunction.Intents;

namespace AlexaCore.LambdaFunction
{
    public class ExampleIntentFactory : IntentFactory
    {
        protected override List<AlexaIntent> ApplicationIntents(IntentParameters intentParameters)
        {
            return IntentFinder.FindIntents(new[] { typeof(LaunchIntent).GetTypeInfo().Assembly },
                intentParameters).ToList();
        }

        public override AlexaIntent LaunchIntent()
        {
            return new LaunchIntent();
        }

        public override AlexaHelpIntent HelpIntent()
        {
            return new HelpIntent();
        }
    }
}