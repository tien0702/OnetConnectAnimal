using UnityEngine;
using System;
using System.IO;
using Unity.VisualScripting;

namespace TT
{
    [System.Serializable]
    public class AudioConfig
    {
        public float MusicVolume = 1.0f;
        public bool MuteMusic = false;

        public float SfxVolume = 1.0f;
        public bool MuteSfx = false;
    }

    [RequireComponent(typeof(AudioSource))]
    public class AudioManager : SingletonBehaviour<AudioManager>
    {
        [SerializeField] protected string _audioConfigPath;
        [field: SerializeField] public AudioConfig AudioConfig { private set; get; }

        [Header("Settings")]
        [SerializeField] protected bool DontDestroy = false;
        [SerializeField] protected bool LoadAllAudio = false;
        [SerializeField] protected string MusicPath;
        [SerializeField] protected string SfxPath;

        [Header("Components")]
        [SerializeField] protected AudioSource _audioSource;
        [SerializeField] protected AudioClip[] Musics;
        [SerializeField] protected AudioClip[] Sfxs;

        public string Path => Application.streamingAssetsPath + _audioConfigPath;

        protected override void Awake()
        {
            base.Awake();
            _audioSource = GetComponent<AudioSource>();
            if (!File.Exists(Path))
                this.SaveAudioConfig();

            this.LoadAudioConfig();
            if (LoadAllAudio)
            {
                Musics = ResourceManager.Instance.GetAssets<AudioClip>(MusicPath);
                Sfxs = ResourceManager.Instance.GetAssets<AudioClip>(SfxPath);
            }

            if(DontDestroy)
            {
                DontDestroyOnLoad(this.gameObject);
            }
        }

        protected override void InitializeSingleton()
        {
            if(_instance != null)
            {
                Destroy(_instance.gameObject);
            }
            base.InitializeSingleton();
        }

        public void SaveAudioConfig()
        {
            string data = JsonUtility.ToJson(AudioConfig);
            File.WriteAllText(Path, data);
        }

        public void LoadAudioConfig()
        {
            string data = File.ReadAllText(Path);
            AudioConfig = JsonUtility.FromJson<AudioConfig>(data);
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
                _audioSource.clip = audio;
                _audioSource.loop = loop;
                _audioSource.volume = AudioConfig.MusicVolume;
                _audioSource.Play();
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
                _audioSource.PlayOneShot(audio, AudioConfig.SfxVolume);
            }
        }

        public virtual void ChangeMusicVolume(float volume)
        {
            AudioConfig.MusicVolume = volume;
            _audioSource.volume = volume;
        }

        public virtual void ChangeSFXVolume(float volume)
        {
            AudioConfig.SfxVolume = volume;
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
