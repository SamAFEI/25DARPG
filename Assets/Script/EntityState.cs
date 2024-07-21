using UnityEngine;

public abstract class EntityState
{
    protected float stateTime;
    public EntityFSM FSM;
    public Entity Entity;
    protected string animName;
    protected bool isAnimFinish;

    public EntityState(Entity _entity, EntityFSM _FSM, string _animName)
    {
        Entity = _entity;
        FSM = _FSM;
        animName = _animName;
    }

    public virtual void OnEnter()
    {
        isAnimFinish = false;
        AnimatorPlay();
    }

    public virtual void OnUpdate()
    {
        stateTime -= Time.deltaTime;
    }

    public virtual void OnFixedUpdate()
    {
    }

    public virtual void OnLateUpdate()
    {
    }

    public virtual void OnExit()
    {
    }

    public virtual void AnimatorPlay()
    {
        if (animName != "")
        {
            Entity.anim.Play(animName);
        }
    }

    public virtual void AnimationFinishTrigger()
    {
        isAnimFinish = true;
    }
}
