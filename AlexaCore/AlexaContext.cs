using AlexaCore.Intents;
using Autofac;

namespace AlexaCore
{
    public class AlexaContext
    {
        public static IntentFactory IntentFactory { get; private set; }

        public static IntentParameters Parameters { get; private set; }
        
        public static IntentNames IntentNames { get; private set; }
        
        public static IContainer DiContainer { get; private set; }

        public AlexaContext(IntentFactory intentFactory, IntentNames intentNames, IntentParameters parameters,
            IContainer container)
		{
			IntentFactory = intentFactory;

		    Parameters = parameters;

            DiContainer = container;

            IntentNames = intentNames ?? new IntentNames();
        }
    }
}
