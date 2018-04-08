using System.Collections.Generic;
using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;

namespace AlexaCore.Tests.Function.Intents
{
    class QuestionNeedingResponseIntent : IntentWithResponse
    {
        private readonly IntentFactory _intentFactory;

        protected override IEnumerable<string> RequiredSlots => new[] { "TestSlot" };

        public QuestionNeedingResponseIntent(IntentFactory intentFactory)
        {
            _intentFactory = intentFactory;
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
            yield return new PossibleResponse("YesIntent", "yes", YesResponse);

            //or pull the response from elsewhere
            yield return new PossibleResponse("ExternalResponseIntent", "external", ExternalResponse);
        }

        private IntentResponse ExternalResponse()
        {
            return new IntentResponse(_intentFactory.GetIntent("LaunchIntent").GetResponse(Slots), false);
        }

        private IntentResponse NoResponse()
        {
            return new IntentResponse(Tell($"No response"), false);
        }

        private IntentResponse YesResponse()
        {
            return new IntentResponse(Tell($"Yes response"), false);
        }
    }
}
