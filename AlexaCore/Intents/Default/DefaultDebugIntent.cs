using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Extensions;
using Newtonsoft.Json;

namespace AlexaCore.Intents.Default
{
    class DefaultDebugIntent : AlexaIntent
    {
        public DefaultDebugIntent(IntentParameters parameters) : base(parameters)
        {
        }

        public override string IntentName => AlexaContext.IntentNames.DefaultDebugIntent;

        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            return Tell($"Intents: {AlexaContext.IntentFactory.RegisteredIntents().JoinStringList()}." +
                        $"ApplicationParameters: {JsonConvert.SerializeObject(AlexaContext.Parameters.ApplicationParameters)}." +
                        $"CommandQueue: {JsonConvert.SerializeObject(AlexaContext.Parameters.CommandQueue)}." +
                        $"InputQueue: {JsonConvert.SerializeObject(AlexaContext.Parameters.InputQueue)}.");
        }
    }
}
