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
    #endregion
    public EntityData Data;

    public bool IsFacingRight;
    public int FacingDir => IsFacingRight ? 1 : -1;
    [Header("重力係數")]
    public float gravityScale;
    [Header("重力")]
    public static float GlobalGravity = -9.81f;

    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody>();
        rb.useGravity = false;
        skeleton = transform.Find("Skeleton").gameObject;
        anim = GetComponentInChildren<Animator>();
        spriteResolvers = skeleton.GetComponentsInChildren<SpriteResolver>().ToList();
        spriteLibrary = skeleton.GetComponentInChildren<SpriteLibrary>();
        FSM = new EntityFSM();
    }
    protected virtual void Start()
    {
        SetGravityScale(Data.gravityScale);
    }
    protected virtual void Update()
    {
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

    #region Flip
    public virtual void CheckIsFacingRight(bool isMovingRight)
    {
        if (isMovingRight != IsFacingRight)
            Flip();
    }
    public virtual void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;

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

    protected virtual void Die(float _delay = 1f)
    {
        Destroy(gameObject,_delay);
    }
}

