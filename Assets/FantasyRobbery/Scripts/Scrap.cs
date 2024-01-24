using UnityEngine;

namespace FantasyRobbery.Scripts
{
    public class Scrap : MonoBehaviour, IInteractable
    {
        public int InstanceId => gameObject.GetInstanceID();
        public float InteractDuration { get; set; } = 0;
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
            if (interactor is NetworkPlayer player)
            {
                //player.PickUp(this);
                Debug.Log($"Grabbed : {gameObject.name}");
            }
        }
    }
}