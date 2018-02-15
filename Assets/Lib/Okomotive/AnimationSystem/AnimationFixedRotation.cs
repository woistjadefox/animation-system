using UnityEngine;

namespace Okomotive.AnimationSystem {

    [AddComponentMenu("Okomotive/Animation System/Animation Fixed Rotation")]
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
		private float curveValue;
		private Quaternion currentRotation;

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

        public override void Action(float time, float speed, AnimationCurve animationCurve) {

			curveValue = animationCurve.Evaluate(time);

			if(curveValue < 0){
				currentRotation = Quaternion.Lerp(startRot, Quaternion.Euler(-targetRot), -curveValue);
			}else{
				currentRotation = Quaternion.Lerp(startRot, endRot, curveValue);
			}

			if (localRotation) {
				target.localRotation = currentRotation;
            } else {
				target.rotation = currentRotation;
            }
        }

        public void SetTargetRotX(float x) {
            targetRot.x = x;
        }

        public void SetTargetRotY(float y) {
            targetRot.x = y;
        }

        public void SetTargetRotZ(float z) {
            targetRot.z = z;
        }

        private void OnValidate() {

            if (position != _lastPos) {
                _lastPos = position;
                //target.rotation = Quaternion.Lerp(target.rotation, Quaternion.Euler(targetRot), position);
            }
        }

    }
}

