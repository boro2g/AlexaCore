using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;

namespace AlexaCore.LambdaFunction.Intents
{
    class LaunchIntent : AlexaIntent
    {
        public LaunchIntent(IntentParameters parameters) : base(parameters)
        {
        }

        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            return Tell("Launch");
        }
    }
}
