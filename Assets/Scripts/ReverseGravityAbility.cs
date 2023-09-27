using Circle;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class ReverseGravityAbility : MonoBehaviour
{
    // We will separate out the player object from the model. If the model rotates, we don't necessarily want 
    // player itself to fully rotate, the movement will get messed up.
    [SerializeField] private Transform playerModel;
    [Tooltip("The speed the player model should rotate when reversing gravity.")]
    [SerializeField] private float rotationSmoothing = 0.125f;

    private Quaternion from;
    private Quaternion to;

    //private InputControls controls;
    //private ButtonControl reverseGravityButton;
    private InputAction gravityAction;

    [Tooltip("Name of the Character Controller")]
    public string nameOfCharacterController = "CharacterCapsulePlain";

    // Start is called before the first frame update
    private void Awake()
    {
        Physics.gravity = Vector3.down * 9.8f;
        to = playerModel.rotation;
        from = Quaternion.Euler(playerModel.rotation.eulerAngles.x + 180, playerModel.rotation.eulerAngles.y, playerModel.rotation.eulerAngles.z);

        gravityAction = InputHandler.GetAction("Toggle Gravity");
    }

    private void OnEnable()
    {
        gravityAction.Enable();
        gravityAction.performed += FlipGravity;
    }

    private void OnDisable()
    {
        gravityAction.Disable();
        gravityAction.performed -= FlipGravity;
    }

    private void Update()
    {
        // I'm just going to assume this script is attached to the player, otherwise finding the player GO
        // should probably be done once and cached instead of being done in update, it's a slow function.
        //to = GameObject.Find(nameOfCharacterController).GetComponent<Transform>().rotation;
        //to = transform.rotation;

        // We will use the local rotation because we are changing the root object rotation somewhere else
        if (playerModel.localRotation != to)
            playerModel.localRotation = Quaternion.Slerp(playerModel.localRotation, to, rotationSmoothing);
    }

    private void FlipGravity(InputAction.CallbackContext context)
    {
        // Sets the gravity for the entire scene
        Vector3 gravity = Physics.gravity;
        gravity.y *= -1;
        Physics.gravity = gravity;

        // Cache the old model rotation and calculate the new rotation
        Quaternion temp = to;
        to = from;
        from = temp;
    }

    public void ResetGravity()
    {
        if (Physics.gravity.y > 0)
        {
            FlipGravity(new InputAction.CallbackContext());
            playerModel.localRotation = to;
        }
    }
}