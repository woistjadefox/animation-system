using UnityEngine;

namespace Okomotive.AnimationSystem {

    [AddComponentMenu("Okomotive/Animation System/Animation Fixed Position")]
    public sealed class AnimationFixedPosition : AnimationBase {

        [SerializeField, Range(0, 1), HideInInspector]
        private float position = 0;

        [SerializeField, HideInInspector]
        private float _lastPos = 0;

        [SerializeField]
        private bool localPosition = true;

        [SerializeField]
        private bool useRigidbody = false;

        [SerializeField]
        private bool resetStartPosOnPlay = false;

        [SerializeField]
        private Vector3 targetPos;

        private Vector3 startPos;
        private Vector3 endPos;
        private Vector3 tmpPos;
        private Rigidbody targetRb;
        private Transform parent;

        public override void OnStart() {
            if (useRigidbody) {
                targetRb = GetComponent<Rigidbody>();
                parent = target.parent;
            }
            startPos = localPosition ? target.localPosition : target.position;
            endPos = targetPos;
        }

        public override void OnPlay(AnimationState state) {
            if (resetStartPosOnPlay) {
                OnStart();
            }
        }

        public override void OnSave() {}

        public override void OnReset() {

            if(localPosition) {
                target.localPosition = startPos;
            } else {
                target.position = startPos;
            }
 
        }

        public override void Action(float time, float speed, AnimationCurve animationCurve) {

            tmpPos = Vector3.Lerp(startPos, endPos, animationCurve.Evaluate(time));

            if (localPosition) {

                if (useRigidbody) {
                    if (parent != null) tmpPos = parent.TransformPoint(tmpPos);
                    targetRb.MovePosition(tmpPos);
                } else {
                    target.localPosition = tmpPos;
                }

            } else {

                if (useRigidbody) {
                    targetRb.MovePosition(tmpPos);
                } else {
                    target.position = tmpPos;
                }
            }

        }

        public void SetTargetPosX(float x) {
            targetPos.x = x;
        }

        public void SetTargetPosY(float y) {
            targetPos.y = y;
        }

        public void SetTargetPosZ(float z) {
            targetPos.z = z;
        }

        private void OnValidate() {

            if (position != _lastPos) {
                _lastPos = position;
            }
        }

    }
}

