using System.Collections;
using UnityEngine;

namespace FantasyRobbery.Scripts.Ui
{
    public abstract class Screen : MonoBehaviour
    {
        //TODO : Add analytics here
        protected virtual IEnumerator OnShowStart()
        {
            yield return null;
        }

        protected virtual void OnShowComplete() {}

        private IEnumerator Start()
        {
            yield return OnShowStart();
            OnShowComplete();
        }
    }
}