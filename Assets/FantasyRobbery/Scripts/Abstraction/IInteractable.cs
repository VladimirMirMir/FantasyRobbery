namespace FantasyRobbery.Scripts.Abstraction
{
    public interface IInteractable
    {
        public int InstanceId { get; }
        public float InteractDuration { get; set; }
        public bool Hold => InteractDuration > 0;
        public void OnFocus();
        public void OnUnfocus();
        public void OnInteractionStart(IInteractor interactor);
        public void OnInteractionEnd(IInteractor interactor);
    }
}