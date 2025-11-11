using UnityEngine;

namespace Utilities.Singleton
{
    /// <summary>
    /// Handles coroutines in a singleton manner, allowing them to be started from anywhere.
    /// Also clears unused resources when the game starts.
    /// </summary>
    public class CoroutineHandler : SingletonMonoBehavior<CoroutineHandler>
    {
        private void Start()
        {
            Resources.UnloadUnusedAssets();
        }
    }
}