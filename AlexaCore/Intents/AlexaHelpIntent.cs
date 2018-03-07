namespace AlexaCore.Intents
{
    public abstract class AlexaHelpIntent : AlexaIntent
    {
        public override string IntentName => AlexaContext.IntentNames.HelpIntent;
    }
}
