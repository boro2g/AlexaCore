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
        public IntentNames IntentNames { get; set; }

        private readonly IntentFactory _intentFactory;

        public override string IntentName => IntentNames.DefaultDebugIntent;

        public DefaultDebugIntent(IntentFactory intentFactory)
        {
            _intentFactory = intentFactory;
        }

        protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
        {
            return Tell($"Intents: {_intentFactory.RegisteredIntents().JoinStringList()}.{Environment.NewLine}" +
                        $"ApplicationParameters: {JsonConvert.SerializeObject(Parameters.ApplicationParameters)}.{Environment.NewLine}" +
                        $"CommandQueue: {JsonConvert.SerializeObject(Parameters.CommandQueue)}.{Environment.NewLine}" +
                        $"InputQueue: {JsonConvert.SerializeObject(Parameters.InputQueue)}.{Environment.NewLine}");
        }
    }
}
