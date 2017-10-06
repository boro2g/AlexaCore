namespace AlexaCore.Intents
{
    public abstract class AlexaHelpIntent : AlexaIntent
    {
        protected AlexaHelpIntent(IntentParameters parameters) : base(parameters)
        {
        }

        public override string IntentName => AlexaContext.IntentNames.HelpIntent;
    }
}
