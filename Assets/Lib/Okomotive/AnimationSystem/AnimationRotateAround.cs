using UnityEngine;

namespace Okomotive.AnimationSystem {

    /* Note:
     * AnimaitonCurves of AnimationStates do not affect the movement at the moment!
     */

    [AddComponentMenu("Okomotive/Animation System/Animation Rotate Around")]
    public sealed class AnimationRotateAround : AnimationBase {

        [SerializeField, Range(0, 1), HideInInspector]
        private float position = 0;

        [SerializeField, HideInInspector]
        private float _lastPos = 0;

        [SerializeField]
        private Transform point;
        [SerializeField]
        private Vector3 axis;
        [SerializeField]
        private float angle;

        //private float normalStep;

        
        private float startAngle;
        private Vector3 startPos;
        private Quaternion startRot;
        private Vector3 endPos;
        private Quaternion endRot;
        

        private void OnValidate() {

            if (position != _lastPos) {
                _lastPos = position;
                //target.rotation = Quaternion.Lerp(target.rotation, Quaternion.Euler(targetRot), position);
            }
        }

        public override void OnStart() {

        }

        public override void OnPlay(AnimationState state) {

            //normalStep = angle / (state.GetTime() * (1 / Time.deltaTime));

     
            startPos = target.position;
            startRot = target.rotation;

            target.RotateAround(point.position, axis, angle);

            endPos = target.position;
            endRot = target.rotation;

            target.position = startPos;
            target.rotation = startRot;
            
        }

        public override void OnSave() {

        }

        public override void OnReset() {
        }

        public void SetPoint(Transform targetPoint) {
            point = targetPoint;
        }

        public void SetAngle(float targetAngle) {
            angle = targetAngle;
        }

        public override void Action(float time, float speed, AnimationCurve animationCurve) {

            //target.RotateAround(point.position, axis, normalStep);

       
            Vector3 targetPos = Vector3.Slerp(startPos, endPos, animationCurve.Evaluate(time));
            Quaternion targetRot = Quaternion.Slerp(startRot, endRot, animationCurve.Evaluate(time));
            target.SetPositionAndRotation(targetPos, targetRot);
            
        }

    }
}

