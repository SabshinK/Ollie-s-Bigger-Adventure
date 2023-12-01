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
    [SerializeField] private GameObject dropShadow;

    [Space]
    [SerializeField] private float flipParticleDuration = 0.5f;

    private CombinedCharacterController cc;

    private void Awake()
    {
        // Get the Animator component attached to this GameObject
        cc = GetComponentInParent<CombinedCharacterController>();
    }

    private void Update()
    {
        float speed = cc.OnGround && !GameManager.IsScripted ? cc.MovementInput.magnitude : 0f;

        animator?.SetFloat("Speed", speed);

        // Set the drop shadow position to the ground, accounting for gravity direction and slope angle
        if (Physics.Raycast(transform.position, -transform.up, out RaycastHit hitInfo))
        {
            dropShadow.transform.localPosition = transform.InverseTransformPoint(hitInfo.point);
            dropShadow.transform.up = hitInfo.normal;
        }
    }

    // Should be subscribed to character controller
    public void FlipDirection()
    {
        StartCoroutine(PlayForSeconds(flipParticleDuration));
    }

    public IEnumerator PlayForSeconds(float seconds)
    {
        particles?.Play();

        yield return new WaitForSeconds(seconds);

        particles?.Stop();
    }
}
