using System;
using System.Collections.Generic;
using System.Linq;
using AlexaCore.Intents.Default;
using Autofac;

namespace AlexaCore.Intents
{
    public abstract class IntentFactory
    {
	    private static Dictionary<string, AlexaIntent> _intents;

        protected IntentFactory()
        {
            _intents = null;
        }

        public void RegisterIntents(ContainerBuilder builder)
        {
            foreach (var intentType in ApplicationIntentTypes())
            {
                builder.RegisterType(intentType).As<AlexaIntent>();
            }

            builder.RegisterType<DefaultCancelIntent>();

            builder.RegisterType<DefaultDebugIntent>();

            builder.RegisterType<DefaultStopIntent>();

            builder.RegisterType(LaunchIntentType())
                .Named<AlexaIntent>("LaunchIntent");

            builder.RegisterType(HelpIntentType()).As<AlexaHelpIntent>();
        }

        public void BuildIntents(IntentParameters intentParameters, IContainer container)
        {
            if (_intents == null)
            {
                var registeredIntents = container.Resolve<IEnumerable<AlexaIntent>>();

                var dictionary = new Dictionary<string, AlexaIntent>();

                foreach (var intent in registeredIntents)
                {
                    dictionary[intent.IntentName] = intent;
                }

                if (!dictionary.ContainsKey(AlexaContext.IntentNames.CancelIntent))
                {
                    dictionary[AlexaContext.IntentNames.CancelIntent] = container.Resolve<DefaultCancelIntent>();
                }

                if (!dictionary.ContainsKey(AlexaContext.IntentNames.StopIntent))
                {
                    dictionary[AlexaContext.IntentNames.StopIntent] = container.Resolve<DefaultStopIntent>();
                }

                if (IncludeDefaultDebugIntent() && !dictionary.ContainsKey(AlexaContext.IntentNames.DefaultDebugIntent))
                {
                    dictionary[AlexaContext.IntentNames.DefaultDebugIntent] = container.Resolve<DefaultDebugIntent>();
                }

                var helpIntent = container.Resolve<AlexaHelpIntent>();

                dictionary[helpIntent.IntentName] = helpIntent;

                var launchIntent = container.ResolveNamed<AlexaIntent>("LaunchIntent");

                dictionary[launchIntent.IntentName] = launchIntent;

                foreach (var key in dictionary.Keys)
                {
                    dictionary[key].SetParameters(intentParameters);
                }

                _intents = dictionary;
            }
        }

        public Dictionary<string, AlexaIntent> Intents()
	    {
		    return _intents;
	    }

        protected abstract List<Type> ApplicationIntentTypes();

        public virtual AlexaIntent LaunchIntent()
        {
            throw new NotImplementedException();
        }

        public virtual AlexaHelpIntent HelpIntent()
        {
            throw new NotImplementedException();
        }

        public abstract Type LaunchIntentType();

        public abstract Type HelpIntentType();

        public AlexaIntent GetIntent(string intentName)
	    {
		    if (_intents != null && _intents.ContainsKey(intentName))
		    {
			    return _intents[intentName];
		    }

		    return null;
	    }

        public virtual bool IncludeDefaultDebugIntent()
        {
            return false;
        }

	    public IEnumerable<string> RegisteredIntents()
	    {
		    return _intents?.Keys.Select(a => a) ?? new string[0];
	    }
    }
}
