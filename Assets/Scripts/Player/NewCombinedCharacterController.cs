using UnityEngine;
using UnityEngine.InputSystem.Controls;
using System;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using Circle;
using UnityEngine.ProBuilder;
using UnityEngine.Events;

[RequireComponent(typeof(Rigidbody))]
public class NewCombinedCharacterController : MonoBehaviour
{
    #region Configurable Fields

    [Header("Speed/Physics Controls")] 
    [Range(0f, 45f)] [Tooltip("Max speeds of each ability.")]
    [SerializeField] private float maxSpeed = 10f;

    [Range(0f, 5f)] [Tooltip("Max acceleration while walking.")]
    [SerializeField] private float maxAcceleration = 1f;

    [Range(0f, 5f)] [Tooltip("Max acceleration while in the air.")]
    [SerializeField] private float maxAirAcceleration = 0.1f;

    [Range(0f, 10f)] [Tooltip("Max jump height.")]
    [SerializeField] private float jumpHeight = 2f;

    [Range(0, 5)] [Tooltip("Number of 'double-jumps'.")]
    [SerializeField] private int airJumps;

    [Range(-1, 10)] [Tooltip("Friction for the Player on the Ground'.")]
    [SerializeField] private float frictionSpeed = 5f;

    [Tooltip("Turn on if you want the Player to stop on a dime when moving on the ground")]
    [SerializeField] private bool preciseMovement;

    [Space]
    [Header("Ramp Controls")] 
    [Range(0, 90)] [Tooltip("Angle at which the player begins to slide down the slope.")]
    [SerializeField] private float maxSlopeAngle = 45f;

    [Range(0f, 100f)] [Tooltip("How tight to keep the player to the surface.")]
    [SerializeField] private float maxSnapSpeed = 15f;

    [Min(0f)] [Tooltip("Adjust based on height of player to keep player touching the surface.")]
    [SerializeField] private float snapToGroundProbeDistance = 2f;

    [Space]
    [Header("Unity Events")]
    [SerializeField] private UnityEvent onFlipDirection;

    #endregion

    /* 
     * Note that while it says private fields, this doesn't necessarily mean the fields are all private access, only that
     * they aren't available in the editor (so public properties are fair game).
     */
    #region Private Fields

    private float snapForce = 5;

    private readonly LayerMask probeMask = -1;
    private Rigidbody body;

    private Vector3 contactNormal, steepNormal;

    private Direction direction = Direction.Right;

    // The current direction of gravity
    private float gravityDirection;
    private int groundContactCount, steepContactCount;

    private int jumpPhase;

    private float minGroundDot;
    private Vector3 movementInput;


    private bool setGroundedOverride;
    private int stepsSinceLastGrounded, stepsSinceLastJump;
    private Quaternion to = Quaternion.identity;
    private bool OnGround => groundContactCount > 0;
    //private bool OnSteep => steepContactCount > 0;

    // A property to be overrided in other classes if they don't want the grounded method to fire
    protected internal bool OverrideOnGround { private get; set; }

    private float airAcceleration;

    private InputAction horizontalAction;
    private InputAction jumpAction;

    private PlayerManager manager;

    #endregion

    #region Unity Callbacks

    private void Awake()
    {
        manager = FindObjectOfType<PlayerManager>();
        horizontalAction = InputHandler.GetAction("Horizontal");
        jumpAction = InputHandler.GetAction("Jump");
    }

    private void OnEnable()
    {
        if (jumpAction != null) jumpAction.performed += Jump;

        GameManager.onCutsceneEnter += Freeze;
        manager.onDeath += Freeze;
        manager.onHit += Freeze;
    }

    private void OnDisable()
    {
        if (jumpAction != null) jumpAction.performed -= Jump;

        GameManager.onCutsceneEnter -= Freeze;
        manager.onDeath -= Freeze;
        manager.onHit -= Freeze;
    }

    private void Start()
    {
        body = GetComponent<Rigidbody>();
        to = Quaternion.Euler(0, 0, 180);
        airAcceleration = maxAirAcceleration;

        /*
         * I wasn't sure what was going on here with this calculation but it's just a dot product between the up vector
         * and the normal of a slope at maxSlopeAngle (the magnitude of those vectors will be one since they are normalized,
         * so the equation gets simplified to just the cosine of the maxSlopeAngle)
         */
        minGroundDot = Mathf.Cos(maxSlopeAngle * Mathf.Deg2Rad);
        Vector3 test = new Vector3(-1, 1, 0).normalized;
        //Debug.Log($"minGroundDotProduct: {minGroundDot}, dot between {maxSlopeAngle} vector and up vector: {Vector3.Dot(test, Vector3.up)}");
        //Debug.Log($"Angle between test vector {test} and up vector based on physics {-Physics.gravity.normalized}: {Vector3.Angle(test, -Physics.gravity)};");
    }

    private void Update()
    {
        // Get horizontal and vertical inputs
        movementInput.x = horizontalAction.ReadValue<float>();
        //movementInput.z = verticalAction.ReadValue<float>();

        // Useful for when things need to happen when the player flips
        // Might be able to use the cancelled event for this though? 
        switch (direction)
        {
            case Direction.Left:
                if (movementInput.x > 0)
                {
                    direction = Direction.Right;
                    FlipDirection();
                }
                break;
            case Direction.Right:
                if (movementInput.x < 0)
                {
                    direction = Direction.Left;
                    FlipDirection();
                }
                break;
        }

        //RaycastHit hit;
        //if (Physics.Raycast(transform.position, Vector3.right, out hit, 0.65f) || Physics.Raycast(transform.position, Vector3.left, out hit, 0.65f))
        //{
        //    //if(!hit.collider.gameObject.CompareTag("WallJump"))
        //    //{
        //    //    maxAirAcceleration = 0.03f;
        //    //}
        //}
        //else
        //{
        maxAirAcceleration = airAcceleration;
        //}
    }

    private void FixedUpdate()
    {
        if (!GameManager.IsScripted && manager.Health > 0)
        {
            UpdateState();
            MovePlayer();
            // Get the current direction of gravity
            gravityDirection = Mathf.Sign(Physics.gravity.y);

            // setGroundedOverride = false;

            Vector3 velocity = body.velocity;
            Vector3 scaledVelocity = Vector3.ClampMagnitude(new Vector3(velocity.x, 0, velocity.z), maxSpeed) + Vector3.ClampMagnitude(new Vector3(0, velocity.y, 0), maxSpeed * 2);
            //scaledVelocity.y = body.velocity.y;
            if (setGroundedOverride) scaledVelocity.y = 0;

            body.velocity = scaledVelocity;

            ClearState();

            Debug.DrawRay(transform.position, Physics.gravity);
            body.AddForce(Physics.gravity); // Todo: shouldn't need this call
        }
    }

    private void OnCollisionEnter(Collision collision) => EvaluateCollision(collision);

    private void OnCollisionStay(Collision collision) => EvaluateCollision(collision);

    #endregion

    private void Jump(InputAction.CallbackContext context)
    {
        Vector3 jumpDirection;
        if (OnGround)
            jumpDirection = contactNormal;
        /*        else if (OnSteep)
                {
                    jumpDirection = steepNormal;
                    jumpPhase = 0;
                }*/
        else if (airJumps > 0 && jumpPhase <= airJumps)
        {
            if (jumpPhase == 0) jumpPhase = 1;

            jumpDirection = contactNormal;
        }
        else
            return;

        stepsSinceLastJump = 0;
        jumpPhase += 1;

        float jumpSpeed = Mathf.Sqrt(Mathf.Abs(-2f * Physics.gravity.y) * jumpHeight * 2);
        jumpDirection = (jumpDirection + Vector3.up).normalized;
        float alignedSpeed = Vector3.Dot(body.velocity, jumpDirection);
        if (alignedSpeed > 0f) jumpSpeed = Mathf.Max(jumpSpeed - alignedSpeed, 0f);

        body.AddForce(transform.up * jumpSpeed, ForceMode.VelocityChange);
    }

    // Handles flipping the player model
    private void FlipDirection()
    {
        // Invoke event for other observers
        onFlipDirection?.Invoke();

        Quaternion rot = transform.rotation;
        to = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y + 180,
            rot.eulerAngles.z);
        body.MoveRotation(to);
    }

    // Does all the stuff that the character controller component would do (i.e. ground snapping, checking the
    // slope of the ground, setting the contact normal, etc.)
    private void UpdateState()
    {
        stepsSinceLastGrounded += 1;
        stepsSinceLastJump += 1;
        if (OnGround || SnapToGround() || CheckSteepContacts())
        {
            stepsSinceLastGrounded = 0;
            if (stepsSinceLastJump > 1) jumpPhase = 0;

            if (groundContactCount > 1) contactNormal.Normalize();
        }
        else
            contactNormal = Vector3.up;
    }

    // Removes all ground contacts, steeps, etc.
    private void ClearState()
    {
        groundContactCount = steepContactCount = 0;
        contactNormal = steepNormal = Vector3.zero;
    }

    // Checks if the player should snap to the ground and if so does
    private bool SnapToGround()
    {
        /*
         * This section is just checks to see if the player should be snapping to the ground, which includes
         * checks to the global override, checks about the steps since last grounded and last jumping, 
         */
        // here's the issue for jetpack with slopes
        if (OverrideOnGround) return false;

        // Check... something lol
        if (stepsSinceLastGrounded > 1 || stepsSinceLastJump <= 2) return false;

        // Check if speed is greater than maxSnapSpeed?
        float speed = body.velocity.magnitude;
        if (speed > maxSnapSpeed) return false;

        // Check if there's ground below the player
        if (!Physics.Raycast(
                body.position, Vector3.down, out RaycastHit hit,
                snapToGroundProbeDistance * -gravityDirection, probeMask
            ))
            return false;

        // Check if that ground is level (enough)
        if (hit.normal.y < minGroundDot) return false;

        groundContactCount = 1;
        contactNormal = hit.normal;
        // Do a dot with the hit normal and body velocity, if that value is 0 greater than zero do some snapping.
        float dot = Vector3.Dot(body.velocity, hit.normal);
        if (dot > 0f) body.AddForce(Physics.gravity * snapForce, ForceMode.Acceleration);

        return true;
    }

    private bool CheckSteepContacts()
    {
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            if (steepNormal.y >= minGroundDot)
            {
                steepContactCount = 0;
                groundContactCount = 1;
                contactNormal = steepNormal;
                return true;
            }
        }
        
        return false;
    }

    private void MovePlayer()
    {
        // This is all debug stuff, it could go in a gizmos method or something
        Vector3 xAxis = Vector3.ProjectOnPlane(Vector3.right, contactNormal).normalized;
        Vector3 zAxis = Vector3.ProjectOnPlane(Vector3.forward, contactNormal).normalized;
        Vector3 position = transform.position;
        Debug.DrawRay(position, xAxis * 2, Color.green);
        Debug.DrawRay(position, zAxis * 2, Color.green);

        // The projection values are never used?
        xAxis = Vector3.right;
        zAxis = Vector3.forward;

        /* 
         * Max acceleration and max air acceleration are separated here I assumed to control the vertical and horizontal
         * values separately, but here the horizontal acceleration is being affected if the player isn't on the ground
         */
        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        Vector3 a = (xAxis * movementInput.x + zAxis * movementInput.z) * (acceleration * 2);
        Debug.DrawRay(transform.position, a * 2, Color.red);

        // Adding acceleration to the rigidbody
        body.AddForce(a, ForceMode.VelocityChange);

        /* 
         * Add friction to the body if precise movement is disabled, else we want to set the horizontal velocity to be zero 
         * when the player isn't giving input. We only need to check the x direction for a side scroller.
         */
        if (OnGround)
        {
            if (movementInput.x == 0 && Mathf.Abs(body.velocity.x) > 0.01f)
            {
                if (!preciseMovement) body.AddForce(Vector3.right * (-body.velocity.x * frictionSpeed), ForceMode.Acceleration);
                else
                {
                    body.velocity = new Vector3(0, body.velocity.y, body.velocity.z);
                    body.AddForce(Physics.gravity * snapForce, ForceMode.Acceleration);
                }
            }
        }
    }

    // Modified from the original version, now this is just checking the angle between the two vectors
    private void EvaluateCollision(Collision collision)
    {
        for (var i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;
            float angle = Vector3.Angle(normal, -Physics.gravity);

            // Grounded case
            if (angle <= maxSlopeAngle)
            {
                groundContactCount += 1;
                contactNormal += normal;
            }
            else
            {
                steepContactCount += 1;
                steepNormal += normal;
            }
        }
    }

    /// <summary>
    ///     Tells this character controller to not apply gravity
    ///     for a frame. Useful for wall jumps and grappling hooks.
    /// </summary>
    /// <param name="isGrounded"></param>
    public void SetGrounded(bool isGrounded)
    {
        setGroundedOverride = isGrounded;
    }

    private void Freeze()
    {
        body.velocity = Vector3.zero;
    }

    private enum Direction
    {
        Right,
        Idle,
        Left
    }
}