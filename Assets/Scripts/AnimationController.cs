using UnityEngine;

public class AnimationController : MonoBehaviour
{
    private Animator animator; // Declare a variable to hold the Animator component

    void Start()
    {
        // Get the Animator component attached to this GameObject
        animator = GetComponent<Animator>();
    }

    void Update()
    {
        // Check if the A key is pressed down
        if (Input.GetKeyDown(KeyCode.A))
        {
            // Set the Animator parameter to play the moving left animation
            animator.SetBool("isMoving", true);
        }
        else if (Input.GetKeyUp(KeyCode.A))
        {
            // Stop the moving left animation when the A key is released
            animator.SetBool("isMoving", false);
        }

        // Check if the D key is pressed down
        if (Input.GetKeyDown(KeyCode.D))
        {
            // Set the Animator parameter to play the moving right animation
            animator.SetBool("isMoving", true);
        }
        else if (Input.GetKeyUp(KeyCode.D))
        {
            // Stop the moving right animation when the D key is released
            animator.SetBool("isMoving", false);
        }
    }
}
