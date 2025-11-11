using UnityEngine;

namespace Utilities.Core
{
    /// <summary>
    /// Provides a base class for UI screens / sub screens to help with page management.
    /// </summary>
    public abstract class ScreenUIBase : MonoBehaviour
    {
        [Header("Base Settings")]
        [Tooltip("The display to toggle the visibility of")]
        [SerializeField] protected GameObject _display;

        /// <summary>
        /// Returns if the display of the sub menu matches the specified game object
        /// </summary>
        public virtual bool IsDisplay(GameObject gameObject) => _display == gameObject;

        /// <summary>
        /// Returns the display that this sub menu is using to show its content
        /// </summary>
        public GameObject DisplayObject => _display;

        /// <summary>
        /// Will peform the display setup operations and make the display visible
        /// </summary>
        public virtual void Display()
        {
            _display.SetActive(true);
        }

        /// <summary>
        /// Will perform the display teardown operations and make the display hidden
        /// </summary>
        public virtual void Hide()
        {
            _display.SetActive(false);
        }

        public void SetVisibility(bool visible)
        {
            if (visible) Display();
            else Hide();
        }
    }
}