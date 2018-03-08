using AlexaCore.Intents;
using Autofac;

namespace AlexaCore
{
    public class AlexaContext
    {
        private static LambdaContainer _container;

        public static IntentFactory IntentFactory { get; private set; }

        public static IntentParameters Parameters { get; private set; }
        
        public static IntentNames IntentNames { get; private set; }

        public static LambdaContainer Container => _container ?? (_container = new LambdaContainer(a => Parameters.Logger.Log(a)));

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
