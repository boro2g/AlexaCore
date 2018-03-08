using System.Collections.Generic;
using System.Linq;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace AlexaCore.Intents
{
	public abstract class IntentAsResponse : AlexaIntent
	{
		protected SkillResponse FindPreviousQuestionResponse(Dictionary<string, Slot> slots)
		{
			var lastCommand = SelectLastCommand();

			Parameters.Logger.LogLine($"lastCommand: {lastCommand?.IntentName}");

			if (lastCommand != null && lastCommand.ExpectsResponse)
			{
                if (AlexaContext.IntentFactory.Intents()[lastCommand.IntentName] is IntentWithResponse intent)
                {
                    Parameters.Logger.LogLine($"IntentAsResponse: {intent.IntentName}");

                    var matchedResponse = intent.PossibleResponses().FirstOrDefault(a => a.IntentName == IntentName);

                    if (matchedResponse != null)
                    {
                        return matchedResponse.Action();
                    }

                    return Tell(intent.PossibleResponsesAsText());
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