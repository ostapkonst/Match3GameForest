using Match3GameForest.Config;
using Microsoft.Xna.Framework.Audio;

namespace Match3GameForest.Core
{
    public class SoundEffectWrapper : ISoundEffect, IRegistering
    {
        private readonly SoundEffectInstance _soundInstance;

        public SoundEffectWrapper(SoundEffect sound)
        {
            _soundInstance = sound.CreateInstance();
        }

        public float Volume
        {
            get => _soundInstance.Volume;
            set => _soundInstance.Volume = value;
        }

        public bool IsLooped
        {
            get => _soundInstance.IsLooped;
            set => _soundInstance.IsLooped = value;
        }

        public void Play() => _soundInstance.Play();
        public void Stop() => _soundInstance.Stop();
    }
}