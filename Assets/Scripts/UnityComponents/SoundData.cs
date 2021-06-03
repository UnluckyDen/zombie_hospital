using UnityEngine;

namespace UnityComponents
{
    public struct SoundData
    {
        public AudioClip AudioClip;
        public float Delay;

        public SoundData(AudioClip audioClip, float delay)
        {
            AudioClip = audioClip;
            Delay = delay;
        }
    }
}