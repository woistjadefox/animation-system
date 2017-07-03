using UnityEngine;
using System.Collections;


namespace MRW.AnimationSystem {

    [AddComponentMenu("MRW/Animation System/Animation Fixed Rotation")]
    public sealed class AnimationFixedRotation : AnimationBase {


        [SerializeField, Range(0, 1), HideInInspector]
        private float position = 0;

        [SerializeField, HideInInspector]
        private float _lastPos = 0;

        [SerializeField]
        private Vector3 targetRot;

        [SerializeField]
        private bool localRotation = true;

        [SerializeField]
        private bool resetStartRotOnPlay = false;

        private Quaternion startRot;
        private Quaternion endRot;

        public override void OnStart() {

            if (localRotation) {
                startRot = target.localRotation;
            } else {
                startRot = target.rotation;
            }

            endRot = Quaternion.Euler(targetRot);
        }

        public override void OnPlay(AnimationState state) {

            if (resetStartRotOnPlay) {
                OnStart();
            }

        }

        public override void OnSave() {

        }

        public override void OnReset() {

            if (localRotation) {
                target.localRotation = startRot;
            } else {
                target.rotation = startRot;
            }
        }

        public override void Action(float time, AnimationCurve animationCurve) {

            if (localRotation) {
                target.localRotation = Quaternion.Lerp(startRot, endRot, animationCurve.Evaluate(time));
            } else {
                target.rotation = Quaternion.Lerp(startRot, endRot, animationCurve.Evaluate(time));
            }
        }

        private void OnValidate() {

            if (position != _lastPos) {
                _lastPos = position;
                //target.rotation = Quaternion.Lerp(target.rotation, Quaternion.Euler(targetRot), position);
            }
        }

    }
}

