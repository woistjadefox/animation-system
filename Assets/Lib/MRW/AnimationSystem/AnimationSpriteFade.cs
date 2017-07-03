using UnityEngine;
using System.Collections;


namespace MRW.AnimationSystem {

    [AddComponentMenu("MRW/Animation System/Animation Sprite Fade")]
    public sealed class AnimationSpriteFade : AnimationBase {

        [SerializeField]
        private SpriteRenderer sprite;

        [SerializeField, Range(0, 1),]
        private float position = 0;

        [SerializeField, HideInInspector]
        private float _lastPos = 0;        

        [SerializeField]
        private float targetValue;

        private float startValue;
        private float endValue;

        public override void OnStart() {
            startValue = sprite.color.a;
            endValue = targetValue;
        }

        public override void OnSave() {

        }

        public override void OnReset() {
            Color c = sprite.color;
            c.a = startValue;
            sprite.color = c;
        }

        public override void Action(float time, AnimationCurve animationCurve) {

            Color c = sprite.color;
            Color targetC = new Color(c.r, c.g, c.g, Mathf.Lerp(startValue, endValue, animationCurve.Evaluate(time)));
            sprite.color = targetC;

        }

        private void OnValidate() {

            if (position != _lastPos) {
                _lastPos = position;
                Action(position, AnimationCurve.Linear(0f, 0f, 1f, 1f));
                
            }
        }

    }
}

