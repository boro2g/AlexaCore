using System;
using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Extensions;
using Newtonsoft.Json;

namespace AlexaCore.Intents.Default
{
    public class DefaultDebugIntent : AlexaIntent
    {
        public override string IntentName => AlexaContext.IntentNames.DefaultDebugIntent;

        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            return Tell($"Intents: {AlexaContext.IntentFactory.RegisteredIntents().JoinStringList()}.{Environment.NewLine}" +
                        $"ApplicationParameters: {JsonConvert.SerializeObject(AlexaContext.Parameters.ApplicationParameters)}.{Environment.NewLine}" +
                        $"CommandQueue: {JsonConvert.SerializeObject(AlexaContext.Parameters.CommandQueue)}.{Environment.NewLine}" +
                        $"InputQueue: {JsonConvert.SerializeObject(AlexaContext.Parameters.InputQueue)}.{Environment.NewLine}");
        }
    }
}
