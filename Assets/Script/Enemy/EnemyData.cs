using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "AFEI/EnemyData")]
public class EnemyData : EntityData
{
    [Header("State")]
    public int maxHP = 100;
    public bool isBoos;
    public float dashSpeed = 1.4f;

    [Header("Attack")]
    public float attack1ResetTime = 1f;
    public float attack1Damage = 10f;
    public bool attack1IsHeavy;
    public float attack2ResetTime = 1f;
    public float attack2Damage = 10f;
    public bool attack2IsHeavy;
    public float attack3ResetTime = 1f;
    public float attack3Damage = 10f;
    public bool attack3IsHeavy;

    [Header("Check")]
    public float alertDistance = 5f;
    public float catchDistance = 2f;
    public float attack1Distance = 5f;
    public float attack2Distance = 5f;
    public float attack3Distance = 5f;

    [Header("Projectiles")]
    public List<GameObject> projectiles = new List<GameObject>();

    [Header("ForeplayAnimtion")]
    public List<AnimationClip> foreplayAnims = new List<AnimationClip>();
    [Header("SexAnimtion")]
    public List<AnimationClip> sexAnims = new List<AnimationClip>();
}
