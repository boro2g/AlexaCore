using System.Collections.Generic;
using Alexa.NET.Request;
using Alexa.NET.Response;

namespace AlexaCore.Intents.Default
{
    public class DefaultCancelIntent : AlexaIntent
	{
	    public IntentNames IntentNames { get; set; }

        public override string IntentName => IntentNames.CancelIntent;

		public override bool ShouldEndSession => true;

		protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
		{
		    return Tell("Goodbye");
		}
	}
}