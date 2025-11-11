namespace Utilities.Core
{
    /// <summary>
    /// Provides an interface for objects that can be reset to their default state when
    /// a build occurs.
    /// This is useful for ensuring that objects are in a known state before a build is created.
    /// Should only be used by scriptable objects.
    /// </summary>
    public interface IBuildResetable
    {
        /// <summary>
        /// Will reset the object back to its default values
        /// </summary>
        public void ResetToDefault();
    }
}