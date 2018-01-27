using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AudioManager : SingletonAsComponent<AudioManager>
{
    [SerializeField] AudioClip defaultBGM;
    [SerializeField] Dictionary<string, AudioClip> clips;

    [SerializeField] int defaultSoundCount = 5;

    [Header("Pitch")]
    [SerializeField] float minPitch = 0.95f;
    [SerializeField] float maxPitch = 1.05f;

    private AudioSource music;
    private List<AudioSource> sounds;

    // 알아서 꺼지지 않기 때문에 태그로 찾아서 끄고 켜야함.
    private Dictionary<string, AudioSource> loopSounds;

    public float soundVolume
    {
        get {
            return sounds[0].volume;
        }
        set {
            sounds.ForEach(sound => sound.volume = value);
        }
    }

    public float musicVolume
    {
        get {
            return music.volume;
        }
        set {
            music.volume = value;
        }
    }

    public bool useVibrate = true;

    protected override void Awake()
    {
        base.Awake();
        music = gameObject.AddComponent<AudioSource>();
        music.loop = true;

        sounds = new List<AudioSource>();
        for (int i = 0; i < defaultSoundCount; i++)
            AddSoundSource();

        // 갯수가 적지 않아 미리 생성하지 않아도 될 듯
        loopSounds = new Dictionary<string, AudioSource>(); 

        if (defaultBGM)
            PlayMusic(defaultBGM);
    }

    private AudioSource AddSoundSource(bool isLoopSound = false)
    {
        AudioSource source = gameObject.AddComponent<AudioSource>();
        sounds.Add(source);
        return source;
    }
    
    public static void Vibrate()
    {
        if(instance.useVibrate)
            Handheld.Vibrate();
    }

    public static void PlayMusic(string name)
    {
        AudioClip clip = instance.clips[name];
        PlayMusic(clip);
    }

    public static void PlaySound(string name)
    {
        AudioClip clip = instance.clips[name];
        PlaySound(clip);
    }

    public static void PlayMusic(AudioClip clip)
    {
        instance.music.clip = clip;
        instance.music.Play();
    }
    
    public static void StopMusic(AudioClip clip)
    {
        instance.music.clip = null;
        instance.music.Stop();
    }

    public static void PlaySound(AudioClip clip)
    {
        AudioSource source = instance.sounds.Where(s => s.isPlaying).SingleOrDefault();
        source = source ?? instance.AddSoundSource();
        source.pitch = Random.Range(instance.minPitch, instance.maxPitch);
        source.clip = clip;
        source.Play();
    }

}