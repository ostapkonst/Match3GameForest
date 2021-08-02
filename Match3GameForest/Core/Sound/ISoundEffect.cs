using Microsoft.Xna.Framework.Audio;

namespace Match3GameForest.Core
{
    public interface ISoundEffect
    {
        float Volume { get; set; }
        bool IsLooped { get; set; }
        void Play();
        void Stop();
    }
}