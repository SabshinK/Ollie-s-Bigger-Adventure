using Circle;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using Cinemachine;
using System.Collections;
using System.Security.Cryptography;

public class ReverseGravityAbility : MonoBehaviour
{
    // We will separate out the player object from the model. If the model rotates, we don't necessarily want 
    // player itself to fully rotate, the movement will get messed up.
    [SerializeField] private Transform playerModel;

    [Tooltip("The speed the player model should rotate when reversing gravity.")]
    [SerializeField] private float rotationSmoothing = 1f;

    [Space]
    [Tooltip("Enables whether the camera offset will shift vertically so that there is less wasted screen space.")]
    [SerializeField] private bool offsetCamera;

    [Tooltip("The speed the camera offset should shift when gravity is reversed.")]
    [SerializeField] private float camOffsetSmoothing = 1f;

    public bool IsReversed { get; private set; }

    private CinemachineFramingTransposer cfm;
    private Vector3 fromOffset;
    private Vector3 toOffset;

    private Quaternion from;
    private Quaternion to;

    private const float epsilon = 0.01f;

    private InputAction gravityAction;

    // Start is called before the first frame update
    private void Awake()
    {
        Physics.gravity = Vector3.down * 9.8f;
        to = playerModel.rotation;
        from = Quaternion.Euler(playerModel.rotation.eulerAngles.x + 180, playerModel.rotation.eulerAngles.y, playerModel.rotation.eulerAngles.z);

        gravityAction = InputHandler.GetAction("Toggle Gravity");
        cfm = FindObjectOfType<CinemachineFramingTransposer>();
        
        toOffset = cfm.m_TrackedObjectOffset;
        fromOffset = -toOffset;
    }

    private void OnEnable()
    {
        gravityAction.performed += FlipGravity;
        GameManager.onCutsceneEnter += DisableGravity;
        GameManager.onCutsceneExit += EnableGravity;
    }

    private void OnDisable()
    {
        gravityAction.performed -= FlipGravity;
        GameManager.onCutsceneEnter -= DisableGravity;
        GameManager.onCutsceneExit -= EnableGravity;
    }

    private void Update()
    {
        // I'm just going to assume this script is attached to the player, otherwise finding the player GO
        // should probably be done once and cached instead of being done in update, it's a slow function.
        //to = GameObject.Find(nameOfCharacterController).GetComponent<Transform>().rotation;
        //to = transform.rotation;

        // We will use the local rotation because we are changing the root object rotation somewhere else
        //if (playerModel.localRotation != to)
        //    playerModel.localRotation = Quaternion.Slerp(playerModel.localRotation, to, rotationSmoothing * Time.deltaTime);

        // Same as rotation, mess with the offset for the camera here
        if (offsetCamera && cfm.m_TrackedObjectOffset != toOffset)
        {
            if (Vector2.Distance(cfm.m_TrackedObjectOffset, toOffset) > epsilon)
                cfm.m_TrackedObjectOffset = Vector3.Lerp(cfm.m_TrackedObjectOffset, toOffset, camOffsetSmoothing * Time.deltaTime);
            else
                cfm.m_TrackedObjectOffset = toOffset;
        }
    }

    private void FlipGravity(InputAction.CallbackContext context)
    {
        IsReversed = !IsReversed;

        // Sets the gravity for the entire scene
        Vector3 gravity = Physics.gravity;
        gravity.y *= -1;
        Physics.gravity = gravity;

        // Set Camera offset
        Vector3 tempOffset = toOffset;
        toOffset = fromOffset;
        fromOffset = tempOffset;

        // Cache the old model rotation and calculate the new rotation
        Quaternion tempRotation = to;
        to = from;
        from = tempRotation;
        playerModel.localRotation = to;
    }

    public void ResetGravity()
    {
        if (Physics.gravity.y > 0)
        {
            FlipGravity(new InputAction.CallbackContext());
            playerModel.localRotation = to;
        }
    }

    private void EnableGravity()
    {
        gravityAction.Enable();
    }

    private void DisableGravity()
    {
        gravityAction.Disable();
    }
}