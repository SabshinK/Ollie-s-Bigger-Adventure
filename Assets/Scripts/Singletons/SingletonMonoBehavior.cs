using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities.Singleton
{
    /// <summary>
    /// Provides a base class for singletons that inherit from MonoBehaviour.
    /// Note: Singletons are contraversial and should be used with caution. Please refer
    /// to the documentation for best practices.
    /// </summary>
    public abstract class SingletonMonoBehavior<T> : MonoBehaviour where T : SingletonMonoBehavior<T>
    {
        private static bool _instanceSet;
        private static T _instance;

        protected bool WillBeDestroyed;

        /// <summary>
        /// Will return the instance of the singleton. If the instance is not set, it will try to find it in the scene.
        /// </summary>
        public static T Instance
        {
            get
            {
                if (!_instanceSet)
                {
                    _instance = FindAnyObjectByType<T>();
                    _instanceSet = _instance != null;
                }
                return _instance;
            }
        }

        protected virtual void Awake()
        {
            if (Instance == this && _instanceSet)
            {
                if (_instance.transform.parent != null)
                {
                    _instance.transform.parent = null;
                }
                DontDestroyOnLoad(_instance.gameObject);

                SceneManager.sceneLoaded += OnSceneLoaded;
                SceneManager.sceneUnloaded += OnSceneUnloaded;
            }
            else
            {
                Destroy(gameObject);
                WillBeDestroyed = true;
            }
        }

        protected virtual void OnDestroy()
        {
            if (Instance != this) return;

            _instance = null;
            _instanceSet = false;
        }

        protected virtual void OnSceneLoaded(Scene scene, LoadSceneMode mode) { }
        protected virtual void OnSceneUnloaded(Scene scene) { }

        /// <summary>
        /// Will try to find an instance of the singleton and return it.
        /// If no instance is found, it will return null.
        /// </summary>
        public static bool TryGetInstance(out T instance)
        {
            instance = Instance;
            return instance != null;
        }

    }
}
