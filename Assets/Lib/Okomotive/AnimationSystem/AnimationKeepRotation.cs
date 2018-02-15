using UnityEngine;

namespace Okomotive.AnimationSystem {

    [AddComponentMenu("Okomotive/Animation System/Animation Keep Rotation")]
    public sealed class AnimationKeepRotation : MonoBehaviour {

        public enum RotationType {
            QuaternionIdentity, StartLocalRotation, LockZ, StartRotation
        }

        public RotationType mode;

        private Quaternion startRot;
        private Quaternion startRotLocal;
        new private Transform transform;

        private void Start() {

            transform = GetComponent<Transform>();
            startRotLocal = transform.localRotation;
            startRot = transform.rotation;
        }

        private void FixedUpdate() {

            switch (mode) {
                case RotationType.QuaternionIdentity:
                    transform.rotation = Quaternion.identity;
                    break;
                case RotationType.StartLocalRotation:
                    transform.localRotation = startRotLocal;
                    break;
                case RotationType.StartRotation:
                    transform.rotation = startRot;
                    break;
                case RotationType.LockZ:
                    startRotLocal = transform.rotation;
                    startRotLocal.z = Quaternion.identity.z;
                    transform.rotation = startRotLocal;
                    break;
            }

        }

    }
}

