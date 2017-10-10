using System;
using System.Diagnostics;

namespace AlexaCore
{
    class OperationTimer : IDisposable
    {
        private readonly Action<string> _logger;
        private readonly string _prefix;
        private readonly bool _enabled;
        private readonly Stopwatch _stopwatch;

        public OperationTimer(Action<string> logger, string prefix = "", bool enabled = true)
        {
            _logger = logger;

            _prefix = prefix;

            _enabled = enabled;

            _stopwatch = Stopwatch.StartNew();
        }

        public long ElapsedMilliseconds => _stopwatch.ElapsedMilliseconds;

        public void Dispose()
        {
            _stopwatch.Stop();

            if (_enabled)
            {
                _logger($"{_prefix}. Took {_stopwatch.ElapsedMilliseconds} ms");
            }
        }
    }
}