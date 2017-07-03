using UnityEngine;
using System.Collections;


namespace MRW.AnimationSystem {

    [AddComponentMenu("MRW/Animation System/Animation Fixed Position")]
    public sealed class AnimationFixedPosition : AnimationBase {

        [SerializeField, Range(0, 1), HideInInspector]
        private float position = 0;

        [SerializeField, HideInInspector]
        private float _lastPos = 0;

        [SerializeField]
        private bool localPosition = true;

        [SerializeField]
        private bool resetStartPosOnPlay = false;

        [SerializeField]
        private Vector3 targetPos;

        private Vector3 startPos;
        private Vector3 endPos;

        public override void OnStart() {
            startPos = localPosition ? target.localPosition : target.position;
            endPos = targetPos;
        }

        public override void OnPlay(AnimationState state) {
            if (resetStartPosOnPlay) {
                OnStart();
            }
        }

        public override void OnSave() {

        }

        public override void OnReset() {

            if(localPosition) {
                target.localPosition = startPos;
            } else {
                target.position = startPos;
            }
 
        }

        public override void Action(float time, AnimationCurve animationCurve) {

            if (localPosition) {
                target.localPosition = Vector3.Lerp(startPos, endPos, animationCurve.Evaluate(time));
            } else {
                target.position = Vector3.Lerp(startPos, endPos, animationCurve.Evaluate(time));
            }

        }

        private void OnValidate() {

            if (position != _lastPos) {
                _lastPos = position;
            }
        }

    }
}

