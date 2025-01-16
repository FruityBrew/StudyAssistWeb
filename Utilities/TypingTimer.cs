using System;
using System.Timers;

namespace Utilities
{
    public class TypingTimer : IDisposable
    {
        private Timer _timer;
        private Action _action;
        private readonly int _interval;
        
        public bool IsActive {  get; private set; }

        public TypingTimer(int intervalSeconds = 2)
        {
            _interval = intervalSeconds;
            _timer = new Timer();
            _timer.Interval = _interval * 1000;
            _timer.AutoReset = false;
        }

        public void Activate(Action action)
        {
            if(_timer.Enabled)
                return;

            _action = action;

            _timer.Elapsed += _HandleTick;

            _timer.Start();

            IsActive = true;
        }

        private void _HandleTick(object? sender, EventArgs e)
        {
            _timer.Stop();
            _timer.Elapsed -= _HandleTick;
            IsActive = false;
            _action?.Invoke();
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }
    }
}
