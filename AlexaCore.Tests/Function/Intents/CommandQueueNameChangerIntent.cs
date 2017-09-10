using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;

namespace AlexaCore.Tests.Function.Intents
{
    class CommandQueueNameChangerIntent : AlexaIntent
    {
        public CommandQueueNameChangerIntent(IntentParameters parameters) : base(parameters)
        {
        }

        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            return Tell("Name changed");
        }

        public override CommandDefinition CommandDefinition()
        {
            return new CommandDefinition {IntentName = "FakeCommandDefinitionName"};
        }
    }
}
