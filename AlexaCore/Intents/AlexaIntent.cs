using System;
using System.Collections.Generic;
using System.Linq;
using Alexa.NET;
using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Extensions;

namespace AlexaCore.Intents
{
	public abstract class AlexaIntent
	{
		protected IntentParameters Parameters;

		public virtual string IntentName => GetType().Name;

	    protected AlexaIntent(IntentParameters parameters)
		{
			Parameters = parameters;
		}

		public SkillResponse GetResponse(Dictionary<string, Slot> slots)
		{
			Parameters.Logger.LogLine($"Intent called: {IntentName}");

			if (!ValidateSlots(slots))
			{
				return BuildResponse(new PlainTextOutputSpeech
				{
					Text = MissingSlotsText()
				});
			}
			
			return GetResponseInternal(slots);
		}

	    protected virtual SkillResponse Tell(string message)
	    {
	        return BuildResponse(new PlainTextOutputSpeech {Text = message});
	    }

		protected virtual SkillResponse BuildResponse(IOutputSpeech outputSpeech)
		{
			return ResponseBuilder.Tell(outputSpeech);
		}

		protected virtual string MissingSlotsText()
		{
			return $"Your request seems to be missing some information. Expected slots are: {RequiredSlots.JoinStringList()}.";
		}

		private bool ValidateSlots(Dictionary<string, Slot> slots)
		{
			List<string> messages = new List<string>();

			foreach (var requiredSlot in RequiredSlots)
			{
				if (!slots.ContainsKey(requiredSlot))
				{
					messages.Add($"{IntentName} requires slot {requiredSlot} - it doesn't exist");
				}
				else
				{
					var slot = slots[requiredSlot];

					if (String.IsNullOrWhiteSpace(slot.Value))
					{
						messages.Add($"{IntentName} requires slot {requiredSlot} - it is empty");
					}
				}
			}

			messages.ForEach(Parameters.Logger.LogLine);

			return !messages.Any();
		}

		protected virtual IEnumerable<string> RequiredSlots => new string[0];

		public virtual bool ShouldEndSession { get; set; } = false;

		protected abstract SkillResponse GetResponseInternal(Dictionary<string, Slot> slots);

		public virtual CommandDefinition CommandDefinition()
		{
			return new CommandDefinition {IntentName = IntentName, ExpectsResponse = this is IntentWithResponse};
		}
	}
}