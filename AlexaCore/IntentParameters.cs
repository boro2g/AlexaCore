using System.Collections.Generic;
using Alexa.NET.Request;
using Amazon.Lambda.Core;

namespace AlexaCore
{
    public class IntentParameters
    {
	    public PersistentQueue<InputItem> InputQueue;

	    public PersistentQueue<CommandDefinition> CommandQueue;

	    public PersistentQueue<ApplicationParameter> ApplicationParameters;

	    public ILambdaLogger Logger { get; }

	    public string UserAccessToken => InputSession?.User?.AccessToken;

		public string UserId => InputSession?.User?.UserId;

		private Session InputSession { get; }

	    public IntentParameters(ILambdaLogger logger, Session inputSession)
	    {
		    Logger = logger;

		    InputSession = inputSession;

		    if (InputSession.Attributes == null)
		    {
			    InputSession.Attributes = new Dictionary<string, object>();
		    }

		    InputQueue = new PersistentQueue<InputItem>(logger, InputSession, "Inputs");

		    CommandQueue = new PersistentQueue<CommandDefinition>(logger, InputSession, "Commands");

			ApplicationParameters = new PersistentQueue<ApplicationParameter>(logger, InputSession, "Parameters");

		    if (inputSession.New)
		    {
			    InputQueue.Reset();
			    CommandQueue.Reset();
				ApplicationParameters.Reset();
		    }
	    }

	    public Dictionary<string, object> SessionAttributes()
	    {
		    return InputSession.Attributes;
	    }
    }
}
