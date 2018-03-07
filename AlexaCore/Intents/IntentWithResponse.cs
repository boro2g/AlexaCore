using System;
using System.Collections.Generic;
using System.Linq;

namespace AlexaCore.Intents
{
    public abstract class IntentWithResponse : AlexaIntent
    {
	    public abstract IEnumerable<PossibleResponse> PossibleResponses();

	    public string PossibleResponsesAsText()
	    {
		    if (PossibleResponses().Any())
		    {
			    return UnexpectedResponse();
		    }

		    return "";
	    }

	    public virtual string UnexpectedResponse()
	    {
			return $"I wasn't expecting that answer - how about {String.Join(" or ", PossibleResponses().Select(a => a.IntentAsText))}";
		}
    }
}
