using UnityEngine;
using System.Collections;

namespace Okomotive.AnimationSystem {

    public abstract class AnimationBlink : MonoBehaviour {

        [SerializeField]
        protected bool activeOnStart = false;
        [SerializeField]
        protected bool blink = false;
        [SerializeField]
        protected float blinkSpeed = 1f;
        [SerializeField]
        protected AnimationCurve blinkEaseInCurve = AnimationCurve.Linear(0, 0, 1, 1);
        [SerializeField]
        protected AnimationCurve blinkEaseOutCurve = AnimationCurve.Linear(0, 1, 1, 0);

        protected abstract IEnumerator BlinkOn();
        protected abstract IEnumerator BlinkOff();

        private bool isOn = false;

        private void Start() {

            OnStart();

            if (activeOnStart) {
                SetOn();
            }
        }

        public bool IsOn() {
            return isOn;
        }

        public float GetBlinkSpeed() {
            return blinkSpeed;
        }

        public void SetBlinkSpeed(float speed) {
            blinkSpeed = speed;
        }

        protected virtual void OnStart() { }

        public virtual void SetOn() {
            if (enabled == false || gameObject.activeSelf == false || gameObject.activeInHierarchy == false) return;
            StopAllCoroutines();
            StartCoroutine(BlinkOn());
            isOn = true;
        }

        public virtual void SetOff() {
            if (enabled == false || gameObject.activeSelf == false || gameObject.activeInHierarchy == false) return;
            StopAllCoroutines();
            StartCoroutine(BlinkOff());
            isOn = false;
        }

        public void SetBlinkEaseInCurve(AnimationCurve curve) {
            blinkEaseInCurve = curve;
        }

        public void SetBlinkEaseOutCurve(AnimationCurve curve) {
            blinkEaseOutCurve = curve;
        }

        public void Toggle() {
            if(isOn) {
                SetOff();
            } else {
                SetOn();
            }
        }

    }
}

