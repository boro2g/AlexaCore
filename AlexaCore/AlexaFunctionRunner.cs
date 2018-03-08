using System;
using System.Collections.Generic;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using AlexaCore.Intents;

namespace AlexaCore
{
    internal class AlexaFunctionRunner
    {
        private readonly IntentFactory _intentFactory;
        private readonly string _noIntentMatchedText;

        public AlexaFunctionRunner(IntentFactory intentFactory, string noIntentMatchedText)
        {
            _intentFactory = intentFactory;
            _noIntentMatchedText = noIntentMatchedText;
        }

        public SkillResponse Run(SkillRequest input, IntentParameters parameters)
        {
            AlexaIntent intentToRun = null;

            Dictionary<string, Slot> slots = null;

            string intentName = "";

            if (input.GetRequestType() == typeof(LaunchRequest))
            {
                intentToRun = _intentFactory.LaunchIntent();

                slots = new Dictionary<string, Slot>();
            }
            else if (input.GetRequestType() == typeof(IntentRequest))
            {
                var intentRequest = (IntentRequest)input.Request;

                var intents = AlexaContext.IntentFactory.Intents();

                slots = intentRequest.Intent.Slots;

                intentName = intentRequest.Intent.Name;

                if (intents.ContainsKey(intentRequest.Intent.Name))
                {
                    intentToRun = intents[intentRequest.Intent.Name];
                }
                else
                {
                    intentToRun = _intentFactory.HelpIntent();
                }
            }

            if (intentToRun == null)
            {
                return ResponseBuilder.Tell(new PlainTextOutputSpeech { Text = String.Format(_noIntentMatchedText, intentName) });
            }

            var skillResponse = intentToRun.GetResponse(slots);

            skillResponse.Response.ShouldEndSession = intentToRun.ShouldEndSession;

            parameters.CommandQueue.Enqueue(intentToRun.CommandDefinition());

            return skillResponse;
        }
    }
}