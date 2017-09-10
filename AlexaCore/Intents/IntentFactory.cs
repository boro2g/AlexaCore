using System.Collections.Generic;
using System.Linq;
using AlexaCore.Intents.Default;

namespace AlexaCore.Intents
{
    public abstract class IntentFactory
    {
	    private static Dictionary<string, AlexaIntent> _intents;

        protected IntentFactory()
        {
            _intents = null;
        }

	    public Dictionary<string, AlexaIntent> Intents(IntentParameters intentParameters)
	    {
		    if (_intents == null)
		    {
			    var intents = ApplicationIntents(intentParameters);

			    var dictionary = new Dictionary<string, AlexaIntent>();

			    foreach (var intent in intents)
			    {
				    dictionary[intent.IntentName] = intent;
			    }

			    if (!dictionary.ContainsKey(AlexaContext.IntentNames.CancelIntent))
			    {
				    dictionary[AlexaContext.IntentNames.CancelIntent] = CancelIntent(intentParameters);
			    }

			    if (!dictionary.ContainsKey(AlexaContext.IntentNames.StopIntent))
			    {
				    dictionary[AlexaContext.IntentNames.StopIntent] = StopIntent(intentParameters);
			    }

		        if (IncludeDefaultDebugIntent() && !dictionary.ContainsKey(AlexaContext.IntentNames.DefaultDebugIntent))
		        {
		            dictionary[AlexaContext.IntentNames.DefaultDebugIntent] = DefaultDebugIntent(intentParameters);
                }

				_intents = dictionary;
		    }

		    return _intents;
	    }

	    protected abstract List<AlexaIntent> ApplicationIntents(IntentParameters intentParameters);

	    public abstract AlexaIntent LaunchIntent(IntentParameters intentParameters);

	    public AlexaIntent GetIntent(string intentName)
	    {
		    if (_intents != null && _intents.ContainsKey(intentName))
		    {
			    return _intents[intentName];
		    }

		    return null;
	    }

	    public virtual AlexaIntent HelpIntent()
	    {
		    return _intents[AlexaContext.IntentNames.HelpIntent];
	    }

        public virtual bool IncludeDefaultDebugIntent()
        {
            return false;
        }

	    public IEnumerable<string> RegisteredIntents()
	    {
		    return _intents?.Keys.Select(a => a) ?? new string[0];
	    }

	    protected virtual AlexaIntent CancelIntent(IntentParameters intentParameters)
	    {
		    return new DefaultCancelIntent(intentParameters);
	    }

	    protected virtual AlexaIntent StopIntent(IntentParameters intentParameters)
	    {
		    return new DefaultStopIntent(intentParameters);
	    }

        protected virtual AlexaIntent DefaultDebugIntent(IntentParameters intentParameters)
        {
            return new DefaultDebugIntent(intentParameters);
        }
    }
}
