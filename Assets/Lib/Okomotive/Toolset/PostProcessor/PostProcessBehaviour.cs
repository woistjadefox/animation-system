using UnityEngine;

namespace Okomotive.Toolset.PostProcessor {

    public abstract class PostProcessBehaviour : MonoBehaviour {

        [SerializeField, HideInInspector]
        protected bool postProcessDone = false;

        public void StartPostProcess() {

            if(postProcessDone == false) {
                postProcessDone = true;
                OnPostProcess();
            }
        }

        public abstract void OnPostProcess();

    }
}
