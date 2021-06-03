using System.Collections;
using System.Collections.Generic;
using Extension;
using UnityComponents;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Trepanation.UnityComponents
{
    [RequireComponent(typeof(AudioSource))]
    public class SoundManager : SingletonDestroy<SoundManager>
    {
        [SerializeField] private Sprite muteSprite;
        [SerializeField] private Sprite unmuteSprite;
        [SerializeField] private List<AudioClip> breathClips;
        [SerializeField] private List<AudioClip> grinderToolClips;
        [SerializeField] private List<AudioClip> cupOfHeadClips;
        [SerializeField] private List<AudioClip> staplerClips;
        [SerializeField] private List<AudioClip> zombieReactionClips;
        [SerializeField] private List<AudioClip> winClips;

        private AudioSource _audioSource;
        private AudioSource _breathAudioSource;
        private AudioSource _grinderToolAudioSource;
        private AudioClip _audioClip;

        private bool _grinderToolRun = false;
        private bool _mute;
        
        private void Start()
        {
            _mute = GameManager.Instance.mute;
            
            _audioSource = gameObject.GetComponent<AudioSource>();
            
            _breathAudioSource = gameObject.AddComponent<AudioSource>();
            _breathAudioSource.volume = 0.5f;
            StartCoroutine(nameof(PlayBreath));

            _grinderToolAudioSource = gameObject.AddComponent<AudioSource>();
            _grinderToolAudioSource.clip = grinderToolClips[0];
            _grinderToolAudioSource.playOnAwake = false;
            _grinderToolAudioSource.loop = true;
            _grinderToolAudioSource.Play();
            _grinderToolAudioSource.volume = 0;
            _grinderToolAudioSource.pitch = 0.5f;
            StartCoroutine(nameof(PlayGrinderTool));
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
            _grinderToolAudioSource.mute = GameManager.Instance.mute;
        }

        public void PlayOpenCupOfHead(float delay = 0)
        {
            var soundData = new SoundData(cupOfHeadClips[Random.Range(0, cupOfHeadClips.Count)], delay);
            StartCoroutine(nameof(PlayOneShot), soundData);
        }
        
        public void PlayStapler(float delay = 0)
        {
            var soundData = new SoundData(staplerClips[Random.Range(0, staplerClips.Count)], delay);
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

        public void GrinderToolPlay(bool play)
        {
            _grinderToolRun = play;
        }

        private IEnumerator PlayGrinderTool()
        {
            while (true)
            {
                if (_grinderToolRun)
                {
                    if (_grinderToolAudioSource.volume < 1)
                        _grinderToolAudioSource.volume += Time.deltaTime * 3;
                    if (_grinderToolAudioSource.pitch < 0.9f)
                        _grinderToolAudioSource.pitch += Time.deltaTime;
                }
                else
                {
                    if (_grinderToolAudioSource.volume > 0)
                        _grinderToolAudioSource.volume -= Time.deltaTime * 3;
                    if (_grinderToolAudioSource.pitch > 0.5f)
                        _grinderToolAudioSource.pitch -= Time.deltaTime;
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