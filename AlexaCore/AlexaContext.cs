using AlexaCore.Intents;

namespace AlexaCore
{
    public class AlexaContext
    {
        private static LambdaContainer _container;

        public static IntentFactory IntentFactory { get; private set; }

        public static IntentParameters Parameters { get; private set; }

        public static IntentNames IntentNames { get; private set; }

        public static LambdaContainer Container => _container ?? (_container = new LambdaContainer(a => Parameters.Logger.Log(a)));

        public AlexaContext(IntentFactory intentFactory, IntentNames intentNames, IntentParameters parameters)
		{
			IntentFactory = intentFactory;

		    Parameters = parameters;

		    IntentNames = intentNames ?? new IntentNames();
        }
    }
}
