using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Utilities.Core
{
    /// <summary>
    /// Provides various common utility functions to prevent needing to rewrite common code.
    /// </summary>
    public static class CommonFunctions
    {
        /// <summary>
        /// Will cause the specified method to be invoked during the next frame of the game.
        /// Remember to use StartCorutine to invoke
        /// </summary>
        public static IEnumerator TriggerNextFrame(Action toTrigger)
        {
            yield return null;
            toTrigger?.Invoke();
        }

        /// <summary>
        /// Will cause the specified method to be invoked after a delay.
        /// </summary>
        public static IEnumerator TriggerAfterDelay(float delay, Action toTrigger)
        {
            yield return new WaitForSeconds(delay);
            toTrigger?.Invoke();
        }

        /// <summary>
        /// Will return a list of enums of the type T where T is an Enum.
        /// </summary>
        public static List<T> GetEnumValues<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            var valuesAsList = new List<T>();

            foreach (var value in values)
            {
                valuesAsList.Add((T)value);
            }

            return valuesAsList;
        }

        /// <summary>
        /// Will return a list of all objects of a specfic type in a specific scene. If none specified,
        /// then will return all objects of that type in all active scenes.
        /// </summary>
        public static List<T> GetAllObjects<T>(bool includeInactive = true, Scene scene = default) where T : MonoBehaviour
        {
            var mode = includeInactive ? FindObjectsInactive.Include : FindObjectsInactive.Exclude;
            var objects = UnityEngine.Object.FindObjectsByType<T>(mode, FindObjectsSortMode.None).ToList();
            if (scene != default)
            {
                objects = objects
                    .Where(x => scene.GetRootGameObjects().Contains(x.transform.root.gameObject))
                    .ToList();
            }

            return objects;
        }

        /// <summary>
        /// Multiplies a vector by another vector component-wise.
        /// Math: A * B = (A.x * B.x, A.y * B.y, A.z * B.z)
        /// Allows for doing myVector.Multiply(otherVector) instead of Vector3.Scale(myVector, otherVector).
        /// </summary>
        public static Vector3 Multiply(this Vector3 vector, Vector3 multiplier)
        {
            return Vector3.Scale(vector, multiplier);
        }

        /// <summary>
        /// Converts a Color to its hexadecimal string representation since its hard to remember that
        /// ColorUtility exists. Same though as ColorUtility.ToHtmlStringRGB.
        /// Example: Color.red will return "FF0000".
        /// </summary>
        public static string ColorToHex(Color color)
        {
            return ColorUtility.ToHtmlStringRGB(color);
        }

        /// <summary>
        /// Will take a hex code and return a Color object.
        /// Example: "#FF0000" will return Color.red.
        /// If the hex code is invalid, it will return Color.clear and log an error.
        /// Also accepts hex codes without the '#' prefix.
        /// </summary>
        public static Color ColorFromHex(string hexCode)
        {
            var modified = hexCode.StartsWith("#") ? hexCode : "#" + hexCode;

            if (ColorUtility.TryParseHtmlString(modified, out var color))
            {
                return color;
            }

            Debug.LogError($"Failed to parse color from hex: {hexCode}");
            return Color.clear;
        }

        /// <summary>
        /// Will darken a color by a specified amount.
        /// </summary>
        public static Color Darken(this Color color, float amount)
        {
            amount = Mathf.Clamp01(amount);
            var offset = 1 - amount;
            color.r *= offset;
            color.g *= offset;
            color.b *= offset;
            return color;
        }

        /// <summary>
        /// Will return a list of all children of a specific type in a parent transform.
        /// Will not include the parent itself.
        /// If includeInactive is true, it will include inactive children as well.
        /// If includeInactive is false, it will only include active children.
        /// </summary>
        public static List<T> GetAllChildren<T>(Transform parent, bool includeInactive = true) where T : Component
        {
            ErrorChecker.IsNull(parent, objName: nameof(parent));

            var children = new List<T>();
            foreach (Transform child in parent)
            {
                if (child == parent) continue; // Skip the parent itself

                if (includeInactive || child.gameObject.activeInHierarchy)
                {
                    if (child.TryGetComponent<T>(out var component))
                    {
                        children.Add(component);
                    }
                    children.AddRange(GetAllChildren<T>(child, includeInactive));
                }
            }

            return children;
        }

        /// <summary>
        /// Will check if the specified index is within the bounds of the list.
        /// Returns true if the index is valid, false otherwise.
        /// </summary>
        public static bool ContainsIndex<T>(this IList<T> list, int index)
        {
            return index >= 0 && index < list.Count;
        }

        /// <summary>
        /// If the index is valid, will return the element at that index.
        /// If the index is invalid, will return the default value for the type T.
        /// </summary>
        public static T GetElementOrDefault<T>(this IList<T> list, int index)
        {
            if (list.ContainsIndex(index))
            {
                return list[index];
            }

            return default;
        }

        /// <summary>
        /// Will check if the specified index is within the bounds of the list and if there is an element after it.
        /// Returns true if the index is valid and there is an element after it, false otherwise.
        /// </summary>
        public static bool ContainsIndexAfter<T>(this IList<T> list, int index)
        {
            return index >= 0 && index < list.Count - 1;
        }
        
        /// <summary>
        /// An extension method for IEnumerables for simple foreach loops
        /// </summary>
        public static void ForEach<T>(this IEnumerable<T> list, Action<T> action)
        {
            foreach (var item in list)
            {
                action(item);
            }
        }
    }
}