using Microsoft.Xna.Framework.Audio;
using Terraria.ModLoader;

namespace Virtuous.Sounds.Item
{
    public class SniperShot : ModSound
    {
        public override SoundEffectInstance PlaySound(ref SoundEffectInstance soundInstance, float volume, float pan, SoundType type)
        {
            soundInstance = sound.CreateInstance();
            soundInstance.Volume = volume * 1f;
            soundInstance.Pan = pan;
            return soundInstance;
        }
    }
}
