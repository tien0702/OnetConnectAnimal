using UnityEngine;
using System;

namespace TT
{
    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        [Header("Settings")] 
        public bool LoadAllAudio = false;
        [Header("Music")] 
        [Range(0f, 1f)] public float MusicVolume;
        public bool MuteMusic;
        public string MusicPath;

        [Header("SFX")] 
        [Range(0f, 1f)] public float SfxVolume;
        public bool MuteSfx;
        public string SfxPath;

        [Header("Components")]
        [SerializeField] protected AudioClip[] Musics;
        [SerializeField] protected AudioClip[] Sfxs;

        protected AudioSource audio_source;

        protected override void Awake()
        {
            base.Awake();
            audio_source = GetComponent<AudioSource>();
            if (LoadAllAudio)
            {
                Musics = ResourceManager.Instance.GetAssets<AudioClip>(MusicPath);
                Sfxs = ResourceManager.Instance.GetAssets<AudioClip>(SfxPath);
            }
        }

        public virtual void PlayMusic(string name, bool loop)
        {
            AudioClip audio = GetMusicAudio(name);
            if (audio == null)
            {
                Debug.Log(string.Format("Sound {0} Not Found", name));
            }
            else
            {
                audio_source.clip = audio;
                audio_source.loop = loop;
                audio_source.Play();
            }
        }

        public virtual void PlaySFX(string name)
        {
            AudioClip audio = GetSFXAudio(name);
            if (audio == null)
            {
                Debug.Log(string.Format("SFX {0} Not Found", name));
            }
            else
            {
                audio_source.PlayOneShot(audio, SfxVolume);
            }
        }

        public virtual void ChangeMusicVolume(float volume)
        {
            MusicVolume = volume;
            audio_source.volume = volume;
        }

        public virtual void ChangeSFXVolume(float volume)
        {
            SfxVolume = volume;
        }

        protected AudioClip GetSFXAudio(string name)
        {
            AudioClip audio = null;
            if (!LoadAllAudio)
            {
                audio = ResourceManager.Instance.GetAsset<AudioClip>(SfxPath + name);
            }
            else
            {
                audio = Array.Find(Sfxs, s => s.name == name);
            }

            return audio;
        }

        protected AudioClip GetMusicAudio(string name)
        {
            AudioClip audio = null;
            if (!LoadAllAudio)
            {
                audio = ResourceManager.Instance.GetAsset<AudioClip>(MusicPath + name);
            }
            else
            {
                audio = Array.Find(Musics, s => s.name == name);
            }

            return audio;
        }

    }
}
