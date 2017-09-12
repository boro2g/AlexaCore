using System.Collections.Generic;
using Alexa.NET.Request;
using NUnit.Framework;

namespace AlexaCore.Tests.Extensions
{
    [TestFixture]
    class PersistentQueueExtensionTests
    {
        [Test]
        public void AddingString_UpdatesInputQueue()
        {
            var inputQueue = new PersistentQueue<InputItem>(null, new Session { Attributes = new Dictionary<string, object>()}, "testQueue");

            inputQueue.Enqueue("test");

            Assert.That(inputQueue.LastItem().Value, Is.EqualTo("test"));
        }

        [Test]
        public void ApplicationParameter_GetsAddedAndUpdated()
        {
            var applicationParameters = new PersistentQueue<ApplicationParameter>(null, new Session { Attributes = new Dictionary<string, object>() }, "testQueue");

            Assert.That(applicationParameters.GetValue("name"), Is.EqualTo(""));

            applicationParameters.AddOrUpdateValue("name", "value");

            Assert.That(applicationParameters.GetValue("name"), Is.EqualTo("value"));

            applicationParameters.AddOrUpdateValue("name", "value updated");

            Assert.That(applicationParameters.GetValue("name"), Is.EqualTo("value updated"));
        }

        [Test]
        public void CommandQueue_QueueEndsWith_FindsCorrectItems()
        {
            var commands = BuildCommandQueue();

            Assert.That(commands.QueueEndsWith(new[] {"4"}), Is.False);

            Assert.That(commands.QueueEndsWith(new[] {"4", "5"}), Is.True);
        }

        [Test]
        public void CommandQueue_QueueEndsWithIntentsOfType_FindsCorrectItems()
        {
            var commands = BuildCommandQueue();

            Assert.That(commands.QueueEndsWithIntentsOfType("3", new[] {"4"}), Is.False);

            Assert.That(commands.QueueEndsWithIntentsOfType("3", new[] { "4" ,"5"}), Is.True);
        }

        private static PersistentQueue<CommandDefinition> BuildCommandQueue()
        {
            var commands = new PersistentQueue<CommandDefinition>(null,
                new Session {Attributes = new Dictionary<string, object>()}, "testQueue");

            commands.Enqueue(new CommandDefinition {IntentName = "1"});
            commands.Enqueue(new CommandDefinition {IntentName = "2"});
            commands.Enqueue(new CommandDefinition {IntentName = "3"});
            commands.Enqueue(new CommandDefinition {IntentName = "4"});
            commands.Enqueue(new CommandDefinition {IntentName = "5"});

            return commands;
        }
    }
}
