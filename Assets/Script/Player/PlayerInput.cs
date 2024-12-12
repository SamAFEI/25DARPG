using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region Components
    public Player player { get; set; }
    public Rigidbody rb { get; set; }
    public PlayerData data { get; set; }
    public PlayerInteract playerInteract { get; set; }
    public UI_Bag ui_Bag;
    public SettingManager settingManager;
    public InputHandle inputHandle;
    #endregion

    #region Input Parameters
    public Vector3 MoveInput;
    public int StunCount;
    #endregion

    #region Timers
    public float LastPressedJumpTime;
    public float LastPressedDashTime;
    public float LastPressedAttackTime;
    public float LastPressedParryTime;
    public float ResetDashTime;
    #endregion

    #region CONTROL PARAMETERS
    public bool IsPressedAttack { get { return LastPressedAttackTime > 0; } }
    public bool IsPressedParry { get { return LastPressedParryTime > 0; } }
    public bool IsPressedDash { get { return LastPressedDashTime > 0; } }
    public bool IsJumping { get; set; }
    public bool IsDashing { get; set; }
    public bool IsParrying { get; set; }
    public bool IsSuperArmoring { get; set; }
    public bool IsAttacking { get; set; }
    #endregion

    private void Awake()
    {
        InitInputHandle();
    }

    private void Start()
    {
        player = GetComponent<Player>();
        rb = player.rb;
        data = player.Data;
        playerInteract = GetComponent<PlayerInteract>();
    }

    private void Update()
    {
        #region Timers
        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
        LastPressedAttackTime -= Time.deltaTime;
        LastPressedParryTime -= Time.deltaTime;
        ResetDashTime -= Time.deltaTime;
        #endregion

        #region Input Handler

        //TEST Input
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            player.SetSpriteLibraryAsset(player.SLAssetNormal);
            player.IsBreak1 = false;
            player.IsBreak2 = false;
        }
        if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            player.SetSpriteLibraryAsset(player.SLAssetBreak1);
        }
        if (Input.GetKeyDown(KeyCode.Alpha3))
        {
            player.SetSpriteLibraryAsset(player.SLAssetBreak2);
        }
        #endregion

        #region ATTACK CHECKS

        if (CanAttack() && IsPressedAttack)
        {
            SetAttacking(true);
        }

        #endregion

        #region DASH CHECKS
        if (CanDash() && IsPressedDash)
        {
            if (MoveInput.x != 0)
            {
                player.CheckIsFacingRight(MoveInput.x > 0);
            }
            Vector3 targetDirection = CameraManager.GetDirectionByCamera(MoveInput.z, player.FacingDir);
            StartCoroutine(StartDash(targetDirection));
        }
        #endregion

        #region PARRY CHECKS
        if (CanParry() && IsPressedParry)
        {
            SetParrying(true);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (player.CanMovement)
        {
            if (MoveInput.x != 0)
            {
                player.CheckIsFacingRight(MoveInput.x > 0);
            }
            if (MoveInput.x > 0 != player.IsFacingRight)
            {
                //避免轉向滑行
                player.SetZeroVelocity();
            }
            Run(1);
        }
    }

    private void OnDestroy()
    {
        inputHandle.Character.Disable();
        inputHandle.SexAction.Disable();
    }

    #region InputHandle 
    private void InitInputHandle()
    {
        inputHandle = new InputHandle();
        inputHandle.Enable();

        inputHandle.Character.Movement.started += InputMovement;
        inputHandle.Character.Movement.performed += InputMovement;
        inputHandle.Character.Movement.canceled += InputMovement;
        inputHandle.Character.Dash.started += InputDash;
        inputHandle.Character.Attack.started += InputAttack;
        inputHandle.Character.Parry.started += InputParry;
        inputHandle.Character.OpenBag.started += InputOpenBag;
        inputHandle.Character.Interact.started += InputInteract;
        inputHandle.Character.Item00.started += InputItem00;
        inputHandle.Character.Item01.started += InputItem01;
        inputHandle.Character.Setting.started += InputSetting;

        inputHandle.SexAction.ResistHorizontal.performed += InputResistHorizontal;
        inputHandle.Character.Enable();
        inputHandle.SexAction.Disable();
    }
    private void InputMovement(InputAction.CallbackContext _context)
    {
        Vector2 vector = _context.ReadValue<Vector2>();
        MoveInput.x = vector.x;
        MoveInput.z = vector.y;
    }
    private void InputDash(InputAction.CallbackContext _context)
    {
        LastPressedDashTime = data.dashInputBufferTime;
    }
    private void InputAttack(InputAction.CallbackContext _context)
    {
        LastPressedAttackTime = data.attackInputBufferTime;
    }
    private void InputParry(InputAction.CallbackContext _context)
    {
        LastPressedParryTime = data.parryInputBufferTime;
    }
    private void InputInteract(InputAction.CallbackContext _context)
    {
        playerInteract.DoInteract();
    }
    private void InputItem00(InputAction.CallbackContext _context)
    {
        player.ItemAction(0);
    }
    private void InputItem01(InputAction.CallbackContext _context)
    {
        player.ItemAction(1);
    }
    private void InputOpenBag(InputAction.CallbackContext _context)
    {
        OpenBag();
    }
    private void InputSetting(InputAction.CallbackContext _context)
    {
        OpenSetting();
    }

    private void InputResistHorizontal(InputAction.CallbackContext _context)
    {
        float input = _context.ReadValue<float>();
        MoveInput.x = input;
    }
    #endregion

    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        Vector3 targetDirection = CameraManager.GetDirectionByCamera(MoveInput.z * 3f, MoveInput.x);

        float rbSpeed = new Vector3(rb.velocity.x, 0, rb.velocity.z).magnitude;
        //Calculate the direction we want to move in and our desired velocity
        //float targetSpeed = MoveInput.normalized.magnitude * data.runMaxSpeed;
        float targetSpeed = targetDirection.normalized.magnitude * data.runMaxSpeed;
        //We can reduce are control using Lerp() this smooths changes to are direction and speed
        targetSpeed = Mathf.Lerp(rbSpeed, targetSpeed, lerpAmount);

        #region Calculate AccelRate
        float accelRate;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.

        accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? data.runAccelAmount : data.runDeccelAmount;
        #endregion

        #region Add Bonus Jump Apex Acceleration
        //Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
        /*if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(rb.velocity.y) < Data.jumpHangTimeThreshold)
        {
            accelRate *= data.jumpHangAccelerationMult;
            targetSpeed *= data.jumpHangMaxSpeedMult;
        }*/
        #endregion

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        /*if (data.doConserveMomentum && Mathf.Abs(RB.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(RB.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && LastOnGroundTime < 0)
        {
            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }*/
        #endregion
        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - rbSpeed;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        rb.AddForce(movement * targetDirection.normalized, ForceMode.Force);
        /*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/
    }
    #endregion

    #region JUMP METHODS
    private void Jump()
    {
        //Ensures we can't call Jump multiple times from one press
        LastPressedJumpTime = 0;

        #region Perform Jump
        //We increase the force applied if we are falling
        //This means we'll always feel like we jump the same amount 
        //(setting the player's Y velocity to 0 beforehand will likely work the same, but I find this more elegant :D)
        float force = data.jumpForce;
        if (rb.velocity.y < 0)
            force -= rb.velocity.y;

        rb.AddForce(Vector3.up * force, ForceMode.Impulse);
        #endregion
    }
    #endregion

    #region ATTACK METHODS
    public void SetAttacking(bool _isAttacking)
    {
        IsAttacking = _isAttacking;
        player.IsAttacking = IsAttacking;
    }
    #endregion

    #region DASH METHODS
    //Dash Coroutine
    private IEnumerator StartDash(Vector3 dir)
    {
        IsDashing = true;
        rb.velocity = Vector3.zero;
        //Overall this method of dashing aims to mimic Celeste, if you're looking for
        // a more physics-based approach try a method similar to that used in the jump

        //LastOnGroundTime = 0;
        LastPressedDashTime = 0;

        float startTime = Time.time;

        //_dashesLeft--;

        player.SetGravityScale(0);

        //We keep the player's velocity at the dash speed during the "attack" phase (in celeste the first 0.15s)
        while (Time.time - startTime <= data.dashDurationTime)
        {
            rb.velocity = dir.normalized * data.dashSpeed;
            //Pauses the loop until the next frame, creating something of a Update loop. 
            //This is a cleaner implementation opposed to multiple timers and this coroutine approach is actually what is used in Celeste :D
            yield return null;
        }

        startTime = Time.time;

        //Begins the "end" of our dash where we return some control to the player but still limit run acceleration (see Update() and Run())
        player.SetGravityScale(data.gravityScale);

        //Dash over
        IsDashing = false;
        //NextDashTime = data.dashResetTime;
    }

    #endregion

    #region PARRY METHODS
    public void SetParrying(bool value)
    {
        IsParrying = value;
    }
    #endregion

    #region CHECK METHODS

    private bool CanDash()
    {
        return !IsDashing && !IsParrying && !IsAttacking && ResetDashTime < 0 && player.CanMovement;
    }

    private bool CanAttack()
    {
        return !IsAttacking && !IsDashing && !IsParrying;
    }

    private bool CanParry()
    {
        return !IsParrying && !IsDashing;
    }

    #endregion

    #region Shortcut Methods
    private void OpenBag()
    {
        if (ui_Bag != null)
        {
            ui_Bag.OpenUI_Area();
        }
    }
    #endregion

    private void OpenSetting()
    {
        if (settingManager != null)
        {
            settingManager.OpenUI_Area();
        }
    }
}
