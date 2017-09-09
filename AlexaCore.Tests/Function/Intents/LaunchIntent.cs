using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;

namespace AlexaCore.Tests.Function.Intents
{
    class LaunchIntent : AlexaIntent
    {
        public LaunchIntent(IntentParameters parameters) : base(parameters)
        {
        }

        public override string IntentName => "LaunchIntent";

        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            Parameters.ApplicationParameters.Update(new ApplicationParameter("name", "value"), a => a.Name == "name",
                true);

            Parameters.InputQueue.Enqueue("string");

            Parameters.InputQueue.Enqueue(new InputItem("stringWithTags", new[] {"tag1", "tag2"}));

            return Tell("LaunchIntent");
        }
    }
}
