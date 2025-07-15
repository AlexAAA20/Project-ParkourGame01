using System;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // Very small parkour code B)
    // Required options on the player piece: (- requried, ~ reccomended)
    // - Rigidbody2D
    // ~ 0.5 0.7 1


    // 'Micro'class that is used for optimized movement.
    /// <summary>
    /// Used for handling movement complexity.
    /// Complicated.
    /// </summary>
    [Serializable]
    public class MovementGizmo
    {
        [Header("Stats")]
        [Tooltip("Acceleration - Increases the speed gained on a key input.")]
        public float acceleration;  // speEEEeed
        [Tooltip("Deceleration - Decreases the speed by a constant, heading towards the 0 mark.")]
        public float deceleration;  // i am evil speed
        [Tooltip("Deceleration On Idle - Decreases the speed towards 0, but only if it isn't accelerating.")]
        public float decelerationOnIdle;  // evil speed when no speEEEeed
        [Tooltip("Max Speed - Caps out the maximal physical speed.")]
        public float maxSpeed;  // i am fun police but also the healthcare unit
        [Tooltip("Redirect Minimal Speed - The minimal speed that is bankable for a redirect.")]
        public float redirectMinimalSpeed;  // fun police again
        [Tooltip("'Memory' Loss - How fast does the quick turnaround decay? Only with Redirect.")]
        public float memoryLoss;  // i have dementia and weight gain

        [Header("Flags")]
        // i feel like the tooltips explain it already
        [Tooltip("Limit Max Speed - If you cap the maximal speed.")]
        public bool limitMaxSpeed;
        [Tooltip("Forcecap Speed - If off, makes it that instead of having speed clamped, it will simply turn off the ability to accelerate any further.")]
        public bool forcecapSpeed;
        [Tooltip("Decelerate - If you actually do decelerate.")]
        public bool decelerate;
        [Tooltip("Different Decelerate - If you use 'Deceleration On Idle' instead of a constant 'Deceleration'.")]
        public bool differentDecelerate;
        [Tooltip("Redirect - Memorize the speed that you stopped moving at and then be " +
            "able to quickly turn into the opposite direction while having the relativly same speed.")]
        public bool redirect;

        // used for keeping track of speEEEeed
        private float _selectedSpeed;
        // oh and also redirection
        private float _memorizedSpeed;

        [Header("Debug Flags")]
        // logs and timber
        [Tooltip("Allow Logging - Quick switch to turn on/off logs.")]
        public bool allowLogging;
        [Tooltip("Acceleration Log - Logs when adding speed.")]
        public bool accelerationLog;
        [Tooltip("Deceleration Log - Logs when losing speed.")]
        public bool decelerationLog;
        [Tooltip("Redirect Log - Logs when redirecting.")]
        public bool redirectLog;
        [Tooltip("Deceleration Rate Log - Logs the difference in speed when losing it.")]
        public bool decelerationRateLog;
        [Tooltip("Frame Log - Logs on start/end of the main function.")]
        public bool frameLog;

        [Header("Debug Readers")]
        // numbers for NERDS
        [Tooltip("Memorized Speed - Speed that will be put when inputting again after stopping.")]
        public float memorizedSpeed;    // redirect speed
        [Tooltip("Desired Speed - Speed that we wanna gain and do not limit, reads from the frame.")]
        public float desiredSpeed;      // speed i wanna
        [Tooltip("Desired Direction - Litteraly just the direction that was fetched previous frame.")]
        public float desiredDirection;  // direction we wanna
        [Tooltip("Deceleration Rate - How much the deceleration stepped from base speed towards 0 in a frame.")]
        public float decelerationRate;  // hstl idk

        // cool function to do the hard work

        /// <summary>
        /// The main (and only) function of the <see cref="MovementGizmo"/> class.
        /// Adds the force based on the things before.
        /// </summary>
        /// <param name="rb">Rigidbody. Used for reading and writing the horizontal speed.</param>
        /// <param name="direction">Direction that is used for inputting. From -1 to 1.</param>
        public void ApplyForce ( Rigidbody2D rb, float direction )
        {            
            float currSpeed = rb.linearVelocityX;
            if ( frameLog && allowLogging ) Debug.Log( $"Begin with {currSpeed} (going {direction})" );

            if (redirect)
            {
                memorizedSpeed = _memorizedSpeed;

                if ( ((direction != desiredDirection &&
                    desiredDirection != 0) || 
                    (direction + desiredDirection == 0)) &&
                    Mathf.Abs( currSpeed ) > redirectMinimalSpeed )  
                {
                    _memorizedSpeed = Mathf.Abs( currSpeed );
                }
                if ( direction != desiredDirection &&
                    desiredDirection == 0 &&
                    _memorizedSpeed != 0 )
                {
                    currSpeed = _memorizedSpeed * direction;
                    if ( allowLogging && redirectLog ) Debug.Log( $"Redirected with power of {_memorizedSpeed} facing {direction} ({currSpeed})" );
                    _memorizedSpeed = 0;
                }
                if ( _memorizedSpeed != 0 )
                {
                    _memorizedSpeed = Mathf.MoveTowards( _memorizedSpeed, 0, memoryLoss );
                    if ( allowLogging && redirectLog && _memorizedSpeed == 0 ) Debug.Log( $"Redirection fully decayed." );
                }

            }

            desiredDirection = direction;

            float x = _selectedSpeed;
            if (decelerationLog && allowLogging) Debug.Log( $"Decelerating by -{deceleration} till 0 ({currSpeed})" );
            _selectedSpeed = Mathf.MoveTowards( currSpeed, 0, decelerate ? (differentDecelerate && direction == 0 ? decelerationOnIdle : deceleration) : 0 );
            
            decelerationRate = x - _selectedSpeed;
            if ( decelerationRateLog && allowLogging ) Debug.Log( $"Now {currSpeed} (changed by {decelerationRate})" );

            bool overshot = Mathf.Abs( currSpeed ) > maxSpeed && limitMaxSpeed;

            if ( !(overshot && Mathf.Abs(_selectedSpeed + direction * acceleration) > maxSpeed) )
            {
                _selectedSpeed += direction * acceleration;
                if ( accelerationLog && allowLogging ) Debug.Log( $"Applied +{acceleration} velocity ({currSpeed})" );
            }

            desiredSpeed = _selectedSpeed;
            
            currSpeed = _selectedSpeed;
            if ( overshot && forcecapSpeed ) currSpeed = Mathf.Clamp( currSpeed, -maxSpeed, maxSpeed );
            if ( frameLog && allowLogging ) Debug.Log( $"Applied {currSpeed}" );
            rb.linearVelocityX = currSpeed;
        }
    }

    [Serializable]
    public class RelativeBoxGizmo
    {
        public Vector3 position;
        public Vector3 size;
    }
    [Header( "Generic Movement" )]

    [Header("Jump Settings")]

    [Tooltip("Jump Strength - Power added upwards on jumping while grounded.")]
    public float jumpStrength = 7f;
    [Tooltip("Jump Speed Boost - Power added upwards is increased by jumpStrength + (linearSpeedX x jumpSpeedBoost).")]
    public float jumpSpeedBoost = 0.2f;
    [Tooltip("Override Jump - Override instead of adding velocity when jumping.")]
    public bool overrideJump = false;

    [Tooltip("Uncrouched Boxcast - Values for... well, you guessed it - isAirborne.")]
    public RelativeBoxGizmo uncrouchedBoxcast;
    [Tooltip("Crouched Boxcast - Values for... well, you guessed it - isAirborne.")]
    public RelativeBoxGizmo crouchedBoxcast;
    [Tooltip("Crouched Box Scale - Size when crouching.")]
    public float crouchedBoxScale = 0.5f;

    [Tooltip("Is Airborne - Honestly, I don't know what to explain here. It's already in the name. It says it in the name." +
        "Just don't mess up instead of 'isGrounded' which doesn't exist by the way.")]
    public bool isAirborne = false;

    [Tooltip("Is Crouching - If you're crouching, true.")]
    public bool isCrouching = false;

    [Tooltip("Stagger Frames - Frames remaining to get out of the stagger. Stagger causes you to act like you're airborne, even if not.")]
    public int staggerFrames = 0;

    [Header( "Side-To-Side Movement" )]

    [Tooltip("Grounded Movement - A big folder of things that relate to movement while 'Is Airborne' is false.")]
    public MovementGizmo groundedMovement;
    [Tooltip("Crouched Movement - Basically if you crouch.")]
    public MovementGizmo crouchedMovement;
    [Tooltip("Airborne Movement - A big config of speed when 'Is Airborne' is true.")]
    public MovementGizmo airborneMovement;


    [Header("Binds")]

    // клав≥ши дл€ ход≥нн€
    [Tooltip("I do not think I have to explain this.")]
    public KeyCode bindWalkLeft = KeyCode.A;
    [Tooltip("I do not think I have to explain this.")]
    public KeyCode bindWalkRight = KeyCode.D;
    [Tooltip("I do not think I have to explain this.")]
    public KeyCode bindCrouch = KeyCode.LeftControl;
    [Tooltip("I do not think I have to explain this.")]
    public KeyCode bindJump = KeyCode.Space;

    [Header("Debug Settings")]

    // трошки складн≥ Gizmos
    [Tooltip("Draw Gizmos - Just a big on/off button for gizmos.")]
    public bool drawGizmos = false;             // загально малювати чи н≥
    [Tooltip("Draw Deceleration Line - Draw a line in the opposite movement direction representing drag. Drawn in a purplish color.")]
    public bool drawDecelerationLine = false;   // зам≥дленн€
    [Tooltip("Draw Speed Line - Draw the actual speed you're going in. Drawn in yellow.")]
    public bool drawSpeedLine = false;          // швидк≥сть (ф≥зична)
    [Tooltip("Draw Input Line - Draw the direction you're moving in. Drawed in slightly cyan.")]
    public bool drawInputLine = false;          // вв≥д гравц€
    [Tooltip("Draw Desired Line - Draw the wanted speed, uncapped. Drawn in red.")]
    public bool drawDesiredLine = false;        // швидк≥сть (бажана)
    [Tooltip("Gizmo Scale - How big are lines relative to the global grid.")]
    public float gizmoScale = 1f;               // швидк≥сть (бажана)
    [Tooltip("Log Jumps - Log every jump, mostly velocity + position.")]
    public bool logJumps = true;
    // TODO: public bool drawOnAirborne = false;         // чи малювати в пов≥тр≥

    [Header("Rendering")]
    
    // texture
    [Tooltip("Texture - Object that will change depending on the situation.")]
    public Transform texture;


    public void Jump ( )
    {
        if ( isAirborne || isCrouching ) return;
        if ( overrideJump )
            rb.linearVelocityY = jumpStrength + Mathf.Abs(rb.linearVelocityX * jumpSpeedBoost);
        else
            rb.linearVelocityY += jumpStrength + rb.linearVelocityX * jumpSpeedBoost;
        if ( logJumps ) { Debug.Log( $"Jumped at {transform.position} with {jumpStrength} base + " +
            $"{Mathf.Abs( rb.linearVelocityX * jumpSpeedBoost )} boost {( jumpSpeedBoost > 0 ? "" : "(none)" )}." ); }
    }

    // rigidbody
    Rigidbody2D rb;

    // collider
    BoxCollider2D coll;

    [HideInInspector]
    public bool isSprinting = false;

    public void Start ( )
    {
        rb = GetComponent<Rigidbody2D>( );
        coll = GetComponent<BoxCollider2D>( );
    }

    public void Update ( )
    {
        MoveHorizontal( ReadHorizontalInput( ) );
        CheckAirborn( );
        if ( staggerFrames > 0 )
        {
            staggerFrames--;
            isAirborne = true;
        }
        texture.GetComponent<SpriteRenderer>( ).color = staggerFrames > 0 ? new Color( 1, 0.6f, 0 ) : new Color(0, 0.8f, 1);
        if ( Input.GetKeyDown( bindCrouch ) || Input.GetKeyUp( bindCrouch ) ) 
        {
            Crouch( );
        }
        if ( Input.GetKeyDown( bindJump ) )
        {
            Jump( );
        }
    }

    public void Crouch( )
    {
        if ( isCrouching )
        {
            Vector3 size = coll.size;
            size.y /= crouchedBoxScale;
            transform.position -= Vector3.down * ( size.y / 2 * crouchedBoxScale );
            coll.size = size;
            isCrouching = false;
        }
        else
        {
            Vector3 size = coll.size;
            size.y *= crouchedBoxScale;
            transform.position += Vector3.down * ( size.y / 2 * crouchedBoxScale ) * (isAirborne ? -1 : 1);
            coll.size = size;
            isCrouching = true;
        }
        texture.localScale = coll.size;
    }

    public void MoveHorizontal( float input )
    {
        MovementGizmo choice = isAirborne ? airborneMovement : groundedMovement;
        if ( isCrouching && !isAirborne )
        {
            choice = crouchedMovement;
        }
        choice.ApplyForce( rb, input );
    }

    public void CheckAirborn( )
    {
        RaycastHit2D hit = Physics2D.BoxCast(
            transform.position + GetIsAirborneDetector( ).position, GetIsAirborneDetector().size, 0, Vector2.zero
            );
        isAirborne = hit.collider == null && hit.collider != coll;
    }

    public void OnDrawGizmos ( )
    {
        if ( drawGizmos )
        {
            if ( Application.isPlaying )
            {
                MovementGizmo usingRightNow = isAirborne ? airborneMovement : groundedMovement;
                if ( drawDecelerationLine )
                {
                    Gizmos.color = new( 0.5f, 0, 1 );
                    Gizmos.DrawLine( transform.position,
                        transform.position - new Vector3( usingRightNow.decelerationRate * gizmoScale, 0 ) );
                }
                if ( drawSpeedLine )
                {
                    Gizmos.color = new( 1, 1, 0 );
                    Gizmos.DrawLine( transform.position + Vector3.down * 0.1f,
                        transform.position + new Vector3( rb.linearVelocityX * gizmoScale, rb.linearVelocityY * gizmoScale - 0.1f ) );
                }
                if ( drawInputLine )
                {
                    Gizmos.color = new( 0, 1, 0.5f );
                    Gizmos.DrawLine( transform.position + Vector3.up * 0.1f,
                        transform.position + new Vector3( usingRightNow.desiredDirection * gizmoScale, 0.1f ) );
                }
                if ( drawDesiredLine )
                {
                    Gizmos.color = new( 1, 0, 0 );
                    Gizmos.DrawLine( transform.position,
                        transform.position + new Vector3( usingRightNow.desiredSpeed * gizmoScale, 0 ) );
                }
            }
            Gizmos.color = isAirborne ? new( 1, 0, 0, 0.5f ) : new( 0, 1, 0, 0.5f );
            Gizmos.DrawCube( transform.position + GetIsAirborneDetector( ).position, GetIsAirborneDetector().size );
        }
    }
    public RelativeBoxGizmo GetIsAirborneDetector ( ) => isCrouching ? crouchedBoxcast : uncrouchedBoxcast;
    public float ReadHorizontalInput ( )
    {
        float dir = 0;
        if ( Input.GetKey( bindWalkLeft ) ) dir = -1;
        if ( Input.GetKey( bindWalkRight ) ) dir = 1;
        return dir;
    }

}
