using System.Collections.Generic;
using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;

namespace AlexaCore.Tests.Function.Intents
{
    class QuestionNeedingResponseIntent : IntentWithResponse
    {
        protected override IEnumerable<string> RequiredSlots => new[] { "TestSlot" };

        public QuestionNeedingResponseIntent(IntentParameters parameters) : base(parameters)
        {
        }

        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            var slotValue = slots[RequiredSlots.ElementAt(0)].Value;

            return Tell($"Slot value: {slotValue}. Are you sure?");
        }

        public override IEnumerable<PossibleResponse> PossibleResponses()
        {
            //yes and no have default helpers
            yield return PossibleResponse.NoResponse(NoResponse);

            //or build your own response
            yield return new PossibleResponse("YesIntent", "yes", YesResponse, "");
        }

        private SkillResponse NoResponse(string parameterValue)
        {
            return Tell($"No response");
        }

        private SkillResponse YesResponse(string parameterValue)
        {
            return Tell($"Yes response");
        }
    }
}
