using System;
using System.Collections.Generic;
using System.Linq;
using Match3GameForest.Config;

namespace Match3GameForest.Core
{
    public class AnimationWrapper : IAnimation, IRegistering
    {
        protected readonly List<IAnimation> _animations;
        protected bool _finished;
        private bool _animaExec;

        public AnimationWrapper()
        {
            _animations = new List<IAnimation>();
            _finished = true;
            _animaExec = false;
        }

        public Action AfterAnimate { get; set; }

        public bool IsAnimate => !_finished || _animations.Any(x => x.IsAnimate);

        public IAnimation Add(IAnimation animation)
        {
            _animations.Add(animation);
            if (_animaExec) {
                _animaExec = !IsAnimate;
            }
            return this;
        }

        public virtual void Update(GameInputState state)
        {
            var updatable = _animations.Where(x => x.IsAnimate).ToList();

            foreach (var animation in updatable) {
                animation.Update(state);
            }

            if (!_animaExec && !IsAnimate) {
                _animaExec = true;
                AfterAnimate?.Invoke();
            }
        }
    }
}
