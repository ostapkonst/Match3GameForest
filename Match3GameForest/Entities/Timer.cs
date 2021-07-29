using System;
using Match3GameForest.Config;
using Microsoft.Xna.Framework;

namespace Match3GameForest.Entities
{
    public class Timer : ITimer, IRegistering
    {
        private int _duration;
        private int _elapsedTime;

        public Timer(GameSettings gameSettings)
        {
            _duration = gameSettings.PlayingDuration * 1000;
            _elapsedTime = 0;
        }

        public void Restart()
        {
            _elapsedTime = 0;
        }

        public bool IsActive { get => TimeLeft > 0; }

        public int TimeLeft { get => (_duration - _elapsedTime) / 1000; }

        public void Update(GameTime gameTime)
        {
            if (!IsActive) return;

            if (_elapsedTime == 0) {
                OnStart?.Invoke();
            }

            _elapsedTime += gameTime.ElapsedGameTime.Milliseconds;

            if (!IsActive) {
                OnFinish?.Invoke();
            }
        }

        public event Action OnFinish;
        public event Action OnStart;
    }
}
