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

# Some timesavers:
You now no longer need to worry about handling any of the input and output data - all the intents are automatically handled behind the scenes. If using reflection, to add a new intent is as simple as writing the new Intent code - all the mapping by name happens in this library.

# Session:
Alexa Lambda functions can handle session data allowing you to persist data between requests. To help with this process, the **Parameters** object available to each intent has some data stores available. How you use these is down to personal preference, some example usages would be:

1. **Parameters.CommandQueue**
These get automatically populated with each intent name as intents get run. If you wanted to check historical events you can iterate through the set of entries. This is useful if you chain intents e.g. one intent asks a question and you then expect a set of responses.

2. **Parameters.InputQueue**
Imagine you hold a list of names in your function and have 2 intents, one to add new names and one to remove names. If you store each time someone adds a name in the InputQueue e.g. `Parameters.InputQueue.Enqueue(new InputItem("NewPerson", new[] {"Add"}));`. 
When you come to return all the names you can query the queue's entries based off the **Tag: Add** e.g. `Parameters.InputQueue.Entries().Where(a => a.Tags.Contains("Add")).Select(a => a.Value)`

3. **Parameters.ApplicationParameters**
If you need to persist dynamic parameters in the function you can store data in the ApplicationParameters. An example would be when the function starts you lookup a key in a database and then persist the id throughout the rest of the function calls.
To store the data you could call: `Parameters.ApplicationParameters.Enqueue(new ApplicationParameter("Key", "Value"))` and then to retrieve the data you could call: `Parameters.ApplicationParameters.Find(a => a.Name == "Key")?.Value`

Updating parameters or values is possible - each queue has an `Update` method available.

# Chaining intents:
A relatively common scenario is to chain intents. 

# Default intents:
Todo

# Extensions:
Todo

# Unit Testing your function:
Todo
