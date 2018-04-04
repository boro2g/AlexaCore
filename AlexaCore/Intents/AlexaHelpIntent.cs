namespace AlexaCore.Intents
{
    public abstract class AlexaHelpIntent : AlexaIntent
    {
        public IntentNames IntentNames { get; set; }

        public override string IntentName => IntentNames.HelpIntent;
    }
}
