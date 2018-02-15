using System;
using UnityEngine;

namespace Okomotive.AnimationSystem {

    [System.Serializable]
    public sealed class AnimationStateSpeed : ISerializationCallbackReceiver {

        [SerializeField]
        private float value = 1f;
        [SerializeField]
        private DynamicFloat source;
        [SerializeField]
        private float multiplier = 1f;

        public float GetValue() {

            if(source == null) {
                return value * multiplier;
            }

            return value * (source.GetValue() * multiplier);
        }

        public void OnAfterDeserialize() {
            if(value == default(float)) {
                value = 1f;
            }

            if (multiplier == default(float)) {
                multiplier = 1f;
            }
        }

        public void OnBeforeSerialize() {}
    }
}

