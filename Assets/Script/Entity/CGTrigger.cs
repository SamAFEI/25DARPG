using System.Collections.Generic;
using UnityEngine;

public class CGTrigger : MonoBehaviour
{
    public List<AudioClip> SexSFX;

    public List<AudioClip> SexVoices;
    public virtual void PlaySexSFXTrigger(int _value)
    {
        if (SexSFX.Count == 0 || SexSFX[_value] == null) return;
        AudioClip clip = SexSFX[_value];
        AudioManager.PlayOnPoint(AudioManager.VoiceSource, clip, transform.position);
    }
    public virtual void PlaySexVoiceTrigger(int _value)
    {
        if (SexVoices.Count == 0 || SexVoices[_value] == null) return;
        AudioClip clip = SexVoices[_value];
        AudioManager.PlayOnPoint(AudioManager.VoiceSource, clip, transform.position);
    }
}
