namespace AlexaCore
{
    public static class PersistentQueueExtensions
    {
		public static void Enqueue(this PersistentQueue<InputItem> queue, string value)
		{
			queue.Enqueue(new InputItem(value));
		}
	}
}
