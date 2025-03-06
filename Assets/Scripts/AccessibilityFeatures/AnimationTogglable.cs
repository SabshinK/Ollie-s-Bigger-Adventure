using UnityEngine;

namespace AccessibilityFeatures
{
    public class AnimationTogglable : MonoBehaviour
    {
        [SerializeField] private Animator _animator;
        [SerializeField] private SpriteRenderer _spriteRenderer;
        [SerializeField] private Sprite _stillImage;

        private void Reset()
        {
            _animator = GetComponent<Animator>();
            _spriteRenderer = GetComponent<SpriteRenderer>();
        }
        
        public void ToggleAnimation(bool enable)
        {
            if (_animator != null) _animator.enabled = enable;
            if (_spriteRenderer != null) _spriteRenderer.sprite = enable ? _spriteRenderer.sprite : _stillImage;
        }
    }
}