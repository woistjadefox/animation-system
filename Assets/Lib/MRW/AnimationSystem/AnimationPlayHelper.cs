using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace MRW.AnimationSystem {
    public class AnimationPlayHelper : MonoBehaviour {

        [SerializeField]
        private Animation target;

        [SerializeField]
        private UnityEvent onPlay;

        [SerializeField]
        private UnityEvent onFinish;

        private YieldInstruction durationYield;
        private Coroutine waitForEndRoutine;

        private void Awake() {
            if (target == null) {
                target = GetComponent<Animation>();
            }

            durationYield = new WaitForSeconds(target.clip.length);
        }

        public void Play() {
            target.Play();

            if (onPlay.GetPersistentEventCount() > 0) onPlay.Invoke();

            if(onFinish.GetPersistentEventCount() > 0) {
                if (waitForEndRoutine != null) StopCoroutine(waitForEndRoutine);
                waitForEndRoutine = StartCoroutine(WaitForEnd());
            }
        }

        private IEnumerator WaitForEnd() {
            yield return durationYield;
            onFinish.Invoke();
        }

    }
}

