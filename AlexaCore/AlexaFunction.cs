using System;
using System.Collections.Generic;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Request.Type;
using Alexa.NET.Response;
using AlexaCore.Intents;
using Amazon.Lambda.Core;
using Newtonsoft.Json;

namespace AlexaCore
{
    public abstract class AlexaFunction
    {
	    private IntentFactory _intentFactory;
	   
	    protected abstract IntentFactory IntentFactory();

	    protected virtual IntentNames IntentNames()
	    {
		    return null;
		}

		public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
		{
			_intentFactory = IntentFactory();

		    context.Logger.LogLine("Input: " + JsonConvert.SerializeObject(input));

            var parameters = new IntentParameters(context.Logger, input.Session);

            var alexaContext = new AlexaContext(_intentFactory, IntentNames(), parameters);

			FunctionInit(alexaContext, parameters);

			var innerResponse = Run(input, parameters);

			innerResponse.SessionAttributes = parameters.SessionAttributes();

			parameters.Logger.LogLine("Output: " + JsonConvert.SerializeObject(innerResponse));

			FunctionComplete(innerResponse);

			return innerResponse;
		}

		protected virtual void FunctionInit(AlexaContext alexaContext, IntentParameters parameters)
		{
		}

		protected virtual void FunctionComplete(SkillResponse innerResponse)
		{
		}

		public virtual string NoIntentMatchedText => "No intent matched - intent was {0}";

	    private SkillResponse Run(SkillRequest input, IntentParameters parameters)
	    {
		    AlexaIntent intentToRun = null;

		    Dictionary<string, Slot> slots = null;

		    string intentName = "";

		    if (input.GetRequestType() == typeof(LaunchRequest))
		    {
			    intentToRun = AlexaContext.IntentFactory.LaunchIntent(parameters);

			    slots = new Dictionary<string, Slot>();
		    }
		    else if (input.GetRequestType() == typeof(IntentRequest))
		    {
			    var intentRequest = (IntentRequest)input.Request;

			    var intents = AlexaContext.IntentFactory.Intents(parameters);

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
			    return ResponseBuilder.Tell(new PlainTextOutputSpeech { Text = String.Format(NoIntentMatchedText, intentName) });
		    }

		    var skillResponse = intentToRun.GetResponse(slots);

		    skillResponse.Response.ShouldEndSession = intentToRun.ShouldEndSession;

		    parameters.CommandQueue.Enqueue(intentToRun.CommandDefinition());

		    return skillResponse;
	    }
	}
}
