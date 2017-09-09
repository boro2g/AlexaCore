using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace AlexaCore.Intents.Default
{
	public class DefaultStopIntent : AlexaIntent
	{
		public DefaultStopIntent(IntentParameters intentParameters) : base(intentParameters)
		{

		}

		public override string IntentName => AlexaContext.IntentNames.HelpIntent;

		public override bool ShouldEndSession => true;

		protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
		{
			return BuildResponse(new PlainTextOutputSpeech
			{
				Text = "Goodbye"
			});
		}
	}
}