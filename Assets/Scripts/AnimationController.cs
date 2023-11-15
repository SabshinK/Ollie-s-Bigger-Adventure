using Circle;
using System.Collections;
using System.Diagnostics.CodeAnalysis;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.InputSystem;

public class AnimationController : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator animator; // Declare a variable to hold the Animator component
    [SerializeField] private ParticleSystem particles;
    [SerializeField] private SpriteRenderer sp;

    [Space]
    [SerializeField] private float flipParticleDuration = 0.5f;

    private CombinedCharacterController cc;

    private InputAction gravityAction;

    private void Awake()
    {
        // Get the Animator component attached to this GameObject
        cc = GetComponentInParent<CombinedCharacterController>();
        //gravityAction = InputHandler.GetAction("Toggle Gravity");
    }

    private void OnEnable()
    {
        //gravityAction.performed += FlipGravity;
    }

    private void OnDisable()
    {
        //gravityAction.performed -= FlipGravity;
    }

    private void Update()
    {
        // Old version of animation
        //// Check if the A key is pressed down
        //if (Input.GetKeyDown(KeyCode.A))
        //{
        //    // Set the Animator parameter to play the moving left animation
        //    animator.SetBool("isMoving", true);
        //}
        //else if (Input.GetKeyUp(KeyCode.A))
        //{
        //    // Stop the moving left animation when the A key is released
        //    animator.SetBool("isMoving", false);
        //}

        //// Check if the D key is pressed down
        //if (Input.GetKeyDown(KeyCode.D))
        //{
        //    // Set the Animator parameter to play the moving right animation
        //    animator.SetBool("isMoving", true);
        //}
        //else if (Input.GetKeyUp(KeyCode.D))
        //{
        //    // Stop the moving right animation when the D key is released
        //    animator.SetBool("isMoving", false);
        //}

        animator?.SetFloat("Speed", cc.MovementInput.magnitude);
    }

    private void FlipGravity(InputAction.CallbackContext context)
    {
        sp.flipY = !sp.flipY;
    }

    // Should be subscribed to character controller
    public void FlipDirection()
    {
        //sp.flipX = !sp.flipX;
        StartCoroutine(PlayForSeconds(flipParticleDuration));
    }

    public IEnumerator PlayForSeconds(float seconds)
    {
        particles?.Play();

        yield return new WaitForSeconds(seconds);

        particles?.Stop();
    }
}
