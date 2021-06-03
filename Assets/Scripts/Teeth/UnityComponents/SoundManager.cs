using System;
using System.Collections;
using System.Collections.Generic;
using Extension;
using UnityComponents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Teeth.UnityComponents
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : SingletonDestroy<SoundManager>
    {
        [SerializeField] private Sprite muteSprite;
        [SerializeField] private Sprite unmuteSprite;
        [SerializeField] private List<AudioClip> breathClips;
        [SerializeField] private List<AudioClip> hammerClips;
        [SerializeField] private List<AudioClip> brokenTeethClips;
        [SerializeField] private List<AudioClip> zombieReactionClips;
        [SerializeField] private List<AudioClip> winClips;

        private AudioSource _audioSource;
        private AudioSource _breathAudioSource;
        
        private AudioClip _audioClip;
        private float _delay;
        
        private bool _mute;

        private void Start()
        {
            _mute = GameManager.Instance.mute;
            _audioSource = gameObject.GetComponent<AudioSource>();
            
            _breathAudioSource = gameObject.AddComponent<AudioSource>();
            _breathAudioSource.volume = 0.5f;
            StartCoroutine(nameof(PlayBreath));
        }

        public void MuteButton()
        {
            GameManager.Instance.mute = !GameManager.Instance.mute;
        }

        public Sprite GetMuteSprite()
        {
            return GameManager.Instance.mute ? muteSprite : unmuteSprite;
        }

        private void Update()
        {
            _audioSource.mute = GameManager.Instance.mute;
            _breathAudioSource.mute = GameManager.Instance.mute;
        }

        public void PlayHammer(float delay = 0)
        {
            var soundData = new SoundData(hammerClips[Random.Range(0, hammerClips.Count)], delay);
            StartCoroutine(nameof(PlayOneShot), soundData);
        }

        public void PlayBrokenTeeth(float delay = 0)
        {
            var soundData = new SoundData(brokenTeethClips[Random.Range(0, brokenTeethClips.Count)], delay);
            StartCoroutine(nameof(PlayOneShot), soundData);
        }
        
        public void PlayZombieReaction(float delay = 0)
        {
            var soundData = new SoundData(zombieReactionClips[Random.Range(0, zombieReactionClips.Count)], delay);
            StartCoroutine(nameof(PlayOneShot), soundData);
        }
        
        public void PlayWin(float delay = 0)
        {
            var soundData = new SoundData(winClips[Random.Range(0, winClips.Count)], delay);
            StartCoroutine(nameof(PlayOneShot), soundData);
        }

        public void SetBreathVolume(float volume)
        {
            _breathAudioSource.volume = volume;
        }

        private IEnumerator PlayOneShot(SoundData soundData)
        {
            yield return new WaitForSeconds(soundData.Delay);
            _audioSource.PlayOneShot(soundData.AudioClip);
        }

        private IEnumerator PlayBreath()
        {
            var breathIndex = 0;
            while (true)
            {
                _breathAudioSource.PlayOneShot(breathClips[breathIndex]);
                yield return new WaitForSeconds(breathClips[breathIndex].length);
                if (breathIndex + 1 == breathClips.Count)
                    breathIndex = 0;
                else breathIndex++;
            }
        }
    }
}