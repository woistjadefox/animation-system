using UnityEngine;

namespace Okomotive.AnimationSystem {

    [AddComponentMenu("Okomotive/Animation System/Animation Fixed Scaling")]
    public sealed class AnimationFixedScaling : AnimationBase {

        [SerializeField, Range(0, 1), HideInInspector]
        private float position = 0;

        [SerializeField, HideInInspector]
        private float _lastPos = 0;


        [SerializeField]
        private bool resetStartPosOnPlay = false;

        [SerializeField]
        private Vector3 targetScale;

        private Vector3 startScale;
        private Vector3 endScale;

        public override void OnStart() {
            startScale = target.localScale;
            endScale = targetScale;
        }

        public override void OnPlay(AnimationState state) {
            if (resetStartPosOnPlay) {
                OnStart();
            }
        }

        public override void OnSave() {}

        public override void OnReset() {
            target.localScale = startScale;
        }

        public override void Action(float time, float speed, AnimationCurve animationCurve) {
            target.localScale = Vector3.Lerp(startScale, endScale, animationCurve.Evaluate(time));
        }

        private void OnValidate() {

            if (position != _lastPos) {
                _lastPos = position;
            }
        }

    }
}

