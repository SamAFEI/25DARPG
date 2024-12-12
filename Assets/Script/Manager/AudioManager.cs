using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public static AudioSource BGMSource { get; private set; }
    public static AudioSource SFXSource { get; private set; }
    public static AudioSource VoiceSource { get; private set; }
    public static AudioMixer AudioMixer { get; private set; }

    public AudioClip selectSEClip;
    public AudioClip bgmClip;
    public AudioClip battleBGMClip;
    public AudioClip bossBGMClip;

    public AudioClip woodBreakenClip;
    public AudioClip pickupClip;
    public AudioClip woodHitClip;
    public AudioClip rockHitClip;
    public AudioClip rockBreakenClip;

    public List<AudioClip> testClips;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            //DontDestroyOnLoad(this.gameObject);
        }
        else if (this != Instance)
        {
            Destroy(this.gameObject);
        }
        BGMSource = transform.Find("BGMSource").GetComponent<AudioSource>();
        SFXSource = transform.Find("SFXSource").GetComponent<AudioSource>();
        VoiceSource = transform.Find("VoiceSource").GetComponent<AudioSource>();
        AudioMixer = BGMSource.outputAudioMixerGroup.audioMixer;
    }

    public static void PlayBGM(int value)
    {
        AudioClip clip = null;
        if (value == 0)
        {
            clip = Instance.bgmClip;
        }
        else if (value == 1)
        {
            clip = Instance.battleBGMClip;
        }
        else if (value == 2)
        {
            clip = Instance.bossBGMClip;
        }
        if (BGMSource.clip != clip)
        {
            BGMSource.Stop();
            BGMSource.clip = clip;
            BGMSource.loop = true;
            BGMSource.Play();
        }
    }
    public static void PlayOnPoint(AudioSource audioSource, AudioClip clip, Vector3 point, float volume = 1f)
    {
        if (clip == null) return;
        GameObject obj = new GameObject(clip.name);
        obj.transform.position = point;
        AudioSource audio = obj.AddComponent<AudioSource>();
        audio.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
        audio.clip = clip;
        audio.volume = volume;
        audio.Play();
        Destroy(obj, clip.length);
    }
    public static void PlayWoodBreakenSFX(Vector3 point)
    {
        PlayOnPoint(SFXSource, Instance.woodBreakenClip, point);
    }
    public static void PlayWoodHitSFX(Vector3 point)
    {
        PlayOnPoint(SFXSource, Instance.woodHitClip, point);
    }
    public static void PlayRockBreakenSFX(Vector3 point)
    {
        PlayOnPoint(SFXSource, Instance.rockBreakenClip, point);
    }
    public static void PlayRockHitSFX(Vector3 point)
    {
        PlayOnPoint(SFXSource, Instance.rockHitClip, point);
    }
    public static void PlayItemPickupSFX(Vector3 point)
    {
        PlayOnPoint(SFXSource, Instance.pickupClip, point);
    }
    public static void AdjustVolume(VolumeType type, float volume)
    {
        AudioMixer.SetFloat(type.ToString(), volume);
    }
    public static float GetVolume(VolumeType type)
    {
        float _volume;
        AudioMixer.GetFloat(type.ToString(), out _volume);
        return _volume;
    }
    public static void PlayTest(VolumeType type)
    {
        if (type == VolumeType.BGMVolume)
        {
        }
        else if (type == VolumeType.SEVolume)
        {
            SFXSource.PlayOneShot(Instance.testClips[(int)type]);
        }
        else if (type == VolumeType.VoiceVolume)
        {
            VoiceSource.PlayOneShot(Instance.testClips[(int)type]);
        }
    }
    public static void PlaySelectSE()
    {
        SFXSource.PlayOneShot(Instance.selectSEClip);
    }
    public static void PausePlay(bool isPause)
    {
        if (isPause)
        {
            SFXSource.Pause();
            VoiceSource.Pause();
            return;
        }
        BGMSource.UnPause();
        SFXSource.UnPause();
        VoiceSource.UnPause();
    }
}

public enum VolumeType
{
    BGMVolume, SEVolume, VoiceVolume
}
