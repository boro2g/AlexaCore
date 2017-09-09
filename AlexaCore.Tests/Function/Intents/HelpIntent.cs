using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;

namespace AlexaCore.Tests.Function.Intents
{
    class HelpIntent : AlexaIntent
    {
        public HelpIntent(IntentParameters parameters) : base(parameters)
        {
        }

        public override string IntentName => new IntentNames().HelpIntent;

        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            return Tell("HelpIntent");
        }
    }
}