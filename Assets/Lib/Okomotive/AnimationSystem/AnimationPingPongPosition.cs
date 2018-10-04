using UnityEngine;

namespace Okomotive.AnimationSystem {

    [AddComponentMenu("Okomotive/Animation System/Animation PingPong Position")]
    public sealed class AnimationPingPongPosition : AnimationBase {

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
        private Vector3 _endPos;

        public override void OnStart() {
            startPos = localPosition ? target.localPosition : target.position;
            endPos = targetPos;
            _endPos = endPos;
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

            if (target.localPosition == startPos) {
                _endPos = endPos;
            }

            if (target.localPosition == endPos) {
                _endPos = startPos;
            }

            if (localPosition) {
                target.localPosition = Vector3.MoveTowards(target.localPosition, _endPos, speed * Time.deltaTime);
            } else {
                target.localPosition = Vector3.MoveTowards(target.position, _endPos, speed * Time.deltaTime);
            }

        }

        private void OnValidate() {

            if (position != _lastPos) {
                _lastPos = position;
            }
        }

    }
}

