using System;
using Alexa.NET.Response;

namespace AlexaCore
{
	public class PossibleResponse
	{
		public PossibleResponse(string intentName, string intentAsText, Func<string, SkillResponse> action, string parameterValue)
		{
			IntentName = intentName;
			IntentAsText = intentAsText;
			Action = action;
			ParameterValue = parameterValue;
		}

		public string IntentName { get; set; }

		public string IntentAsText { get; set; }

		public Func<string, SkillResponse> Action { get; set; }

		public string ParameterValue { get; }

		public static PossibleResponse YesResponse(Func<string, SkillResponse> action, string parameterValue = "")
		{
			return new PossibleResponse(AlexaContext.IntentNames.YesIntent, "yes", action, parameterValue);
		}

		public static PossibleResponse NoResponse(Func<string, SkillResponse> action, string parameterValue = "")
		{
			return new PossibleResponse(AlexaContext.IntentNames.NoIntent, "no", action, parameterValue);
		}
	}
}