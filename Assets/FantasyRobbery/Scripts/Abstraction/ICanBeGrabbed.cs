using UnityEngine;

namespace FantasyRobbery.Scripts.Abstraction
{
    public interface ICanBeGrabbed
    {
        public Transform CachedTransform { get; }
    }
}