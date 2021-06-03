using System;
using System.Collections;
using System.Collections.Generic;
using Extension;
using UnityComponents;
using UnityEngine;
using UnityEngine.InputSystem.Utilities;
using Random = UnityEngine.Random;

namespace Limb.UnityComponents
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : SingletonDestroy<SoundManager>
    {
        [SerializeField] private Sprite muteSprite;
        [SerializeField] private Sprite unmuteSprite;
        [SerializeField] private List<AudioClip> breathClips;
        [SerializeField] private List<AudioClip> detachClips;
        [SerializeField] private List<AudioClip> screamClips;
        [SerializeField] private List<AudioClip> screwdriverClips;
        [SerializeField] private List<AudioClip> winClips;
        [SerializeField] private List<AudioClip> zombieReactionClips;
        
        private AudioSource _audioSource;
        private AudioSource _breathAudioSource;
        private AudioSource _screwdriverAudioSource;
        private AudioClip _audioClip;

        private bool _screwdriverRun = false;
        
        private void Start()
        {
            _audioSource = gameObject.GetComponent<AudioSource>();
            
            _breathAudioSource = gameObject.AddComponent<AudioSource>();
            _breathAudioSource.volume = 0.5f;
            StartCoroutine(nameof(PlayBreath));

            _screwdriverAudioSource = gameObject.AddComponent<AudioSource>();
            _screwdriverAudioSource.clip = screwdriverClips[0];
            _screwdriverAudioSource.playOnAwake = false;
            _screwdriverAudioSource.loop = true;
            _screwdriverAudioSource.Play();
            _screwdriverAudioSource.volume = 0;
            _screwdriverAudioSource.pitch = 0.5f;
            StartCoroutine(nameof(PlayScrewdriver));
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
            _screwdriverAudioSource.mute = GameManager.Instance.mute;
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

        public void PlayDetach(float delay = 0)
        {
            var soundData = new SoundData(detachClips[Random.Range(0, detachClips.Count)], delay);
            StartCoroutine(nameof(PlayOneShot), soundData);
        }
        
        public void PlayZombieScream(float delay = 0)
        {
            var soundData = new SoundData(screamClips[Random.Range(0, screamClips.Count)], delay);
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

        public void ScrewDriverPlay(bool play)
        {
            _screwdriverRun = play;
        }

        private IEnumerator PlayScrewdriver()
        {
            while (true)
            {
                if (_screwdriverRun)
                {
                    if (_screwdriverAudioSource.volume < 1)
                        _screwdriverAudioSource.volume += Time.deltaTime * 3;
                    if (_screwdriverAudioSource.pitch < 0.8f)
                        _screwdriverAudioSource.pitch += Time.deltaTime;
                }
                else
                {
                    if (_screwdriverAudioSource.volume > 0)
                        _screwdriverAudioSource.volume -= Time.deltaTime * 3;
                    if (_screwdriverAudioSource.pitch > 0.5f)
                        _screwdriverAudioSource.pitch -= Time.deltaTime;
                }
                yield return new WaitForSeconds(Time.deltaTime);
            }
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