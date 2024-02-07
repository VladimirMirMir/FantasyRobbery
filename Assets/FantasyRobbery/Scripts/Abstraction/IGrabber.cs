namespace FantasyRobbery.Scripts.Abstraction
{
    public interface IGrabber
    {
        bool CanGrab();
        void Grab(ICanBeGrabbed iCanBeGrabbed);
    }
}