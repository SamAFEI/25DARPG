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
    #endregion

    public EntityData Data;
    public bool IsHurting { get { return LastHurtTime > 0; } }
    public bool IsStunning { get { return LastStunTime > 0; } }
    public bool IsSuperArmeding { get { return LastSuperArmedTime > 0; } }
    public bool IsSexing;

    public bool IsHeaveyAttack;
    public bool IsFacingRight;
    public int FacingDir => IsFacingRight ? 1 : -1;
    [Header("重力係數")]
    public float gravityScale;
    [Header("重力")]
    public static float GlobalGravity = -9.81f;

    #region Timers
    public float LastHurtTime;
    public float LastStunTime;
    public float LastSuperArmedTime;
    #endregion
    public string sexAnimName;
    public float AttackDamage;
    public int CurrentHp;
    public int MaxHp;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
    }

    protected virtual void Start()
    {
        SetGravityScale(Data.gravityScale);
    }

    protected virtual void Update()
    {
        #region Timers
        LastHurtTime -= Time.deltaTime;
        LastStunTime -= Time.deltaTime;
        LastSuperArmedTime -= Time.deltaTime;
        #endregion

        FSM.currentState.OnUpdate();
    }

    protected virtual void FixedUpdate()
    {
        FSM.currentState.OnFixedUpdate();
        SetGravity();
    }

    protected virtual void LateUpdate()
    {
        FSM.currentState.OnLateUpdate();
    }


    public void AnimationFinishTrigger() => FSM.currentState.AnimationFinishTrigger();
    public void PlayAttackTrigger(int _index) => entityFX.DoPlayAttackFX(_index);
    public void DoSexHurt() => SexHurt();

    #region Flip
    public virtual void CheckIsFacingRight(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Flip();
    }
    public virtual void Flip()
    {
        Vector3 scale = skeleton.transform.localScale;
        scale.x *= -1;
        skeleton.transform.localScale = scale;

        IsFacingRight = !IsFacingRight;
    }
    #endregion

    #region Velocity
    public void SetZeroVelocity()
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

    public virtual void SetSpriteLibraryAsset(SpriteLibraryAsset _asset)
    {
        spriteLibrary.spriteLibraryAsset = _asset;
    }

    #region Hurt
    public virtual IEnumerator HurtFlasher()
    {
        SetFlashColor();
        float currentFlashAmount = 0f;
        float elapsedTime = 0f;
        while(elapsedTime < 0.3f)
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

    }
    #endregion

    protected virtual void Die(float _delay = 1f)
    {
        Destroy(gameObject, _delay);
    }
}

