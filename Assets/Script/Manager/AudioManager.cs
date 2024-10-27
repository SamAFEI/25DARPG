using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance { get; private set; }
    public static AudioSource BGMSource { get; private set; }
    public static AudioSource SFXSource { get; private set; }
    public static AudioSource VoiceSource { get; private set; }

    public AudioClip bgmClip;
    public AudioClip battleBGMClip;
    public AudioClip bossBGMClip;

    public AudioClip woodBreakenClip;
    public AudioClip pickupClip;
    public AudioClip woodHitClip;
    public AudioClip rockHitClip;
    public AudioClip rockBreakenClip;

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
    }
    private void Start()
    {
        PlayBGM(0);
    }

    public static void PlayBGM(int value)
    {
        BGMSource.Stop();
        if (value == 0)
        {
            BGMSource.clip = Instance.bgmClip;
        }
        else if(value == 1)
        {
            BGMSource.clip = Instance.battleBGMClip;
        }
        else if (value == 2)
        {
            BGMSource.clip = Instance.bossBGMClip;
        }
        BGMSource.loop = true;
        BGMSource.Play();
    }

    public static void PlayOnPoint(AudioSource audioSource, AudioClip clip, Vector3 point, float volume = 1f)
    {
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
}
