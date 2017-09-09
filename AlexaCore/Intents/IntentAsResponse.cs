using System.Collections.Generic;
using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace AlexaCore.Intents
{
	public abstract class IntentAsResponse : AlexaIntent
	{
		protected IntentAsResponse(IntentParameters parameters) : base(parameters)
		{
		}

		protected SkillResponse FindPreviousQuestionResponse(Dictionary<string, Slot> slots)
		{
			var lastCommand = SelectLastCommand();

			Parameters.Logger.LogLine($"lastCommand: {lastCommand?.IntentName}");

			if (lastCommand != null && lastCommand.ExpectsResponse)
			{
				var intent = AlexaContext.IntentFactory.Intents(Parameters)[lastCommand.IntentName] as IntentWithResponse;
				
				if (intent != null)
				{
					Parameters.Logger.LogLine($"IntentAsResponse: {intent.IntentName}");

					var matchedResponse = intent.PossibleResponses().FirstOrDefault(a => a.IntentName == IntentName);

					if (matchedResponse != null)
					{
						return matchedResponse.Action(matchedResponse.ParameterValue);
					}

					return BuildResponse(new PlainTextOutputSpeech { Text = intent.PossibleResponsesAsText() });
				}
			}

			return AlexaContext.IntentFactory.HelpIntent().GetResponse(slots);
		}

		protected virtual CommandDefinition SelectLastCommand()
		{
			return Parameters.CommandQueue.LastItem();
		}
	}
}