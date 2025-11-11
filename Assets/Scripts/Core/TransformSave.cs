using System.Collections.Generic;
using UnityEngine;

namespace Utilities.Core
{
    /// <summary>
    /// Provides a convient way of saving and restoring the position, rotation, and scale of a transform.
    /// </summary>
    public class TransformSave
    {
        public Vector3 Position;
        public Quaternion Rotation;
        public Vector3 Scale;
        
        /// <summary>
        /// Will save the current position, rotation, and scale of the transform.
        /// </summary>
        /// <remarks>
        /// <br/>If local is true, it will save the local position, rotation, and scale.
        /// <br/>If local is false, it will save the world position and rotation, but still the local scale.
        /// </remarks>
        public TransformSave(Transform transform, bool local = false)
        {
            if (local)
            {
                Position = transform.localPosition;
                Rotation = transform.localRotation;
                Scale = transform.localScale;
            }
            else
            {
                Position = transform.position;
                Rotation = transform.rotation;
                Scale = transform.localScale;
            }
        }
        
        /// <summary>
        /// Will apply the saved position, rotation, and scale to the transform.
        /// </summary>
        /// <remarks>
        /// <br/>If local is true, it will apply the local position, rotation, and scale.
        /// <br/>If local is false, it will apply the world position and rotation, but still the local scale.
        /// </remarks>
        public void Apply(Transform transform, bool local = false)
        {
            if (local)
            {
                transform.SetLocalPositionAndRotation(Position, Rotation);
            }
            else
            {
                transform.SetPositionAndRotation(Position, Rotation);
            }

            transform.localScale = Scale;
        }
        
        /// <summary>
        /// Will go through and save the global transforms of all children of the specified parent transform.
        /// This will not include the parent itself.
        /// Will return a dictionary where the key is the child transform and the value is the TransformSave.
        /// </summary>
        public static Dictionary<Transform, TransformSave> CreateParentSave(Transform parent)
        {
            var saves = new Dictionary<Transform, TransformSave>();
            foreach (Transform child in parent)
            {
                saves[child] = new TransformSave(child);
            }
            return saves;
        }
    }
}