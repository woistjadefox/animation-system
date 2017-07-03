using UnityEngine;
using UnityEngine.Events;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace MRW.AnimationSystem {

    public delegate void AnimationStateAction(float t, AnimationCurve curve);

    [System.Serializable]
    public class AnimationState : ISerializationCallbackReceiver {

        [SerializeField]
        private float time = 1f;
        [SerializeField]
        private float delay = 0f;
        [SerializeField]
        private bool loop = false;
        [SerializeField]
        private bool triggerOnce = false;
        [SerializeField]
        private AnimationCurve curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
        [SerializeField]
        private UnityEvent enterEvents;
        [SerializeField]
        private UnityEvent exitEvents;

        private bool triggered = false;
        private YieldInstruction delayYield;


        public void OnBeforeSerialize() {

        }

        public void OnAfterDeserialize() {

            if (delay > 0f) {
                delayYield = new WaitForSeconds(delay);
            }
        }

        public float GetTime() {
            return time;
        }

        public float GetDelay() {
            return delay;
        }

        public bool IsLoop() {
            return loop;
        }

        public bool IsTriggerOnce() {
            return triggerOnce;
        }

        public AnimationCurve GetCurve() {
            return curve;
        }

        private bool HasDelay() {
            return delay > 0 ? true : false;
        }

        private bool IsTriggered() {
            return triggered;
        }

        public bool IsProcessable() {

            if(triggerOnce && triggered) {
                return false;
            }

            return true;
        }


        public IEnumerator Process(MonoBehaviour m, AnimationStateAction action) {

            bool oneLoop = true;

            while(oneLoop) {

                if (HasDelay()) yield return delayYield;

                enterEvents.Invoke();

                for (float t = 0f; t < 1f; t += Time.deltaTime / time) {

                    if ((t + (Time.deltaTime / time)) > 1f) {
                        t = 1f;
                    }

                    action(t, curve);
                    yield return null;
                }

                exitEvents.Invoke();
                triggered = true;

                if (IsLoop() == false) oneLoop = false;
            }
        }

#if UNITY_EDITOR
        public IEnumerator ProcessInEditor(AnimationStateAction action) {



            float startVal = (float)EditorApplication.timeSinceStartup;

            yield return null;

            float t = 0;
            float diff = 0;

            while (t < 1f) {

                diff = (float)EditorApplication.timeSinceStartup - startVal;
                t += diff / time;

                action(t, curve);

                startVal = (float)EditorApplication.timeSinceStartup;
                yield return null;
            }

            action(1, curve);
        }
#endif

    }
}
