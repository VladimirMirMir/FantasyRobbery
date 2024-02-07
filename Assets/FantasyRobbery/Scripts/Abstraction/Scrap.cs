using FishNet.Object;
using UnityEngine;

namespace FantasyRobbery.Scripts.Abstraction
{
    public class Scrap : NetworkBehaviour, IInteractable, ICanBeGrabbed
    {
        [SerializeField] private int minCost = 0;
        [SerializeField] private int maxCost = 0;

        private Transform _cachedTransform;
        
        public int InstanceId => gameObject.GetInstanceID();
        public Transform CachedTransform => _cachedTransform ??= transform;
        [field: SerializeField] public float InteractDuration { get; set; } = 0;
        
        public void OnFocus()
        {
            Debug.Log($"Press \"E\" to grab : {gameObject.name}");
        }

        public void OnUnfocus()
        {
            Debug.Log($"Lost focus on {gameObject.name}");
        }

        public void OnInteractionStart(IInteractor interactor)
        {
            Debug.Log($"Interaction with {gameObject.name} starts.");
        }

        public void OnInteractionEnd(IInteractor interactor)
        {
            if (interactor is IGrabber grabber)
                grabber.Grab(this);
        }
    }
}