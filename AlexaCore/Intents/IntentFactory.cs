using System;
using System.Collections.Generic;
using System.Linq;
using AlexaCore.Intents.Default;
using Autofac;

namespace AlexaCore.Intents
{
    public abstract class IntentFactory
    {
        private const string LaunchIntentKey = "LaunchIntent";
        private static Dictionary<string, AlexaIntent> _intents;
        private AlexaHelpIntent _helpIntent;
        private AlexaIntent _launchIntent;

        protected IntentFactory()
        {
            _intents = null;
        }

        internal void RegisterIntents(ContainerBuilder builder)
        {
            foreach (var intentType in ApplicationIntentTypes())
            {
                builder.RegisterType(intentType).As<AlexaIntent>().PropertiesAutowired();
            }

            builder.RegisterType<DefaultCancelIntent>().PropertiesAutowired();

            builder.RegisterType<DefaultDebugIntent>().PropertiesAutowired();

            builder.RegisterType<DefaultStopIntent>().PropertiesAutowired();

            builder.RegisterType(LaunchIntentType())
                .Named<AlexaIntent>(LaunchIntentKey).PropertiesAutowired();

            builder.RegisterType(HelpIntentType()).As<AlexaHelpIntent>().PropertiesAutowired();
        }

        internal void BuildIntents(IntentParameters intentParameters, IContainer container)
        {
            if (_intents == null)
            {
                var registeredIntents = container.Resolve<IEnumerable<AlexaIntent>>();

                var intentNames = container.Resolve<IntentNames>();

                var dictionary = new Dictionary<string, AlexaIntent>();

                foreach (var intent in registeredIntents)
                {
                    dictionary[intent.IntentName] = intent;
                }

                if (!dictionary.ContainsKey(intentNames.CancelIntent))
                {
                    dictionary[intentNames.CancelIntent] = container.Resolve<DefaultCancelIntent>();
                }

                if (!dictionary.ContainsKey(intentNames.StopIntent))
                {
                    dictionary[intentNames.StopIntent] = container.Resolve<DefaultStopIntent>();
                }

                if (IncludeDefaultDebugIntent() && !dictionary.ContainsKey(intentNames.DefaultDebugIntent))
                {
                    dictionary[intentNames.DefaultDebugIntent] = container.Resolve<DefaultDebugIntent>();
                }

                _helpIntent = container.Resolve<AlexaHelpIntent>();

                dictionary[_helpIntent.IntentName] = _helpIntent;

                _launchIntent = container.ResolveNamed<AlexaIntent>(LaunchIntentKey);

                dictionary[_launchIntent.IntentName] = _launchIntent;

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
            return _launchIntent;
        }

        public virtual AlexaHelpIntent HelpIntent()
        {
            return _helpIntent;
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
