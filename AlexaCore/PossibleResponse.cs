using System;
using AlexaCore.Intents;

namespace AlexaCore
{
	public class PossibleResponse
	{
		public PossibleResponse(string intentName, string intentAsText, Func<IntentResponse> action)
		{
			IntentName = intentName;
			IntentAsText = intentAsText;
			Action = action;
		}

		public string IntentName { get; set; }

		public string IntentAsText { get; set; }

		public Func<IntentResponse> Action { get; set; }

		public static PossibleResponse YesResponse(Func<IntentResponse> action)
		{
			return new PossibleResponse(new IntentNames().YesIntent, "yes", action);
		}

		public static PossibleResponse NoResponse(Func<IntentResponse> action)
		{
			return new PossibleResponse(new IntentNames().NoIntent, "no", action);
		}
	}
}