using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Match3GameForest.Config;

namespace Match3GameForest.Core
{
    public class AnimationWrapper : IAnimation, IRegistering
    {
        protected readonly ConcurrentBag<IAnimation> _animations;
        protected readonly ConcurrentBag<ManualResetEvent> _tokenSource;
        protected volatile bool _finished;

        public event Action Next; // TODO: https://habr.com/ru/post/240385/

        public AnimationWrapper()
        {
            _animations = new ConcurrentBag<IAnimation>();
            _tokenSource = new ConcurrentBag<ManualResetEvent>();
            _finished = true;
        }

        public bool IsAnimate => _animations.Any(x => x.IsAnimate) || !_finished;

        public IAnimation Add(IAnimation animation)
        {
            _animations.Add(animation);
            return this;
        }

        public void Waite()
        {
            var mre = new ManualResetEvent(false);
            _tokenSource.Add(mre);
            mre.WaitOne();
        }

        public virtual void Update(GameInputState state)
        {
            if (!_finished) return;

            foreach (var animation in _animations) {
                if (animation.IsAnimate) {
                    animation.Update(state);
                }
            }

            if (!IsAnimate) {
                Next?.Invoke();

                foreach (var token in _tokenSource) {
                    token.Set();
                }
            }
        }

        public void Dispose()
        {
            foreach (var animation in _animations) {
                animation.Dispose();
            }
            _tokenSource.Clear();
            _animations.Clear();
        }
    }
}
