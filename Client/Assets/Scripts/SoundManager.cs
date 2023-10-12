namespace DevelopersHub.ClashOfWhatecer
{
    using System.Collections;
    using System.Collections.Generic;
    using UnityEngine;

    public class SoundManager : MonoBehaviour
    {

        [Header("Sources")]
        public AudioSource musicSource = null;
        public AudioSource soundSource = null;

        [Header("Clips")]
        public AudioClip mainMusic = null;
        public AudioClip battleMusic = null;
        public AudioClip buttonClickSound = null;
        public AudioClip goldCollect = null;
        public AudioClip elixirCollect = null;
        public AudioClip buildStart = null;
        public AudioClip placeUnitSound = null;

        private static SoundManager _instance = null; public static SoundManager instanse { get { return _instance; } }

        private bool _musicMute = false; public bool musicMute { get { return _musicMute; } set { _musicMute = value; musicSource.mute = value; } }
        private bool _soundMute = false; public bool soundMute { get { return _soundMute; } set { _soundMute = value; soundSource.mute = value; } }

        private void Awake()
        {
            _instance = this;
            musicSource.playOnAwake = false;
            soundSource.playOnAwake = false;
            musicSource.loop = true;
            soundSource.loop = false;
            try
            {
                if (PlayerPrefs.HasKey("music_mute"))
                {
                    _musicMute = (PlayerPrefs.GetInt("music_mute") == 1);
                }
                if (PlayerPrefs.HasKey("sound_mute"))
                {
                    _soundMute = (PlayerPrefs.GetInt("sound_mute") == 1);
                }
            }
            catch (System.Exception)
            {

            }
            musicSource.mute = _musicMute;
            soundSource.mute = _soundMute;
        }

        public void PlayMusic(AudioClip clip)
        {
            if (clip == null) { return; }
            musicSource.clip = clip;
            musicSource.time = 0;
            musicSource.Play();
        }

        public void PlaySound(AudioClip clip)
        {
            if (clip == null) { return; }
            soundSource.PlayOneShot(clip);
        }

    }
}