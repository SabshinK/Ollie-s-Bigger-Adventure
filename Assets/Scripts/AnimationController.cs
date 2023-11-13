using UnityEditor.PackageManager;
using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator; // Declare a variable to hold the Animator component
    private ParticleSystem particles;

    private NewCombinedCharacterController cc;

    private void Awake()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
        cc = GetComponentInParent<NewCombinedCharacterController>();
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

        animator.SetFloat("Speed", cc.MovementInput.magnitude);
    }

    // Should be subscribed to character controller
    public void FlipDirection()
    {
        // Something like this? Basically when the flip direction event happens you can use this function to do particle things
        // Could even call a coroutine
        //particles.play
    }
}
