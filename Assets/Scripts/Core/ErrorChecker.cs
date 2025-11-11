using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utilities.Core
{
    /// <summary>
    /// Provides utility methods for error checking and validation of collections and objects.
    /// </summary>
    /// <remarks>
    /// This is a relatively new class I made so still needs to be expanded and used more widely.
    /// </remarks>
    public static class ErrorChecker
    {
        /// <summary>
        /// Will check if the specified index is valid for the given collection.
        /// Logs an error if the index is out of bounds or if the collection is null.
        /// </summary>
        public static bool IsOutOfRange<T>(IEnumerable<T> collection, int newIndex, GameObject context = null)
        {
            if (collection == null)
            {
                Debug.LogError("Collection is null.", context);
                return true;
            }

            if (Mathf.Clamp(newIndex, 0, collection.Count() - 1) != newIndex)
            {
                Debug.LogError($"Invalid index {newIndex} for collection of size {collection.Count()}", context);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if an object is null and logs an error if it is.
        /// </summary>
        public static bool IsNull<T>(T obj, string objName = "", GameObject context = null)
        {
            if (obj == null)
            {
                Debug.LogError($"{(string.IsNullOrEmpty(objName) ? "Object" : objName)} is null.", context);
                return true;
            }
            return false;
        }
        
        /// <summary>
        /// Checks if a collection is null or empty and logs an error if it is.
        /// </summary>
        public static bool IsNullOrEmpty<T>(IEnumerable<T> collection, string collectionName = "", GameObject context = null)
        {
            if (collection == null || !collection.Any())
            {
                Debug.LogError($"{(string.IsNullOrEmpty(collectionName) ? "Collection" : collectionName)} is null or empty.", context);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Checks if a condition is true and logs an error if it is not.
        /// </summary>
        public static bool IsFalse(bool condition, string message, GameObject context = null)
        {
            if (!condition)
            {
                Debug.LogError(message, context);
                return true;
            }
            
            return false;
        }
        
        /// <summary>
        /// Checks if a condition is false and logs an error if it is.
        /// </summary>
        public static bool IsTrue(bool condition, string message, GameObject context = null)
        {
            if (condition)
            {
                Debug.LogError(message, context);
                return true;
            }

            return false;
        }
    }
}