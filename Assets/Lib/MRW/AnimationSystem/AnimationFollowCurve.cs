using UnityEngine;
using System.Collections;


namespace MRW.AnimationSystem {

    [AddComponentMenu("MRW/Animation System/Animation Follow Curve")]
    public sealed class AnimationFollowCurve : AnimationBase {

        [SerializeField, Range(0,1)]
        private float position = 0;

        [SerializeField, HideInInspector]
        private float _lastPos = 0;

        [SerializeField]
        private BezierCurve bezierCurve;

        [SerializeField]
        private AnimationState fadeIn;
        private Vector3 fadeInStartPos;

        private void OnValidate() {
            
            if(position != _lastPos) {
                _lastPos = position;

                target.position = bezierCurve.GetPointAt(position);
            }
        }

        public override void OnSave() {

        }

        public override void OnReset() {
            target.position = bezierCurve.GetPointAt(0);
        }

        public override void Action(float time, AnimationCurve animationCurve) {
            target.position = bezierCurve.GetPointAt(animationCurve.Evaluate(time));
        }

        public void FadeIn() {

            if (!enabled) return;

            fadeInStartPos = target.position;
            StartCoroutine(IFadeIn());
        }

        public IEnumerator IFadeIn() {
            yield return StartCoroutine(fadeIn.Process(this, FadeInAction));
        }

        private void FadeInAction(float time, AnimationCurve curve) {
            target.position = Vector3.Lerp(fadeInStartPos, bezierCurve.GetPointAt(0f), time);
        }

    }
}

