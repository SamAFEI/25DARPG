using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.U2D.Animation;

public class Player : Entity
{
    #region Components
    public PlayerInput input { get; set; }
    public UI_PlayerStatus uiPlayerStatus { get; private set; }
    public UI_PlayerHint uiPlayerHint { get; private set; }
    public UI_Interactable uiInteractable { get; private set; }
    public GameObject weaponPoint;
    #endregion

    #region FSM States
    public PlayerStateIdle idleState { get; set; }
    public PlayerStateRun runState { get; set; }
    public PlayerStateSword swordState { get; set; }
    public PlayerStateDash dashState { get; set; }
    public PlayerStateStun stunState { get; set; }
    public PlayerStateParry parryState { get; set; }
    public PlayerStateParrySuscces parrySusccesState { get; set; }
    public PlayerStateHurt hurtState { get; set; }
    public PlayerStateSex sexState { get; set; }
    public PlayerStateDie dieState { get; set; }
    #endregion
    public new PlayerData Data => (PlayerData)base.Data;
    public bool CanMovement { get { return !input.IsAttacking && !input.IsDashing && !input.IsParrying 
                && !IsStunning && !IsSexing && !IsSystem && !IsHurting && !IsDied; } }
    public Vector3 MoveInput { get { return input.MoveInput; } }
    public bool IsSystem { get; set; }
    public bool IsBreak1 { get; set; }
    public bool IsBreak2 { get; set; }
    public string sexAnimName { get; set; }
    public string testSexAnim { get; set; }
    public int testSexAnimIndex { get; set; }
    public bool hasRockAttack { get { return InventoryManager.Instance.inventories.Where(x => x.item.name == "Magic Sword Fragment").ToList().Count() > 0; } }

    public float LastResistTime;

    public SpriteLibraryAsset SLAssetNormal;
    public SpriteLibraryAsset SLAssetBreak1;
    public SpriteLibraryAsset SLAssetBreak2;
    public AudioClip sfxParrySuccess;
    public AudioClip voiceParrySuccess;
    public AudioClip sfxAttack;
    public AudioClip voiceAttack;
    public AudioClip sfxHPPotion;
    public AudioClip voiceHurt;
    public AudioClip voiceSex;
    public AudioClip sfxRockAttack;

    protected override void Awake()
    {
        base.Awake();
        MaxHp = Data.MaxHP;
        CurrentHp = MaxHp;
        AttackDamage = Data.AttackDamage;
        input = GetComponent<PlayerInput>();
        uiPlayerHint = GetComponentInChildren<UI_PlayerHint>();
        idleState = new PlayerStateIdle(this, FSM, "Idle");
        runState = new PlayerStateRun(this, FSM, "Run");
        swordState = new PlayerStateSword(this, FSM, "Sword1");
        dashState = new PlayerStateDash(this, FSM, "Dash");
        stunState = new PlayerStateStun(this, FSM, "Stun");
        parryState = new PlayerStateParry(this, FSM, "Parry");
        parrySusccesState = new PlayerStateParrySuscces(this, FSM, "ParrySuscces");
        hurtState = new PlayerStateHurt(this, FSM, "Hurt");
        sexState = new PlayerStateSex(this, FSM, "Sex");
        dieState = new PlayerStateDie(this, FSM, "Die");
        FSM.InitState(idleState);
    }

    protected override void Start()
    {
        base.Start();
        uiPlayerStatus = GameObject.FindObjectOfType<UI_PlayerStatus>();
        uiInteractable = GameObject.Find("UI_Canvas").transform.Find("UI_Interactable").GetComponent<UI_Interactable>();
    }

    protected override void Update()
    {
        base.Update();
        LastResistTime -= Time.deltaTime;

        if (Input.GetKeyDown(KeyCode.Alpha4))
        {
            testSexAnimIndex++;
            if (testSexAnimIndex > 3) testSexAnimIndex = 1;
            testSexAnim = "OrcForeplay" + testSexAnimIndex.ToString().PadLeft(2, '0');
            sexAnimName = testSexAnim;
            IsSexing = true;
            Debug.Log(sexAnimName);
            FSM.SetNextState(sexState);
        }
        if (Input.GetKeyDown(KeyCode.Alpha5))
        {
            testSexAnimIndex++;
            if (testSexAnimIndex > 4) testSexAnimIndex = 1;
            testSexAnim = "OrcSex" + testSexAnimIndex.ToString().PadLeft(2, '0');
            sexAnimName = testSexAnim;
            IsSexing = true;
            Debug.Log(sexAnimName);
            FSM.SetNextState(sexState);
        }
        if (Input.GetKeyDown(KeyCode.Alpha0))
        {
            Hurt(10000);
        }
        if (Input.GetKeyDown(KeyCode.Alpha9))
        {
            entityFX.DoPlayBuffFX(0);
        }
        if (Input.GetKeyDown(KeyCode.Alpha8))
        {
            StartCoroutine(DemoSword3());
        }
        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            Hurt(-10000);
        }
    }

    protected override void FixedUpdate()
    {
        base.FixedUpdate();
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        if (other.tag == "Projectile")
        {
            //•Œß¿ª•¥±º
            if ((other.transform.position - entityCollider.ClosestPoint(other.transform.position)).magnitude > 0.3f) { return; }
            if (IsHurting || IsStunning || IsSuperArmeding) { return; }
            ProjectileBase _projectile = other.GetComponentInParent<ProjectileBase>();
            Repel(_projectile.transform.position);
            Hurt(_projectile.AttackDamage, _projectile.IsHeaveyAttack);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.tag == "EnemyAttack")
        {
            Enemy _enemy = other.GetComponentInParent<Enemy>();
            if (CanBeStunned && _enemy.CanBeStunned && !_enemy.IsStunning)
            {
                TimerManager.Instance.DoFrozenTime(0.1f);
                PlaySFXTrigger(1);
                PlayVoiceTrigger(1);
                entityFX.DoPlayHitFX(0, weaponPoint.transform.position);
                _enemy.LastStunTime = 3f;
                FSM.SetNextState(parrySusccesState);
                return;
            }
            if (IsHurting || IsStunning || IsSuperArmeding || !_enemy.CanDamage) { return; }
            if (_enemy.IsCatching)
            {
                if (_enemy.IsDied) { return; }
                if (!IsBreak1)
                {
                    sexAnimName = _enemy.Data.foreplayAnims[Random.Range(0, _enemy.Data.foreplayAnims.Count)].name;
                }
                else
                {
                    sexAnimName = _enemy.Data.sexAnims[Random.Range(0, _enemy.Data.sexAnims.Count)].name;
                }
                IsSexing = true;
                FSM.SetNextState(sexState);
                StartCoroutine(Resist());
                GameManager.AddSexEnemies(_enemy);
                return;
            }
            float damage = _enemy.AttackDamage * Random.Range(0.80f, 1.20f);
            Repel(_enemy.transform.position, _enemy.IsHeaveyAttack);
            Hurt(damage, _enemy.IsHeaveyAttack);
        }
    }

    public override void Die(float _delay = 0.8f)
    {
        input.inputHandle.Character.Disable();
        input.inputHandle.SexAction.Disable();
        UI_Canvas.Instance.FadeInUI_Die();
    }

    #region Animation Action
    public override void PlaySFXTrigger(int _value)
    {
        AudioClip clip = null;
        if (_value == 0) { clip = sfxAttack; }
        else if (_value == 1) { clip = sfxParrySuccess; }
        else if (_value == 2) { clip = sfxHPPotion; }
        else if (_value == 4) { clip = sfxRockAttack; }
        if (clip != null)
        {
            AudioManager.PlayOnPoint(AudioManager.SFXSource, clip, transform.position);
        }
    }

    public override void PlayVoiceTrigger(int _value)
    {
        AudioClip clip = null;
        if (_value == 0) { clip = voiceAttack; }
        else if (_value == 1) { clip = voiceParrySuccess; }
        else if (_value == 2) { clip = voiceHurt; }
        else if (_value == 3) { clip = voiceSex; }
        if (clip != null)
        {
            AudioManager.PlayOnPoint(AudioManager.VoiceSource, clip, transform.position);
        }
    }
    #endregion

    #region Hurt
    public override IEnumerator HurtFlasher()
    {
        SetFlashColor();
        while (IsHurting)
        {
            SetFlashAmount(1);
            yield return new WaitForSeconds(.1f);
            SetFlashAmount(0);
            yield return new WaitForSeconds(.1f);
        }
    }

    public virtual void Hurt(float _damage, bool _isHeaveyAttack = false)
    {
        //DoDamageHp
        if (_damage > 0)
        {
            if (IsHurting || IsStunning || IsSuperArmeding) { return; }
            CameraManager.Shake(3f, 0.1f);
            LastHurtTime = Data.hurtResetTime;
            StartCoroutine(HurtFlasher());
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiPlayerStatus.DoLerpHealth();
        }
        //HealHp
        else
        {
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiPlayerStatus.DoLerpHealth(1f);
            entityFX.DoPlayBuffFX(0);
        }
        if (_isHeaveyAttack)
        {
            LastStunTime = Data.hurtResetTime;
        }
    }

    public override void SexHurt()
    {
        if (!IsSexing) { return; }
        CameraManager.Shake(8f, 0.2f);
        foreach (Enemy _enemy in GameManager.Instance.sexEnemies)
        {
            float _damage = _enemy.AttackDamage;
            CurrentHp = (int)Mathf.Clamp(CurrentHp - _damage, 0, MaxHp);
            uiPlayerStatus.DoLerpHealth();
        }
    }

    public void SetBreak(bool reset = false)
    {
        if (reset)
        {
            IsBreak1 = false;
            IsBreak2 = false;
            SetSpriteLibraryAsset(SLAssetNormal);
            return;
        }
        if (!IsBreak1)
        {
            IsBreak1 = true;
            SetSpriteLibraryAsset(SLAssetBreak1);
            return;
        }
        if (!IsBreak2)
        {
            IsBreak2 = true;
            SetSpriteLibraryAsset(SLAssetBreak2);
            return;
        }
    }

    public void Repel(Vector3 sourcePosition, bool isHeavyAttack = false)
    {
        if (input.IsDashing) { return; }
        IsSystem = true;
        Vector3 _vector = CheckRelativeVector(sourcePosition);
        float faceRight = rb.mass * -5;
        if (isHeavyAttack) 
        { 
            faceRight *= 3;
            CheckIsFacingRight(_vector.x > 0);
        }
        _vector = new Vector3(_vector.x, 0, _vector.z).normalized * faceRight;
        _vector = CameraManager.GetDirectionByCamera(_vector);
        SetZeroVelocity();
        rb.AddForce(_vector, ForceMode.Impulse);
        IsSystem = false;
    }
    #endregion

    #region Sex
    public void SearchSexEnemy()
    {
        Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
        foreach (Collider collider in colliders)
        {
            if (collider.tag == "Enemy")
            {
                Enemy enemy = collider.GetComponentInParent<Enemy>();
                sexAnimName = "";
                IsSexing = true;
                StartCoroutine(Resist());
                GameManager.AddSexEnemies(enemy);
                break;
            }
        }
    }

    /// <summary>
    /// ±√§„Sex
    /// </summary>
    /// <returns></returns>
    public IEnumerator Resist()
    {
        int _count = 1;
        float _currentInput;
        float _nextInput = 0;
        while (IsSexing)
        {
            if (MoveInput.x != 0)
            {
                _currentInput = MoveInput.x;
                if (_currentInput == _nextInput)
                {
                    _count++;
                    StartCoroutine(DoShark());
                    if (_count == 10)
                    {
                        IsSexing = false;
                        yield break;
                    }
                }
                _nextInput = _currentInput * -1;
            }
            yield return null;
        }
    }
    #endregion

    public IEnumerator DoShark()
    {
        Vector3 original = transform.position;
        float time = 0.1f;
        while (time > 0)
        {
            time -= Time.deltaTime;
            float offset = Mathf.Sin(Time.time * .5f) * .1f;
            transform.position = original + new Vector3(offset, 0, offset);
            yield return new WaitForSeconds(0.001f);
        }
        transform.position = original;
    }

    public void ItemAction(int _index)
    {
        Inventory inventory = UI_Shortcut.Instance.GetSlotItemData(_index);
        if (inventory == null || inventory.amount == 0) return;

        ItemData item = inventory.item;
        if (item.name == "HPPotion")
        {
            PlaySFXTrigger(2);
            Hurt(-MaxHp * item.effectsVaule);
        }
        InventoryManager.SaveInventory(item, -1);
    }

    public IEnumerator DemoSword3()
    {
        IsSystem = true;
        input.inputHandle.Character.Disable();
        input.MoveInput = Vector2.zero;
        yield return new WaitForSeconds(0.5f);
        input.SetAttacking(true);
        yield return new WaitForSeconds(0.5f);
        input.SetAttacking(true);
        yield return new WaitForSeconds(0.5f);
        input.SetAttacking(true);
        yield return new WaitForSeconds(1f);
        input.inputHandle.Character.Enable();
        IsSystem = false;
    }

    public IEnumerator DoStartAnimation()
    {
        IsSystem = true;
        input.inputHandle.Character.Disable();
        input.inputHandle.SexAction.Disable();
        while (true)
        {
            float index = Random.Range(1, 5);
            if (index < 3)
            {
                if (index == 1)
                    testSexAnim = "OrcForeplay01"; 
                else if (index == 2)
                    testSexAnim = "OrcForeplay03"; 
                SetSpriteLibraryAsset(SLAssetBreak1);
            }
            else 
            {
                if (index == 3)
                    testSexAnim = "OrcSex01";
                else if (index == 4)
                    testSexAnim = "OrcSex04";
                SetSpriteLibraryAsset(SLAssetBreak2);
            }
            anim.Play(testSexAnim);
            yield return new WaitForSeconds(5f);
        }
    }
}
