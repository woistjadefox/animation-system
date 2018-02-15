using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using Okomotive.Toolset;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Okomotive.AnimationSystem {

    public delegate void AnimationStateAction(float time, float speed, AnimationCurve curve);


    [System.Serializable]
    public sealed class AnimationState : ISerializationCallbackReceiver {

        [SerializeField]
        private float time;
        [SerializeField]
        private AnimationStateSpeed speed;
        [SerializeField]
        private float delay;
        [SerializeField]
        private bool loop;
        [SerializeField]
        private bool triggerOnce;
        [SerializeField]
        private AnimationCurve curve;
        [SerializeField]
        private UnityEvent enterEvents;
        [SerializeField]
        private UnityEvent exitEvents;

        private bool triggered = false;
        private YieldInstruction delayYield;
        private YieldInstruction fixedUpdateYield;


        public void OnBeforeSerialize() {
        }

        public void OnAfterDeserialize() {

            if (delay > 0f) {
                delayYield = new WaitForSeconds(delay);
            }

            if (curve.Evaluate(0.5f) == 0f && curve.Evaluate(0.14f) == 0f) {
                curve = AnimationCurve.Linear(0f, 0f, 1f, 1f);
            }

            fixedUpdateYield = new WaitForFixedUpdate();
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


        public IEnumerator Process(MonoBehaviour m, AnimationStep step, AnimationStateAction action) {

            triggered = true;
            bool oneLoop = true;

            while(oneLoop) {
                if (HasDelay()) yield return delayYield;

                if(enterEvents.GetPersistentEventCount() > 0) enterEvents.Invoke();

                for (float t = 0f; t < 1f; t += Time.deltaTime / time) {

                    if ((t + (Time.deltaTime / time)) > 1f) {
                        t = 1f;
                    }

                    action(t, speed.GetValue(), curve);

                    if(step == AnimationStep.FixedUpdate) {
                        yield return fixedUpdateYield;
                    } else {
                        yield return null;
                    }

                }

                if (exitEvents.GetPersistentEventCount() > 0) exitEvents.Invoke();

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

                action(t, speed.GetValue(), curve);

                startVal = (float)EditorApplication.timeSinceStartup;
                yield return null;
            }

            action(1, speed.GetValue(), curve);
        }
#endif

    }
}
