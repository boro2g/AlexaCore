using System.Linq;

namespace AlexaCore
{
    public static class PersistentQueueExtensions
    {
		public static void Enqueue(this PersistentQueue<InputItem> queue, string value)
		{
			queue.Enqueue(new InputItem(value));
		}

        public static void AddOrUpdateValue(this PersistentQueue<ApplicationParameter> queue, string name, string value)
        {
            queue.Update(new ApplicationParameter(name, value), a => a.Name == name, true);
        }

        public static string GetValue(this PersistentQueue<ApplicationParameter> queue, string name)
        {
            var parameter =
                queue.Find(a => a.Name == name);

            return parameter == null ? "" : parameter.Value;
        }

        public static bool QueueEndsWith(this PersistentQueue<CommandDefinition> queue, string[] requiredIntents)
        {
            requiredIntents = requiredIntents.Reverse().ToArray();

            var commandQueue = queue.Entries().ToArray();

            var queueLength = commandQueue.Length;

            if (queueLength < requiredIntents.Length)
            {
                return false;
            }

            for (int i = 0; i < requiredIntents.Length; i++)
            {
                var item = commandQueue.ElementAt(queueLength - (i + 1));

                if (item.IntentName != requiredIntents[i])
                {
                    return false;
                }
            }

            return true;
        }

        public static bool QueueEndsWithIntentsOfType(this PersistentQueue<CommandDefinition> queue, string requiredIntent, string[] validIntents)
        {
            var commands = queue.Entries().ToArray();

            int index = 0;

            int lastIndex = -1;

            foreach (var command in commands)
            {
                if (command.IntentName == requiredIntent)
                {
                    lastIndex = index;
                }

                index++;
            }

            if (lastIndex == -1)
            {
                return false;
            }

            bool match = true;

            foreach (var remainingCommand in commands.Skip(lastIndex + 1))
            {
                if (!validIntents.Contains(remainingCommand.IntentName))
                {
                    match = false;
                }
            }

            return match;
        }
    }
}
