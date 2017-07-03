using UnityEngine;
using System.Collections;

namespace MRW.AnimationSystem {

    public enum AnimationStateType {
        Idle, Running
    }

    [AddComponentMenu("MRW/Animation System/Animation Ease In Out")]
    public abstract class AnimationBase : MonoBehaviour {

        [SerializeField]
        protected Transform target;
        [SerializeField]
        protected AnimationStateType state;
        [SerializeField]
        protected bool loop = false;
        [SerializeField]
        protected AnimationState[] states;

#if UNITY_EDITOR
        private EditorCoroutine editorPlay;
#endif

        private bool initDone = false;

        private Coroutine animationRoutine;

        public abstract void Action(float time, AnimationCurve animationCurve);

        public virtual void OnStart() { }

        public virtual void OnSave() { }
        
        public virtual void OnReset() { }

        public virtual void OnPlay(AnimationState state) { }

        public virtual void BeforeAction() { }

        private void OnEnable() {
            // only trigger OnEnable if it's during the game
            if(initDone) {
                Init();
            }
        }
        
        private void OnDisable() {
            if (animationRoutine != null) StopCoroutine(animationRoutine);
        }

        public void SetTarget(Transform target) {
            this.target = target;
        }

        private void Start() {

            if (target == null) {
                target = GetComponent<Transform>();
            }

            OnStart();

            if (initDone == false) {
                Init();
            }
        }

        private void Init() {

            initDone = true;

            if(state == AnimationStateType.Running) {
                PlayAllStates();
            } 


        }

        public void Stop() {
            if (!enabled) return;

            state = AnimationStateType.Idle;
            if (animationRoutine != null) StopCoroutine(animationRoutine);
        }

        public void PlayAllStates() {
            if (!enabled) return;

            Stop();
            animationRoutine = StartCoroutine(IPlayAllStates());
        }

        public bool StateExists(int state) {
            return states.Length > state && states[state] != null;
        }

        private IEnumerator IPlayAllStates() {

            state = AnimationStateType.Running;

            bool oneLoop = true;

            while (oneLoop) {
                for (int i = 0; i < states.Length; i++) {

                    if (states[i].IsProcessable()) {
                        OnPlay(states[i]);
                        yield return animationRoutine = StartCoroutine(states[i].Process(this, Action));
                    }
                }

                if (loop == false) oneLoop = false;
            }

            state = AnimationStateType.Idle;
        }

        public void PlayState(int nr) {
            if (!enabled) return;

            Stop();
            animationRoutine = StartCoroutine(IPlayState(nr));
        }

        private IEnumerator IPlayState(int nr) {

            state = AnimationStateType.Running;

            if (states[nr].IsProcessable()) {
                OnPlay(states[nr]);
                yield return animationRoutine = StartCoroutine(states[nr].Process(this, Action));
            }

            state = AnimationStateType.Idle;
        }

#if UNITY_EDITOR
        public void PlayInEditor(int state) {
            OnSave();
            OnStart();
            OnPlay(states[state]);
            editorPlay = EditorCoroutine.StartCoroutine(states[state].ProcessInEditor(Action));
        }

        public void PlayInEditorReset() {
            if (editorPlay != null) editorPlay.Stop();
            OnReset();
        }
#endif

    }
}

