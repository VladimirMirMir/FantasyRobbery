using System.Collections.Generic;

namespace FantasyRobbery.Scripts.Abstraction
{
    public interface ICanBeActivated
    {
        public List<Activation> Activations { get; }
    }
}