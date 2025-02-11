using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public abstract class Entity : MonoBehaviour
{
    #region Components
    public Animator anim { get; private set; }
    public Rigidbody rb { get; private set; }
    public GameObject skeleton { get; private set; }
    public List<SpriteResolver> spriteResolvers { get; private set; } = new List<SpriteResolver>();
    public SpriteLibrary spriteLibrary { get; private set; }
    public EntityFSM FSM { get; private set; }
    public EntityFX entityFX { get; private set; }
    public GameObject attackMesh { get; private set; }
    public List<SpriteRenderer> sprites { get; private set; } = new List<SpriteRenderer>();
    public Collider entityCollider { get; private set; }
    #endregion

    public EntityData Data;
    public AudioData AudioData;
    public float AttackDamage { get; set; }
    public int CurrentHp { get; set; }
    public int MaxHp { get; set; }
    public bool IsHurting { get { return LastHurtTime > 0; } }
    public bool IsStunning { get { return LastStunTime > 0; } }
    public bool IsSuperArmeding { get { return LastSuperArmedTime > 0; } }
    public bool IsDied { get { return CurrentHp <= 0; } }
    public bool IsSexing { get; set; }
    public bool IsAttacking { get; set; }
    public bool IsHeaveyAttack { get; set; }
    public bool IsFacingRight { get; set; }
    public bool IsAttackBeDefended { get; set; }
    public bool CanDamage { get; set; }
    public bool CanBeStunned { get; set; }
    public bool IsMoveToTarget { get; set; }
    public AttackTypeEnum AttackType { get; set; }
    public bool IsEarthshatter { get; set; }
    public string myLayerName { get; set; }
    public int FacingDir => IsFacingRight ? 1 : -1;
    [Header("重力係數")]
    public float gravityScale;
    [Header("重力")]
    public static float GlobalGravity = -9.81f;

    #region Timers
    public float LastHurtTime { get; set; }
    public float LastStunTime { get; set; }
    public float LastSuperArmedTime { get; set; }
    #endregion

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        entityCollider = GetComponent<Collider>();
        rb.useGravity = false;
        skeleton = transform.Find("Skeleton").gameObject;
        attackMesh = skeleton.transform.Find("AttackMesh").gameObject;
        attackMesh.SetActive(false);
        anim = GetComponentInChildren<Animator>();
        spriteResolvers = skeleton.GetComponentsInChildren<SpriteResolver>().ToList();
        spriteLibrary = skeleton.GetComponentInChildren<SpriteLibrary>();
        sprites = skeleton.GetComponentsInChildren<SpriteRenderer>().ToList();
        entityFX = GetComponentInChildren<EntityFX>();
        FSM = new EntityFSM();
        IsFacingRight = transform.localScale.x < 0;
    }

    protected virtual void Start()
    {
        myLayerName = LayerMask.LayerToName(gameObject.layer);
        SetGravityScale(Data.gravityScale);
    }

    protected virtual void Update()
    {
        if (GameManager.IsPaused)
        {
            //SetGravityScale(0);
            return;
        }
        #region Timers
        LastHurtTime -= Time.deltaTime;
        LastStunTime -= Time.deltaTime;
        LastSuperArmedTime -= Time.deltaTime;
        #endregion
        FSM.ChangeNextState();
        FSM.currentState.OnUpdate();
    }

    protected virtual void FixedUpdate()
    {
        if (GameManager.IsPaused)
        {
            return;
        }
        FSM.currentState.OnFixedUpdate();
        SetGravity();
    }

    protected virtual void LateUpdate()
    {
        if (GameManager.IsPaused)
        {
            return;
        }
        FSM.currentState.OnLateUpdate();
        DoBillboard();
    }

    #region Animation Action
    public virtual void AnimationFinishTrigger() => FSM.currentState.AnimationFinishTrigger();
    public virtual void PlayAttackTrigger(int _index) => entityFX.DoPlayAttackFX(_index);
    public virtual void DamageTrigger(int _value)
    {
        CanDamage = _value > 0;
    }
    public virtual void StunnedTrigger(int _value)
    {
        CanBeStunned = _value > 0;
    }
    public virtual void SuperArmedTrigger(int _value)
    {
        LastSuperArmedTime = _value;
    }
    public virtual void SetAttackMoveDirection() { }
    public virtual void MoveToTargetTrigger(int _value)
    {
        IsMoveToTarget = _value > 0;
        if (!IsMoveToTarget) { SetZeroVelocity(); }
    }
    public virtual void CameraShakeTrigger()
    {
        CameraManager.Shake(10f, 0.3f);
    }
    public virtual void PlayAttackSFXTrigger(int _value)
    {
        if (AudioData == null) return;
        if (AudioData.AttackSFXs.Count == 0 || AudioData.AttackSFXs[_value] == null) return;
        AudioClip clip = AudioData.AttackSFXs[_value];
        AudioManager.PlayOnPoint(AudioManager.SFXSource, clip, transform.position, true);
    }
    public virtual void PlaySkillSFXTrigger(int _value)
    {
        if (AudioData == null) return;
        if (AudioData.SkillSFXs.Count == 0 || AudioData.SkillSFXs[_value] == null) return;
        AudioClip clip = AudioData.SkillSFXs[_value];
        AudioManager.PlayOnPoint(AudioManager.SFXSource, clip, transform.position, true);
    }
    public virtual void PlaySFXTrigger(int _value)
    {
        if (AudioData == null) return;
        if (AudioData.SFX.Count == 0 || AudioData.SFX[_value] == null) return;
        AudioClip clip = AudioData.SFX[_value];
        AudioManager.PlayOnPoint(AudioManager.SFXSource, clip, transform.position, true);
    }
    public virtual void PlayVoiceTrigger(int _value)
    {
        if (AudioData == null) return;
        if (AudioData.Voices.Count == 0 || AudioData.Voices[_value] == null) return;
        AudioClip clip = AudioData.Voices[_value];
        AudioManager.PlayOnPoint(AudioManager.VoiceSource, clip, transform.position, true);
    }
    public virtual void PlaySexSFXTrigger(int _value) 
    {
        if (AudioData == null) return;
        if (AudioData.SexSFX.Count == 0 || AudioData.SexSFX[_value] == null) return;
        AudioClip clip = AudioData.SexSFX[_value];
        AudioManager.PlayOnPoint(AudioManager.VoiceSource, clip, transform.position);
    }
    public virtual void PlaySexVoiceTrigger(int _value)
    {
        if (AudioData == null) return;
        if (AudioData.SexVoices.Count == 0 || AudioData.SexVoices[_value] == null) return;
        AudioClip clip = AudioData.SexVoices[_value];
        AudioManager.PlayOnPoint(AudioManager.VoiceSource, clip, transform.position);
    }
    public virtual void PlayHurtSFXTrigger() 
    {
        if (AudioData == null) return;
        if (AudioData.HurtSFXs == null) return; 
        int index = Random.Range(0 ,AudioData.HurtSFXs.Count());
        AudioClip clip = AudioData.HurtSFXs[index];
        AudioManager.PlayOnPoint(AudioManager.SFXSource, clip, transform.position, true);
    }
    public virtual void SetAttackType(AttackTypeEnum type)
    {
        AttackType = type;
    }
    #endregion

    #region Flip
    public virtual void CheckIsFacingRight(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
        {
            Flip();
        }
    }
    public virtual void Flip()
    {
        Vector3 scale = skeleton.transform.localScale;
        scale.x *= -1;
        skeleton.transform.localScale = scale;
        IsFacingRight = !IsFacingRight;
    }

    public virtual void FacingToPlayer()
    {
        CheckIsFacingRight(CheckRelativeVector(GameManager.Instance.player.transform.position).x > 0);
    }
    #endregion

    #region Velocity
    public virtual void SetZeroVelocity()
    {
        rb.velocity = Vector3.zero;
    }

    public void SetVelocity(float _x, float _y, float _z)
    {
        CheckIsFacingRight(_x > 0);
        rb.velocity = new Vector3(_x, _y, _z);
    }
    public void SetVelocity(Vector3 _vector)
    {
        SetVelocity(_vector.x, _vector.y, _vector.z);
    }
    #endregion

    #region Gravity
    public void SetGravityScale(float scale)
    {
        gravityScale = scale;
    }
    protected virtual void SetGravity()
    {
        if (rb.useGravity) return;
        Vector3 gravity = GlobalGravity * gravityScale * Vector3.up;
        rb.AddForce(gravity, ForceMode.Acceleration);
    }
    #endregion

    #region Hurt About Action
    public virtual IEnumerator HurtFlasher()
    {
        SetFlashColor();
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while (elapsedTime < 0.3f)
        {
            elapsedTime += Time.deltaTime;
            currentFlashAmount = Mathf.Lerp(1f, 0f, elapsedTime / 0.3f);
            SetFlashAmount(currentFlashAmount);
            yield return null;
        }
    }
    public virtual void SetFlashColor()
    {
        foreach (SpriteRenderer spr in sprites)
        {
            spr.material.SetColor("_FlashColor", Color.white);
        }
    }
    public virtual void SetFlashAmount(float _amount)
    {
        foreach (SpriteRenderer spr in sprites)
        {
            spr.material.SetFloat("_FlashAmount", _amount);
        }
    }
    public virtual void SexHurt()
    {
        //繼承用
    }
    #endregion 

    public virtual void ShotProjectile(int _index)
    {
        //繼承用
    }

    public virtual void Die(float _delay = 0.5f)
    {
        entityFX.enabled = false;
        Destroy(gameObject, _delay);
    }

    public void IgnoreLayersTrigger(int _value)
    {
        if (_value > 0)
        {
            gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
            return;
        }
        gameObject.layer = LayerMask.NameToLayer(myLayerName);
    }


    /// <summary>
    /// 紙娃娃
    /// </summary>
    /// <param name="_asset"></param>
    public virtual void SetSpriteLibraryAsset(SpriteLibraryAsset _asset)
    {
        spriteLibrary.spriteLibraryAsset = _asset;
    }

    /// <summary>
    /// 面向Camera
    /// </summary>
    protected void DoBillboard()
    {
        transform.rotation = Camera.main.transform.rotation;
        transform.rotation = Quaternion.Euler(0f, transform.rotation.eulerAngles.y, 0f);
    }

    /// <summary>
    /// 計算相對位置
    /// </summary>
    /// <param name="_target"></param>
    /// <returns> 右邊 X > 0  前方 Z > 0 </returns>
    protected Vector3 CheckRelativeVector(Vector3 _target)
    {
        Vector3 _vector = _target - this.transform.position;
        float faceRight = Vector3.Cross(transform.forward, _vector).y; //檢查是否在右邊 X > 0
        float faceFoward = Vector3.Dot(transform.forward, _vector); //檢查是否在前方 Z > 0
        return new Vector3(faceRight, 0, faceFoward);
    }

}

