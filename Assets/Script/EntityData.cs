using UnityEngine;

[CreateAssetMenu(menuName = "AFEI/EntityData")]
public class EntityData : ScriptableObject
{
    [Header("Gravity")]
    [HideInInspector] public float gravityStrength; //Downwards force (gravity) needed for the desired jumpHeight and jumpTimeToApex.
    [HideInInspector] public float gravityScale;

    [Space(5)]
    [Header("Fall")]
    public float fallGravityMult = 10f; //Multiplier to the player's gravityScale when falling.
    public float maxFallSpeed = 20f; //Maximum fall speed (terminal velocity) of the player when falling.
    [Space(5)]
    [Header("Falling and Input Down")]
    public float fastFallGravityMult = 10f; //Larger multiplier to the player's gravityScale when they are falling and a downwards input is pressed.
                                      //Seen in games such as Celeste, lets the player fall extra fast if they wish.
    public float maxFastFallSpeed = 20f; //Maximum fall speed(terminal velocity) of the player when performing a faster fall.
    
    [Space(20)]
    [Header("Run")]
    public float runMaxSpeed = 5f; //Target speed we want the player to reach.
    public float runAcceleration = 4f; //The speed at which our player accelerates to max speed, can be set to runMaxSpeed for instant acceleration down to 0 for none at all
    [HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
    public float runDecceleration = 4f; //The speed at which our player decelerates from their current speed, can be set to runMaxSpeed for instant deceleration down to 0 for none at all
    [HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
    
    [Space(5)]
    [Range(0f, 1)] public float accelInAir = 1f; //Multipliers applied to acceleration rate when airborne.
    [Range(0f, 1)] public float deccelInAir = 1f;

    [Space(20)]
    [Header("Jump")]
    public float jumpHeight = 10f; //Height of the player's jump
    public float jumpTimeToApex = 0.5f; //Time between applying the jump force and reaching the desired jump height. These values also control the player's gravity and jump force.
    [HideInInspector] public float jumpForce; //The actual force applied (upwards) to the player when they jump.

    [Header("Both Jumps")]
    public float jumpCutGravityMult = 10f; //Multiplier to increase gravity if the player releases thje jump button while still jumping
    [Range(0f, 1)] public float jumpHangGravityMult = 1f; //Reduces gravity while close to the apex (desired max height) of the jump
    public float jumpHangTimeThreshold = 0.5f; //Speeds (close to 0) where the player will experience extra "jump hang". The player's velocity.y is closest to 0 at the jump's apex (think of the gradient of a parabola or quadratic function)
    [Space(0.5f)]
    public float jumpHangAccelerationMult = 0;
    public float jumpHangMaxSpeedMult = 0;

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

    //Unity Callback, called when the inspector updates
    private void OnValidate()
    {
        //Calculate gravity strength using the formula (gravity = 2 * jumpHeight / timeToJumpApex^2) 
        gravityStrength = -(2 * jumpHeight) / (jumpTimeToApex * jumpTimeToApex);

        //Calculate the rigidbody's gravity scale (ie: gravity strength relative to unity's gravity value, see project settings/Physics2D)
        gravityScale = gravityStrength / Physics2D.gravity.y;

        //Calculate are run acceleration & deceleration forces using formula: amount = ((1 / Time.fixedDeltaTime) * acceleration) / runMaxSpeed
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        //Calculate jumpForce using the formula (initialJumpVelocity = gravity * timeToJumpApex)
        jumpForce = Mathf.Abs(gravityStrength) * jumpTimeToApex;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        #endregion
    }
}
