using UnityEngine;

namespace FantasyRobbery.Scripts.Abstraction
{
    public abstract class Activation
    {
        public abstract KeyCode ActivationKeyCode { get; }
        public abstract void ActivationAction(IInteractor interactor);
    }
}