using System.Collections.Generic;
using AlexaCore.Intents;

namespace AlexaCore
{
    public class AlexaContext
    {
	    public static IntentFactory IntentFactory { get; private set; }

        public static IntentParameters Parameters { get; private set; }

        public static IntentNames IntentNames { get; private set; }

		private static Dictionary<string, ContainerLocker> Container { get; set; }

		public AlexaContext(IntentFactory intentFactory, IntentNames intentNames, IntentParameters parameters)
		{
			IntentFactory = intentFactory;

		    Parameters = parameters;

		    IntentNames = intentNames ?? new IntentNames();
		}

		public static void RegisterType<T>(string key, T value, bool lockValue = false)
		{
		    EnsureContainer();

			if (Container.ContainsKey(key))
			{
			    var existing = Container[key];

			    if (existing.LockValue)
			    {
			        Parameters.Logger.Log("Container already contains locked key");

                    return;
			    }

			    Parameters.Logger.Log("Container already contains key - updating to new value");
			}

			Container[key] = new ContainerLocker(value, lockValue);
		}

        public static void SetLock(string key, bool lockValue)
        {
            EnsureContainer();

            if (Container.ContainsKey(key))
            {
                Container[key].LockValue = lockValue;
            }
        }

        public static T Resolve<T>(string key)
		{
		    EnsureContainer();

            if (!Container.ContainsKey(key))
			{
				return default(T);
			}

			return (T) Container[key].Value;
		}

        private static void EnsureContainer()
        {
            if (Container == null)
            {
                Container = new Dictionary<string, ContainerLocker>();
            }
        }

        protected class ContainerLocker
        {
            public object Value { get; }
            public bool LockValue { get; set; }

            public ContainerLocker(object value, bool lockValue)
            {
                Value = value;
                LockValue = lockValue;
            }
        }
	}
}
