using UnityEngine;
using UnityEngine.InputSystem.Controls;
using System;

[RequireComponent(typeof(Rigidbody))]
public class CombinedCharacterController : MonoBehaviour
{
    [Header("Platformer or Top Down Mechanics?")]
    [Tooltip(
        "If true, act as a front-facing controller locked in the XY plane. If false, act as a top-down controller.")]
    public bool lockToXY;

    [Header("Speed and physics controls:")] [Range(0f, 45f)] [Tooltip("Max speeds of each ability.")]
    public float maxSpeed = 10f;


    [Range(0f, 5f)] [Tooltip("Max acceleration while walking.")]
    public float maxAcceleration = 1f;

    [Range(0f, 5f)] [Tooltip("Max acceleration while in the air.")]
    public float maxAirAcceleration = 1f;

    [Range(0f, 10f)] [Tooltip("Max jump height.")]
    public float jumpHeight = 2f;

    [Range(0, 5)] [Tooltip("Number of 'double-jumps'.")]
    public int airJumps;

    [Range(-1, 10)] [Tooltip("Friction for the Player on the Ground'.")]
    public float frictionSpeed;

    [Tooltip("Turn on if you want the Player to stop on a dime when moving on the ground")]
    public bool preciseMovement;

    /*    [Header("Ramp controls:")] [Range(0, 90)] [Tooltip("Angle at which the player begins to slide down the slope.")]*/
    private float maxSlopeAngle = 50f;


    /*    [Range(0f, 100f)] [Tooltip("How tight to keep the player to the surface.")]*/
    private float maxSnapSpeed = 15f;

    /*    [Min(0f)] [Tooltip("Adjust based on height of player to keep player touching the surface.")]*/
    private float snapToGroundProbeDistance = 2f;

    private float snapForce = 5;

    private readonly LayerMask probeMask = -1;
    private readonly float rotationSpeed = 720f;
    private Rigidbody body;

    private Camera cam;
    private Vector3 contactNormal, steepNormal;
    private bool desiredJump;

    private Direction direction = Direction.Right;

    // The current direction of gravity
    private float gravityDirection;
    private int groundContactCount, steepContactCount;

    private int jumpPhase;

    private Vector3 lookAt;

    private float minGroundDotProduct;
    private Vector3 playerInput;


    private bool setGroundedOverride;
    private int stepsSinceLastGrounded, stepsSinceLastJump;
    private Quaternion to = Quaternion.identity;
    private bool OnGround => groundContactCount > 0;
    private bool OnSteep => steepContactCount > 0;

    protected internal bool OverrideOnGround { private get; set; }

    private float airAcceleration;

    //private InputControls controls;

    public ButtonControl jumpButton;


    // Start is called before the first frame update
    private void Start()
    {
        cam = Camera.main;
        body = GetComponent<Rigidbody>();
        to = Quaternion.Euler(0, 0, 180);
        airAcceleration = maxAirAcceleration;

        if (lockToXY)
        {
            Quaternion rot = transform.rotation;
            transform.rotation = Quaternion.Euler(rot.eulerAngles.x, rot.eulerAngles.y + 90,
                rot.eulerAngles.z);
        }
        //controls = new InputControls();
        //controls.SideScrollerMechanics.MoveHort.Enable();
        //controls.SideScrollerMechanics.MoveVert.Enable();
        //controls.SideScrollerMechanics.Jump.Enable();
        //jumpButton = (ButtonControl)controls.SideScrollerMechanics.Jump.controls[0];
    }

    // Stay on ground stuff
    private void Update()
    {
        /*
         * I wasn't sure what was going on here with this calculation but it's just a dot product between the up vector
         * and the normal of a slope at maxSlopeAngle (the magnitude of those vectors will be one since they are normalized,
         * so the equation gets simplified to just the cosine of the maxSlopeAngle)
         */
        // Todo: remove for performance later
        minGroundDotProduct = Mathf.Cos(maxSlopeAngle * Mathf.Deg2Rad);

        //playerInput.x = controls.SideScrollerMechanics.MoveHort.ReadValue<float>();
        //playerInput.z = controls.SideScrollerMechanics.MoveVert.ReadValue<float>();
        playerInput = Vector3.ClampMagnitude(playerInput, 1f);

        // Is the player operating in "Platform" mode?
        if (lockToXY)
        {
            playerInput.z = 0;
            switch (direction)
            {
                case Direction.Left:
                    if (playerInput.x > 0)
                    {
                        direction = Direction.Right;
                        FlipDirection();
                    }

                    break;
                case Direction.Right:
                    if (playerInput.x < 0)
                    {
                        direction = Direction.Left;
                        FlipDirection();
                    }

                    break;
            }
        }

        // Checking for walls on the sides so that wall jump can be activated
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.right, out hit, 0.65f) || Physics.Raycast(transform.position, Vector3.left, out hit, 0.65f))
        {
            if(!hit.collider.gameObject.CompareTag("WallJump"))
            {
                maxAirAcceleration = 0.03f;
            }
        }
        else
        {
            maxAirAcceleration = airAcceleration;
        }

        // Getting jump input for FixedUpdate to handle, will be replaced with an event call
        desiredJump |= jumpButton.isPressed;
    }


    private void FixedUpdate()
    {
        // TODO: Remove this state logic, I'm not sure what it's doing but there's probably a simpler way
        UpdateState();
        MovePlayer();
        // Get the current direction of gravity (not necessary)
        gravityDirection = Mathf.Sign(Physics.gravity.y);

        // TODO: Remove this and have a jump event instead
        if (desiredJump)
        {
            desiredJump = false;
            Jump();
        }


        // setGroundedOverride = false;

        // Calculating velocity with speeds and overrides accounted for
        Vector3 velocity = body.velocity;
        Vector3 scaledVelocity = Vector3.ClampMagnitude(new Vector3(velocity.x, 0, velocity.z), maxSpeed);
        scaledVelocity.y = body.velocity.y;
        if (setGroundedOverride) scaledVelocity.y = 0;

        body.velocity = scaledVelocity;


        // Control player rotation when operating in "Top-Down" mode
        if (!lockToXY) RotateTowardsMouse();

        // TODO: should also be replaced
        ClearState();

        Debug.DrawRay(transform.position, Physics.gravity);
        body.AddForce(Physics.gravity); // Todo: shouldn't need this call
    }

    private void OnCollisionEnter(Collision collision) => EvaluateCollision(collision);

    private void OnCollisionStay(Collision collision) => EvaluateCollision(collision);


    /// <summary>
    ///     Tells this character controller to not apply gravity
    ///     for a frame. Useful for wall jumps and grappling hooks.
    /// </summary>
    /// <param name="isGrounded"></param>
    public void SetGrounded(bool isGrounded)
    {
        setGroundedOverride = isGrounded;
    }

    // For the topdown style, this will probably go in its own aiming component
    private void RotateTowardsMouse()
    {
        // Get rotation from mouse position
        Vector3 p = Input.mousePosition;
        p.z = 20;
        Ray ray = cam.ScreenPointToRay(p);

        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            lookAt = hit.point;
            lookAt.y = 0;
        }

        Vector3 movementDirection = lookAt - transform.position;
        movementDirection.y = 0;
        movementDirection.Normalize();

        if (movementDirection != Vector3.zero)
        {
            Quaternion toRotation = Quaternion.LookRotation(movementDirection, Vector3.up);
            Quaternion rot = Quaternion.RotateTowards(transform.rotation, toRotation, rotationSpeed * Time.deltaTime);
            body.MoveRotation(rot);
        }
    }

    // Removes all ground contacts, steeps, etc.
    private void ClearState()
    {
        groundContactCount = steepContactCount = 0;
        contactNormal = steepNormal = Vector3.zero;
    }

    // Handles flipping the player model
    private void FlipDirection()
    {
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

        // Check if the player is on the ground, if there is any ground snapping, or if there are any steep contacts
        if (OnGround || SnapToGround() || CheckSteepContacts())
        {
            stepsSinceLastGrounded = 0;
            if (stepsSinceLastJump > 1) jumpPhase = 0;

            if (groundContactCount > 1) contactNormal.Normalize();
        }
        else
            contactNormal = Vector3.up;
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
        if (hit.normal.y < GetMinDot(hit.collider.gameObject.layer)) return false;

        // Do a dot with the hit normal and body velocity, if that value is 0 greater than zero do some snapping.
        groundContactCount = 1;
        contactNormal = hit.normal;
        float dot = Vector3.Dot(body.velocity, hit.normal);
        if (dot > 0f) body.AddForce(Physics.gravity * snapForce, ForceMode.Acceleration);

        return true;
    }

    private bool CheckSteepContacts()
    {
        if (steepContactCount > 1)
        {
            steepNormal.Normalize();
            if (steepNormal.y >= minGroundDotProduct)
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
        Vector3 xAxis = ProjectOnContactPlane(Vector3.right).normalized;
        Vector3 zAxis = ProjectOnContactPlane(Vector3.forward).normalized;
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
        // print(xAxis);
        float acceleration = OnGround ? maxAcceleration : maxAirAcceleration;
        Vector3 a = (xAxis * playerInput.x + zAxis * playerInput.z) * (acceleration * 2);
        Debug.DrawRay(transform.position, a * 2, Color.red);

        // Adding acceleration to the rigidbody
        body.AddForce(a, ForceMode.VelocityChange);

        /* 
         * Add friction to the body if precise movement is disabled, else we want to set the horizontal velocity to be zero 
         * when the player isn't giving input. We only need to check the x direction for a side scroller.
         */
        if (OnGround)
        {
            if (playerInput.x == 0 && Mathf.Abs(body.velocity.x) > 0.01f)
            {
                if (!preciseMovement) body.AddForce(Vector3.right * (-body.velocity.x * frictionSpeed), ForceMode.Acceleration);
                else
                {
                    body.velocity = new Vector3(0, body.velocity.y, body.velocity.z);
                    body.AddForce(Physics.gravity * snapForce, ForceMode.Acceleration);
                }
            }

            if (playerInput.z == 0 && Mathf.Abs(body.velocity.z) > 0.01f)
            {
                if (!preciseMovement) body.AddForce(Vector3.forward * (-body.velocity.z * frictionSpeed), ForceMode.Acceleration);
                else
                {
                    body.velocity = new Vector3(body.velocity.x, body.velocity.y, 0);
                    body.AddForce(Physics.gravity * snapForce, ForceMode.Acceleration);
                }
            }
        }
    }

    private void Jump()
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

    /*
     * There is a check going on here for the collisions. Basically, in the beginning of the game a variable called
     * minGroundDot is calculated, which is the dot product of a slope with maxSlopeAngle. When comparing the 
     * y value of the hit normal with the minGroundDot, any value less than minGroundDot is from an angle that is
     * too steep and will be ignored. Because the gravity can flip there is an if case checking the gravity 
     * direction and flipping the numbers.
     * 
     * It might be easier to find the angle between the vectors? It's probably less efficient but it might work.
     */
    private void EvaluateCollision(Collision collision)
    {
        float minDot = GetMinDot(collision.gameObject.layer);
        for (var i = 0; i < collision.contactCount; i++)
        {
            Vector3 normal = collision.GetContact(i).normal;

            if (gravityDirection > 0)
            {
                if (normal.y <= -minDot)
                {
                    groundContactCount += 1;
                    contactNormal += normal;
                }
                else if (normal.y > -0.01f)
                {
                    steepContactCount += 1;
                    steepNormal += normal;
                }
            }
            else
            {
                if (normal.y >= minDot)
                {
                    groundContactCount += 1;
                    contactNormal += normal;
                }
                else if (normal.y > -0.01f)
                {
                    steepContactCount += 1;
                    steepNormal += normal;
                }
            }
        }
    }

    // Could just use Vector3.ProjectOnPlane instead?
    private Vector3 ProjectOnContactPlane(Vector3 vector) =>
        vector - contactNormal * Vector3.Dot(vector, contactNormal);

    // Idk why layer is a parameter here, this is just functionally a getter
    private float GetMinDot(int layer) => minGroundDotProduct;

    private enum Direction
    {
        Right,
        Idle,
        Left
    }
}