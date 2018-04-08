using System.Collections.Generic;
using System.Linq;
using Alexa.NET.Request;

namespace AlexaCore.Intents
{
	public abstract class IntentAsResponse : AlexaIntent
	{
        public IntentFactory IntentFactory { get; set; }

		protected IntentResponse FindPreviousQuestionResponse(Dictionary<string, Slot> slots)
		{
			var lastCommand = SelectLastCommand();

			Parameters.Logger.LogLine($"lastCommand: {lastCommand?.IntentName}");

			if (lastCommand != null && lastCommand.ExpectsResponse)
			{
                if (IntentFactory.Intents()[lastCommand.IntentName] is IntentWithResponse intent)
                {
                    Parameters.Logger.LogLine($"IntentAsResponse: {intent.IntentName}");

                    var matchedResponse = intent.PossibleResponses().FirstOrDefault(a => a.IntentName == IntentName);

                    if (matchedResponse != null)
                    {
                        return matchedResponse.Action();
                    }

                    return new IntentResponse(Tell(intent.PossibleResponsesAsText()), false);
                }
            }

			return new IntentResponse(IntentFactory.HelpIntent().GetResponse(slots), false);
		}

		protected virtual CommandDefinition SelectLastCommand()
		{
			return Parameters.CommandQueue.LastItem();
		}
	}
}