using AlexaCore.Intents;
using Amazon.Lambda.Core;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AlexaCore.LambdaFunction
{
    public class Function : AlexaFunction
    {
        protected override IntentFactory IntentFactory()
        {
            return new ExampleIntentFactory();
        }
    }
}
