#if UNITY_EDITOR

using System.Collections;
using UnityEditor;


namespace Okomotive.AnimationSystem {

    public sealed class EditorCoroutine {
        #region Fields
        private readonly IEnumerator routine;
        #endregion

        #region Constructors
        EditorCoroutine(IEnumerator routine) {
            this.routine = routine;
        }
        #endregion

        #region Public Methods
        public static EditorCoroutine StartCoroutine(IEnumerator _routine) {
            EditorCoroutine coroutine = new EditorCoroutine(_routine);
            coroutine.Start();
            return coroutine;
        }
        #endregion

        #region Private Methods
        private void Start() {
#if DEBUG_EDITORCOROUTINE
            UnityEngine.Debug.Log("Start");
#endif
            EditorApplication.update += Update;
        }

        public void Stop() {
#if DEBUG_EDITORCOROUTINE
            UnityEngine.Debug.Log("Stop");
#endif
            EditorApplication.update -= Update;
        }

        private void Update() {
#if DEBUG_EDITORCOROUTINE
            UnityEngine.Debug.Log("Update");
#endif
            if (!routine.MoveNext()) Stop();
        }

        #endregion
    }
}

#endif