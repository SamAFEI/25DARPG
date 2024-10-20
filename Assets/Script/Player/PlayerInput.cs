using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInput : MonoBehaviour
{
    #region Components
    public Player player => GetComponent<Player>();
    public Rigidbody rb => player.rb;
    public PlayerData data => player.Data;
    public PlayerInteract playerInteract => GetComponent<PlayerInteract>();
    public GameObject ui_Bag;
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
    public float LastUpParryTime;
    public float CanCounterTime;
    #endregion

    #region CONTROL PARAMETERS
    public bool IsPressedAttack { get { return LastPressedAttackTime > 0; } }
    public bool IsPressedParry { get { return LastPressedParryTime > 0; } }
    public bool IsUpParry { get { return LastUpParryTime > 0; } }
    public bool IsPressedDash { get { return LastPressedDashTime > 0; } }
    public bool CanCounter { get { return CanCounterTime > 0; } }
    public bool IsDefending { get { return IsParrying && !CanCounter; } }
    public bool IsJumping { get; private set; }
    public bool IsDashing { get; private set; }
    public bool IsParrying { get; private set; }
    public bool IsSuperArmoring { get; private set; }
    public bool IsAttacking { get; private set; }
    #endregion

    private void Awake()
    {
        InitInputHandle(); 
    }

    private void Update()
    {
        #region Timers
        LastPressedJumpTime -= Time.deltaTime;
        LastPressedDashTime -= Time.deltaTime;
        LastPressedAttackTime -= Time.deltaTime;
        LastPressedParryTime -= Time.deltaTime;
        LastUpParryTime -= Time.deltaTime;
        CanCounterTime -= Time.deltaTime;
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
        if (IsParrying && IsUpParry)
        {
            SetParrying(false);
        }
        #endregion
    }

    private void FixedUpdate()
    {
        if (player.CanMovement && !player.IsDied)
        {
            if (MoveInput.x != 0)
            {
                player.CheckIsFacingRight(MoveInput.x > 0);
            }
            Run(1);
        }
    }

    #region InputHandle 
    private void InitInputHandle()
    {
        inputHandle = new InputHandle();
        inputHandle.Enable();

        inputHandle.Character.Movement.performed += InputMovement;
        inputHandle.Character.Movement.canceled += InputMovement;
        inputHandle.Character.Dash.started += InputDash;
        inputHandle.Character.Attack.canceled += InputAttack;
        inputHandle.Character.Defend.started += InputDefend;
        inputHandle.Character.Defend.canceled += InputDefendCancel;
        inputHandle.Character.OpenBag.started += InputOpenBag;
        inputHandle.Character.Interact.started += InputInteract;
        inputHandle.Character.Item00.started += InputItem00;
        inputHandle.Character.Item01.started += InputItem01;

        inputHandle.SexAction.ResistHorizontal.started += InputResistHorizontal;
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
    private void InputDefend(InputAction.CallbackContext _context)
    {
        LastPressedParryTime = data.parryInputBufferTime;
    }
    private void InputDefendCancel(InputAction.CallbackContext _context)
    {
        LastUpParryTime = data.parryInputBufferTime;
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

    private void InputResistHorizontal(InputAction.CallbackContext _context)
    {
        float input = _context.ReadValue<float>();
        MoveInput.x = input;
    }
    #endregion

    #region RUN METHODS
    private void Run(float lerpAmount)
    {
        Vector3 targetDirection = CameraManager.GetDirectionByCamera(MoveInput.z, MoveInput.x);

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
        if (value)
        {
            CanCounterTime = data.canCounterTime;
        }
        else
        {
            CanCounterTime = 0;
        }
    }
    #endregion

    #region CHECK METHODS

    private bool CanDash()
    {
        return !IsDashing && !IsParrying;
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
            ui_Bag.SetActive(!ui_Bag.activeSelf);
        }
    }
    #endregion
}
