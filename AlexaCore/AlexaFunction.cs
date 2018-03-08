using Alexa.NET.Request;
using Alexa.NET.Response;
using AlexaCore.Intents;
using Amazon.Lambda.Core;
using Autofac;
using Newtonsoft.Json;

namespace AlexaCore
{
    public abstract class AlexaFunction
    {
	    private IntentFactory _intentFactory;

        private IContainer _container;
	   
	    protected abstract IntentFactory IntentFactory();

	    protected virtual IntentNames IntentNames()
	    {
		    return null;
		}

        private AlexaContext AlexaContext { get; set; }

        protected virtual bool EnableOperationTimerLogging => true;

		public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
		{
            IntentParameters parameters;

            using (new OperationTimer(context.Logger.LogLine, "Init", EnableOperationTimerLogging))
		    {
		        parameters = SetupFunction(input, context);

		        context.Logger.LogLine("Input: " + JsonConvert.SerializeObject(input));

		        var initResponse = FunctionInit(AlexaContext, parameters);

		        if (initResponse != null)
		        {
		            return initResponse;
		        }
		    }

		    SkillResponse innerResponse;

		    using (new OperationTimer(context.Logger.LogLine, "Run function", EnableOperationTimerLogging))
		    {
		        innerResponse = new AlexaFunctionRunner(_intentFactory, NoIntentMatchedText).Run(input, parameters);

		        innerResponse.SessionAttributes = parameters.SessionAttributes();

		        parameters.Logger.LogLine("Output: " + JsonConvert.SerializeObject(innerResponse));
		    }

		    using (new OperationTimer(context.Logger.LogLine, "Function complete", EnableOperationTimerLogging))
		    {
		        FunctionComplete(innerResponse);
		    }

		    return innerResponse;
		}

        private IntentParameters SetupFunction(SkillRequest input, ILambdaContext context)
        {
            _intentFactory = IntentFactory();

            _container = BuildContainer(_intentFactory);

            var parameters = BuildParameters(context.Logger, input.Session);

            AlexaContext = new AlexaContext(_intentFactory, IntentNames(), parameters);

            _intentFactory.BuildIntents(parameters, _container);

            return parameters;
        }

        private IContainer BuildContainer(IntentFactory intentFactory)
        {
            var builder = new ContainerBuilder();

            intentFactory.RegisterIntents(builder);

            RegisterDependencies(builder);

            return builder.Build();
        }

        protected virtual void RegisterDependencies(ContainerBuilder builder)
        {
            
        }

        protected virtual IntentParameters BuildParameters(ILambdaLogger logger, Session session)
        {
            return new IntentParameters(logger, session);
        }
       
        protected virtual SkillResponse FunctionInit(AlexaContext alexaContext, IntentParameters parameters)
        {
            return null;
        }

		protected virtual void FunctionComplete(SkillResponse innerResponse)
		{
		}

		public virtual string NoIntentMatchedText => "No intent matched - intent was {0}";
	}
}
