using System.Collections.Generic;
using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;

namespace AlexaCore.Tests.Function.Intents
{
    class SlotIntent : AlexaIntent
    {
        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            return Tell($"Slot value: {slots[RequiredSlots.ElementAt(0)].Value}");
        }

        protected override IEnumerable<string> RequiredSlots => new[] {"TestSlot"};
    }
}