using Alexa.NET.Response;

namespace AlexaCore.Intents
{
    public class IntentResponse
    {
        public SkillResponse SkillResponse { get; }
        public bool ShouldEndSession { get; }

        public IntentResponse(SkillResponse skillResponse, bool shouldEndSession)
        {
            SkillResponse = skillResponse;
            ShouldEndSession = shouldEndSession;
        }
    }
}