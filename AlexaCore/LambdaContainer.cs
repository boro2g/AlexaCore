using System;
using System.Collections.Generic;

namespace AlexaCore
{
    public class LambdaContainer
    {
        private readonly Action<string> _logger;

        private Dictionary<string, ContainerLocker> Container { get; set; }

        public LambdaContainer(Action<string> logger)
        {
            _logger = logger;
        }

        public void Reset()
        {
            Container = new Dictionary<string, ContainerLocker>();
        }

        public IEnumerable<string> ContainerKeys()
        {
            EnsureContainer();

            return Container.Keys;
        }

        public void RegisterType<T>(Func<T> valueActivator, bool lockValue = false)
        {
            string key = BuildKey<T>();

            RegisterType(key, valueActivator, lockValue);
        }

        public void RegisterType<T>(string key, Func<T> valueActivator, bool lockValue = false)
        {
            EnsureContainer();

            if (Container.ContainsKey(key))
            {
                var existing = Container[key];

                if (existing.LockValue)
                {
                    _logger?.Invoke("_container already contains locked key");

                    return;
                }

                _logger?.Invoke("_container already contains key - updating to new value");
            }

            Container[key] = new ContainerLocker(valueActivator(), lockValue);
        }

        public void SetLock(string key, bool lockValue)
        {
            EnsureContainer();

            if (Container.ContainsKey(key))
            {
                Container[key].LockValue = lockValue;
            }
        }

        public T Resolve<T>()
        {
            return Resolve<T>(BuildKey<T>());
        }

        public T Resolve<T>(string key)
        {
            EnsureContainer();

            if (!Container.ContainsKey(key))
            {
                return default(T);
            }

            return (T)Container[key].Value;
        }

        private string BuildKey<T>()
        {
            return $"_byType_{typeof(T).FullName}";
        }

        private void EnsureContainer()
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