using UnityEngine;

public class CGTrigger : MonoBehaviour
{
    public AudioData Data;
    public Animator Anim => GetComponent<Animator>();
    public bool isStart;

    public virtual void PlaySexSFXTrigger(int _value)
    {
        if (Data.SexSFX.Count == 0 || Data.SexSFX[_value] == null) return;
        AudioClip clip = Data.SexSFX[_value];
        AudioManager.PlayOnPoint(AudioManager.SFXSource, clip, transform.position);
    }
    public virtual void PlaySexVoiceTrigger(int _value)
    {
        if (Data.SexVoices.Count == 0 || Data.SexVoices[_value] == null) return;
        AudioClip clip = Data.SexVoices[_value];
        AudioManager.PlayOnPoint(AudioManager.VoiceSource, clip, transform.position);
    }

    public virtual void PlayStartVoice()
    {
        if (isStart)
        {
            PlaySexVoiceTrigger(0);
        }
        else
        {
            PlaySexVoiceTrigger(3);
        }
        isStart = false;
    }

    public void PlayIdle()
    {
        isStart = true;
        Anim.Play("Idle");
    }

    public void PlayOrgasm()
    {
        Anim.Play("Orgasm");
    }

    public void PlayInsertion()
    {
        Anim.Play("Insertion");
    }
}
