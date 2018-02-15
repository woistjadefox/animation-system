using UnityEngine;
using System.Collections;

namespace Okomotive.AnimationSystem {

    public enum AnimationStateType {
        Idle, Running
    }

    public enum AnimationStep {
        FixedUpdate, Update
    }

    public abstract class AnimationBase : MonoBehaviour {

        [SerializeField]
        protected Transform target;
        [SerializeField]
        protected AnimationStateType state;
        [SerializeField]
        protected bool loop = false;
        [SerializeField]
        protected AnimationState[] states;
        [SerializeField]
        protected bool dontInterrupt = false;
        [SerializeField]
        protected AnimationStep step;

#if UNITY_EDITOR
        private EditorCoroutine editorPlay;
#endif

        private bool initDone = false;

        private Coroutine animationRoutine;

        private Coroutine singleStateRoutine;

        public abstract void Action(float time, float speed, AnimationCurve animationCurve);

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
            Stop();
        }

        public void SetTarget(Transform target) {
            this.target = target;
        }

        public virtual void Awake() {

            if (target == null) {
                target = GetComponent<Transform>();
            }

            OnStart();
        }

        private void Start() {

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
            if (animationRoutine != null) {
                StopCoroutine(animationRoutine);
                StopCoroutine(singleStateRoutine);
            }
        }

        public void StopAndDisable() {
            Stop();
            enabled = false;
        }

        public void PlayAllStates() {
            if (enabled == false || gameObject.activeSelf == false || gameObject.activeInHierarchy == false) return;
            if (dontInterrupt && state == AnimationStateType.Running) return;

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
                        yield return singleStateRoutine = StartCoroutine(states[i].Process(this, step, Action));
                    }
                }

                if (loop == false) oneLoop = false;
            }

            state = AnimationStateType.Idle;
        }

        public void PlayState(int nr) {
            if (enabled == false || gameObject.activeSelf == false || gameObject.activeInHierarchy == false) return;
            if (dontInterrupt && state == AnimationStateType.Running) return;

            Stop();
            animationRoutine = StartCoroutine(IPlayState(nr));
        }

        public Coroutine GetAnimationRoutine() {
            return animationRoutine;
        }

        private IEnumerator IPlayState(int nr) {

            state = AnimationStateType.Running;

            if (states[nr].IsProcessable()) {
                OnPlay(states[nr]);
                yield return singleStateRoutine = StartCoroutine(states[nr].Process(this, step, Action));
            }

            state = AnimationStateType.Idle;
        }

#if UNITY_EDITOR
        public void EditorPlayState(int state) {
            OnSave();
            OnStart();
            OnPlay(states[state]);
            editorPlay = EditorCoroutine.StartCoroutine(states[state].ProcessInEditor(Action));
        }

        public void EditorPlayReset() {
            if (editorPlay != null) editorPlay.Stop();
            OnReset();
        }
#endif

    }
}

