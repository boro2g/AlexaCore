using System.Collections.Generic;
using Alexa.NET.Request;
using Amazon.Lambda.Core;

namespace AlexaCore
{
    public class IntentParameters
    {
        private const string InputsQueueKey = "Inputs";

        private const string CommandsQueueKey = "Commands";

        private const string ParametersQueueKey = "Parameters";

        public PersistentQueue<InputItem> InputQueue => GetParameter<PersistentQueue<InputItem>>(InputsQueueKey);

        public PersistentQueue<CommandDefinition> CommandQueue => GetParameter<PersistentQueue<CommandDefinition>>(CommandsQueueKey);

        public PersistentQueue<ApplicationParameter> ApplicationParameters =>
            GetParameter<PersistentQueue<ApplicationParameter>>(ParametersQueueKey);

        protected readonly Dictionary<string, object> ParameterQueues;

        //public ILambdaLogger Logger { get; }

	    public string UserAccessToken => InputSession?.User?.AccessToken;

		public string UserId => InputSession?.User?.UserId;

		private Session InputSession { get; }

	    public IntentParameters(ILambdaLogger logger, Session inputSession)
	    {
		    //Logger = logger;

		    InputSession = inputSession;

		    if (InputSession.Attributes == null)
		    {
			    InputSession.Attributes = new Dictionary<string, object>();
		    }

	        ParameterQueues = new Dictionary<string, object>
	        {
	            {InputsQueueKey, new PersistentQueue<InputItem>(logger, InputSession, InputsQueueKey)},
	            {CommandsQueueKey, new PersistentQueue<CommandDefinition>(logger, InputSession, CommandsQueueKey)},
	            {ParametersQueueKey, new PersistentQueue<ApplicationParameter>(logger, InputSession, ParametersQueueKey)}
	        };

	        AddParameters(logger, InputSession);
            
		    if (inputSession.New)
		    {
		        foreach (var key in ParameterQueues.Keys)
		        {
		            var resettableQueue = ParameterQueues[key] as IResettable;

		            resettableQueue?.Reset();
		        }
		    }
	    }

        protected virtual void AddParameters(ILambdaLogger logger, Session inputSession)
        {
            
        }

        public T GetParameter<T>(string key) where T : class
        {
            return ParameterQueues[key] as T;
        }

        public Dictionary<string, object> SessionAttributes()
	    {
		    return InputSession.Attributes;
	    }
    }
}
