# AlexaCore 
AlexaCore is a set of helpers to assist with building C# Alexa functions. 

It's underpinned by the library [https://github.com/timheuer/alexa-skills-dotnet](https://github.com/timheuer/alexa-skills-dotnet) which provides the serialization and mapping from the parameters sent into your Lambda function.

The core of the functionality revolves around loading *intents* - an intent key is provided by Amazon in the request payload based off your configuration set in the developer portal.

# Some basic examples:
The tests project is a good start point for examples. 

To get up and running you need to:

1. Inherit your function from: **AlexaFunction** and implement an **IntentFactory**
```csharp
class TestFunction : AlexaFunction
{
    protected override IntentFactory IntentFactory()
    {
        return new TestFunctionIntentFactory();
    }
}
```

2. Setup some intents, the most simple example would inherit from **AlexaIntent**

```csharp
class DemoIntent : AlexaIntent
{
    public DemoIntent(IntentParameters parameters) : base(parameters)
    {
    }

    public override string IntentName => "DemoIntent";

    protected override SkillResponse GetResponseInternal(Dictionary<string, Slot> slots)
    {
        return Tell("Hi, I'm the demo intent");
    }
}
```

3. Within your implementation of the **IntentFactory** you can either manually register all your intents or use reflection to find them. You also need to specify your **LaunchIntent** (this will get returned when the skill starts):
```csharp
class TestFunctionIntentFactory : IntentFactory
{
    protected override List<AlexaIntent> ApplicationIntents(IntentParameters intentParameters)
    {
        //either register explicitly 
        //return new List<AlexaIntent> {new HelpIntent(intentParameters), new LaunchIntent(intentParameters)};

        //or use reflection to find all your intents based of a set of source assemblies
        return IntentFinder.FindIntents(new[] { typeof(LaunchIntent).GetTypeInfo().Assembly },
            intentParameters).ToList();
    }

    public override AlexaIntent LaunchIntent(IntentParameters intentParameters)
    {
        return new LaunchIntent(intentParameters);
    }
}
```
