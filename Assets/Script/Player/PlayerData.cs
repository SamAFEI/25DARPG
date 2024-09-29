
using UnityEngine;

[CreateAssetMenu(menuName = "AFEI/PlayerData")]
public class PlayerData : EntityData
{
    [Header("Assists")]
    [Range(0.01f, 0.5f)] public float coyoteTime = 0.2f; //Grace period after falling off a platform, where you can still jump
    [Range(0.01f, 0.5f)] public float jumpInputBufferTime = 0.2f; //Grace period after pressing jump where a jump will be automatically performed once the requirements (eg. being grounded) are met.
    [Range(0.01f, 0.5f)] public float attackInputBufferTime = 0.2f;
    [Range(0.01f, 0.5f)] public float dashInputBufferTime = 0.2f;
    [Range(0.01f, 0.5f)] public float parryInputBufferTime = 0.2f;

    [Space(20)]
    [Header("Dash")]
    public float dashSpeed = 30f;
    public float dashDurationTime = 0.2f;
    public float dashResetTime = 0.5f;

    [Space(20)]
    public float canCounterTime = 0.2f;

    [Header("State")]
    public int MaxHP = 100;
    public float AttackDamage = 10;
}
