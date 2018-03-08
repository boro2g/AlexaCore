using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlexaCore.Intents;
using AlexaCore.LambdaFunction.Intents;

namespace AlexaCore.LambdaFunction
{
    public class ExampleIntentFactory : IntentFactory
    {
        protected override List<Type> ApplicationIntentTypes()
        {
            return IntentFinder.FindIntentTypes(new[] { typeof(LaunchIntent).GetTypeInfo().Assembly }).ToList();
        }

        public override Type LaunchIntentType()
        {
            return typeof(LaunchIntent);
        }

        public override Type HelpIntentType()
        {
            return typeof(HelpIntent);
        }
    }
}