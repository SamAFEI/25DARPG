using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AFEI/AudioData")]
public class AudioData : ScriptableObject
{
    public AudioClip HurtSFX;

    public List<AudioClip> AttackSFXs;

    public List<AudioClip> SkillSFXs;

    public List<AudioClip> SFX;

    public List<AudioClip> Voices;

    public List<AudioClip> SexSFX;

    public List<AudioClip> SexVoices;
}
