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
        protected virtual void OnCloseStart() {}
        protected virtual void OnCloseComplete() {}

        public abstract void Initialize(params string[] args);

        private IEnumerator Start()
        {
            yield return OnShowStart();
            OnShowComplete();
        }

        private void OnDestroy()
        {
            OnCloseStart();
            OnCloseComplete();
        }
    }
}