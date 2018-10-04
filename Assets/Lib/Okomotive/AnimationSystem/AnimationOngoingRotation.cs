using UnityEngine;

namespace Okomotive.AnimationSystem {

    [AddComponentMenu("Okomotive/Animation System/Animation Ongoing Rotation")]
    public sealed class AnimationOngoingRotation : AnimationBase {

        [SerializeField, Range(0, 1), HideInInspector]
        private float position = 0;

        [SerializeField, HideInInspector]
        private float _lastPos = 0;

        [SerializeField]
        private Vector3 rotAxis;

        [SerializeField]
        private bool localRotation = true;

        private Quaternion startRot;


        public override void OnStart() {

            if (localRotation) {
                startRot = target.localRotation;
            } else {
                startRot = target.rotation;
            }
        }

        public override void OnPlay(AnimationState state) {}

        public override void OnSave() {}

        public override void OnReset() {

            if (localRotation) {
                target.localRotation = startRot;
            } else {
                target.rotation = startRot;
            }
        }

        public override void Action(float time, float speed, AnimationCurve animationCurve) {


            if (localRotation) {
				target.localRotation *= Quaternion.Euler(rotAxis * speed * animationCurve.Evaluate(time) * (Time.deltaTime * 60f));
            } else {
				target.rotation *= Quaternion.Euler(rotAxis * speed * animationCurve.Evaluate(time) * (Time.deltaTime * 60f));
            }

        }

        public void ReverseAxis() {
            rotAxis = rotAxis * -1;
        }

        private void OnValidate() {

            if (position != _lastPos) {
                _lastPos = position;
            }
        }

    }
}

