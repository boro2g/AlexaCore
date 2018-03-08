using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using AlexaCore.Intents;
using AlexaCore.Tests.Function.Intents;

namespace AlexaCore.Tests.Function
{
    class TestFunctionIntentFactory : IntentFactory
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

        public override bool IncludeDefaultDebugIntent()
        {
            return true;
        }
    }
}