using UnityEngine;
using UnityEngine.Events;

namespace Circle
{
    /*
     * Simple class that holds an event to be called when an animation event fires, since animation events can
     * only call functions on the objects/children those animations are attached to :/
     */
    public class AnimationEventHandler : MonoBehaviour
    {
        public UnityEvent animEvent;

        private void Invoke()
        {
            animEvent?.Invoke();
        }
    }
}
