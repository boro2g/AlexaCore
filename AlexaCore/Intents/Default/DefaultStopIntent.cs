using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace AlexaCore.Intents.Default
{
	public class DefaultStopIntent : AlexaIntent
	{
	    public IntentNames IntentNames { get; set; }

        public override string IntentName => IntentNames.StopIntent;

		public override bool ShouldEndSession => true;

		protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
		{
		    return Tell("Goodbye");
		}
	}
}