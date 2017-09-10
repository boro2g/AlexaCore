using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;

namespace AlexaCore.Tests.Function.Intents
{
    class ParameterUpdaterIntent : AlexaIntent
    {
        public ParameterUpdaterIntent(IntentParameters parameters) : base(parameters)
        {
        }
        
        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            Parameters.ApplicationParameters.Update(new ApplicationParameter("name", "value updated"), a => a.Name == "name", true);

            return Tell("Parameter updated");
        }
    }
}
