using UnityEngine;

public class OrcBoss : Enemy
{
    public AudioClip voiceAlter;
    public AudioClip voiceDie;
    public GameObject dropItem;

    private bool isAlter;

    protected override void Update()
    {
        LastSuperArmedTime = 10f;
        base.Update();

        if (!isAlter && IsAlerting)
        {
            PlayVoiceTrigger(0);
            AttackMoveMaxSpeed = 3f;
            FSM.SetNextState(attack3State);
            isAlter = true;
        }
    }

    public override void Hurt(float _damage, bool _isHeaveyAttack = false)
    {
        base.Hurt(_damage, _isHeaveyAttack);
        if (IsDied)
        {
            PlayVoiceTrigger(1);
            Instantiate(dropItem, gameObject.transform.position, Quaternion.identity);
        }
    }

    public override void AlertStateAction()
    {
        if (IsKeepawaying)
        {
            FSM.SetNextState(keepawayState);
            return;
        }
        if (CanAttack3)
        {
            AttackMoveMaxSpeed = 3f;
            FSM.SetNextState(attack3State);
            return;
        }
        if (CanAttack1)
        {
            FSM.SetNextState(attack1State);
            return;
        }
        if (CanChase)
        {
            if (Random.Range(0, 100) > 80)
            {
                FSM.SetNextState(dashState);
                return;
            }
            FSM.SetNextState(chaseState);
            return;
        }
    }
    public override void Attack1Finish()
    {
        AttackMoveMaxSpeed = 3f;
        FSM.SetNextState(attack2State);
    }

    public override void PlayVoiceTrigger(int _value)
    {
        AudioClip clip = null;
        float volume = 0.8f;
        if (_value == 0) { clip = voiceAlter; }
        else if (_value == 1) { clip = voiceDie; }
        if (clip != null)
        {
            AudioManager.PlayOnPoint(AudioManager.VoiceSource, clip, transform.position, volume);
        }
    }
}
