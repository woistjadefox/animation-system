using UnityEngine;
using System.Collections;

namespace Okomotive.AnimationSystem {

    [AddComponentMenu("Okomotive/Animation System/Animation Follow Curve")]
    public sealed class AnimationFollowCurve : AnimationBase {

        [SerializeField, Range(0,1)]
        private float position = 0;

        [SerializeField]
        private bool useAbsolutePositions = false;

        [SerializeField, HideInInspector]
        private float _lastPos = 0;

        [SerializeField]
        private BezierCurve bezierCurve;

		[SerializeField]
		private bool rotateFollowingCurve = false;
		[SerializeField]
		private Vector3 rotationDelta = new Vector3();

        [SerializeField]
        private AnimationState fadeIn;
        private Vector3 fadeInStartPos;
        private Vector3 nextPoint;
        private Quaternion startRotation;
        private Vector3 startPositionDelta;

        private void OnValidate() {
            
            if(position != _lastPos) {
                _lastPos = position;
                target.position = bezierCurve.GetPointAt(position);
            }
        }

        public override void OnSave() {

        }

        public override void OnReset() {
            target.position = bezierCurve.GetPointAt(0) + startPositionDelta;
            target.rotation = startRotation;
        }

        public override void Action(float time, float speed, AnimationCurve animationCurve) {

            if (useAbsolutePositions)
            {
                startPositionDelta = Vector3.zero;
            }

			nextPoint = bezierCurve.GetPointAt(animationCurve.Evaluate(time)) + startPositionDelta;
           

			if(rotateFollowingCurve && nextPoint != target.position){
                target.rotation = Quaternion.LookRotation(nextPoint - target.position) * startRotation * Quaternion.Euler(rotationDelta);
			}

			target.position = nextPoint;

        }

        public void FadeIn() {

            if (!enabled) return;

            fadeInStartPos = target.position;
            StartCoroutine(IFadeIn());
        }

        public IEnumerator IFadeIn() {
            yield return StartCoroutine(fadeIn.Process(this, step, FadeInAction));
        }

        private void FadeInAction(float time, float speed, AnimationCurve curve) {
            target.position = Vector3.Lerp(fadeInStartPos, bezierCurve.GetPointAt(curve.Evaluate(time)), time);
        }

        public float GetCurrentPosition() {
            return position;
        }

        public void MoveToPostion(float pos) {
            position = pos;

            if (position != _lastPos) {
                _lastPos = position;
                target.position = bezierCurve.GetPointAt(position);
            }
        }

        public float GetCurveLenght() {
            return bezierCurve.length;
        }

        public override void OnPlay(AnimationState state)
        {
            startRotation = target.rotation;
            startPositionDelta =  target.position - bezierCurve.GetPointAt(0f);
        }

    }
}

